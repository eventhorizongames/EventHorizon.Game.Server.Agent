using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.State;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Update
{
    public class UpdateAgentHandler : INotificationHandler<UpdateAgentEvent>
    {
        readonly IAgentRepository _agentRepository;
        public UpdateAgentHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }

        public async Task Handle(UpdateAgentEvent notification, CancellationToken cancellationToken)
        {
            await _agentRepository.Update(notification.Agent);
        }
    }
}