using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.State.Event;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Agent.State.Schedule
{
    public class PersistAgentStateScheduledTask : IScheduledTask
    {
        public string Schedule => "*/30 * * * * *"; // Every 30 seconds
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PersistAgentStateScheduledTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                await serviceScope.ServiceProvider.GetService<IMediator>().Publish(new PersistAgentStateEvent());
            }
        }
    }
}