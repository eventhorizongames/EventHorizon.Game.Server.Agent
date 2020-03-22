using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Model;
using EventHorizon.Game.Server.Agent.State;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Get
{
    public class GetAgentsByZoneTagHandler : IRequestHandler<GetAgentsByZoneTagEvent, IList<AgentEntity>>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentsByZoneTagHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<IList<AgentEntity>> Handle(GetAgentsByZoneTagEvent request, CancellationToken cancellationToken)
        {
            return await _agentRepository.AllByZoneTag(
                request.ZoneTag
            );
        }
    }
}