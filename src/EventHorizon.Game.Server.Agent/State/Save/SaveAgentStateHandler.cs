using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Performance;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Agent.State.Event
{
    public struct PersistAgentStateHandler : INotificationHandler<PersistAgentStateEvent>
    {
        private static string FILE_DIRECTORY = "App_Data";
        private static string FILENAME = "App_Data/PersistenceStore.json";

        readonly ILogger _logger;
        readonly IPerformanceTracker _performanceTracker;
        readonly IAgentRepository _agentRepository;

        public PersistAgentStateHandler(
            ILogger<PersistAgentStateHandler> logger,
            IPerformanceTracker performanceTracker,
            IAgentRepository agentRepository)
        {
            _logger = logger;
            _performanceTracker = performanceTracker;
            _agentRepository = agentRepository;
        }
        public async Task Handle(PersistAgentStateEvent notification, CancellationToken cancellationToken)
        {
            using (_performanceTracker.Track(nameof(PersistAgentStateHandler)))
            {
                var agentList = await _agentRepository.All();

                var toSaveAgentList = agentList;
                if (toSaveAgentList.Count() > 0)
                {
                    WriteToFile(
                        JsonConvert.SerializeObject(toSaveAgentList)
                    );
                }
            }
        }

        private void WriteToFile(string obj)
        {
            Directory.CreateDirectory(FILE_DIRECTORY);
            using (var file = File.Create(FILENAME))
            {
                file.Write(Encoding.UTF8.GetBytes(obj));
            }
        }
    }
}