using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Model;
using EventHorizon.Game.Server.Agent.State;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Get
{
    public class GetAgentHandler : IRequestHandler<GetAgentEvent, AgentEntity>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<AgentEntity> Handle(GetAgentEvent request, CancellationToken cancellationToken)
        {
            return await _agentRepository.FindById(request.Id);
        }
    }
}