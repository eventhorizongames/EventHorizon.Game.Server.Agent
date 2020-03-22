namespace EventHorizon.Game.Server.Agent.State.Event.Handler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Agent.Model;
    using EventHorizon.Performance;
    using MediatR;

    public class LoadAgentStateHandler : INotificationHandler<LoadAgentStateEvent>
    {
        private static string FILENAME = "App_Data/PersistenceStore.json";

        private readonly IAgentRepository _agentRepository;
        private readonly IPerformanceTracker _performanceTracker;

        public LoadAgentStateHandler(
            IAgentRepository agentRepository,
            IPerformanceTracker performanceTracker
        )
        {
            _performanceTracker = performanceTracker;
            _agentRepository = agentRepository;
        }

        public async Task Handle(
            LoadAgentStateEvent notification,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track(nameof(LoadAgentStateHandler)))
            {
                var agentRepository = _agentRepository;
                foreach (var agent in ReadFile())
                {
                    await agentRepository.Update(agent);

                }
            }
        }

        private IList<AgentEntity> ReadFile()
        {
            if (!File.Exists(FILENAME))
            {
                return new List<AgentEntity>();
            }
            return JsonSerializer.Deserialize<IList<AgentEntity>>(
                File.ReadAllText(FILENAME)
            );
        }
    }
}