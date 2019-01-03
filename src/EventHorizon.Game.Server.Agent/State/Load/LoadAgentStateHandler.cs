using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Model;
using EventHorizon.Performance;
using MediatR;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Agent.State.Event.Handler
{
    public struct LoadAgentStateHandler : INotificationHandler<LoadAgentStateEvent>
    {
        private static string FILENAME = "App_Data/PersistenceStore.json";

        readonly IAgentRepository _agentRepository;
        readonly IPerformanceTracker _performanceTracker;

        public LoadAgentStateHandler(
            IAgentRepository agentRepository,
            IPerformanceTracker performanceTracker)
        {
            _performanceTracker = performanceTracker;
            _agentRepository = agentRepository;
        }

        public Task Handle(LoadAgentStateEvent notification, CancellationToken cancellationToken)
        {
            using (_performanceTracker.Track(nameof(LoadAgentStateHandler)))
            {
                var agentRepository = _agentRepository;
                Parallel.ForEach<AgentEntity>(ReadFile(), agent => agentRepository.Update(agent));
            }
            return Task.CompletedTask;
        }

        private IList<AgentEntity> ReadFile()
        {

            if (!File.Exists(FILENAME))
            {
                return new List<AgentEntity>();
            }
            return JsonConvert.DeserializeObject<IList<AgentEntity>>(
                File.ReadAllText(FILENAME)
            );
        }
    }
}