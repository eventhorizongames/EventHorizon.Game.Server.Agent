namespace EventHorizon.Game.Server.Agent.State.Event
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Performance;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class PersistAgentStateHandler : INotificationHandler<PersistAgentStateEvent>
    {
        private static string FILE_DIRECTORY = "App_Data";
        private static string FILENAME = "App_Data/PersistenceStore.json";

        private readonly ILogger _logger;
        private readonly IPerformanceTracker _performanceTracker;
        private readonly IAgentRepository _agentRepository;

        public PersistAgentStateHandler(
            ILogger<PersistAgentStateHandler> logger,
            IPerformanceTracker performanceTracker,
            IAgentRepository agentRepository
        )
        {
            _logger = logger;
            _performanceTracker = performanceTracker;
            _agentRepository = agentRepository;
        }

        public async Task Handle(
            PersistAgentStateEvent notification,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track(nameof(PersistAgentStateHandler)))
            {
                var agentList = await _agentRepository.All();

                var toSaveAgentList = agentList;
                if (toSaveAgentList.Count() > 0)
                {
                    WriteToFile(
                        JsonSerializer.Serialize(
                            toSaveAgentList
                        )
                    );
                }
            }
        }

        private void WriteToFile(
            string obj
        )
        {
            Directory.CreateDirectory(FILE_DIRECTORY);
            using (var file = File.Create(
                FILENAME
            ))
            {
                file.Write(
                    Encoding.UTF8.GetBytes(
                        obj
                    )
                );
            }
        }
    }
}