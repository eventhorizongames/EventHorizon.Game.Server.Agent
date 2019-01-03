using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Get;
using EventHorizon.Game.Server.Agent.Model;
using EventHorizon.Game.Server.Agent.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Agent.Bus
{
    [Authorize]
    public class AgentBus : Hub
    {
        readonly IMediator _mediator;
        public AgentBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IList<AgentEntity>> GetAgentsByZoneTag(string zoneTag)
        {
            return await _mediator.Send(new GetAgentsByZoneTagEvent(zoneTag));
        }

        public async Task<AgentEntity> GetAgent(string id)
        {
            return await _mediator.Send(new GetAgentEvent
            {
                Id = id,
            });
        }

        public async Task UpdateAgent(AgentEntity agent)
        {
            await _mediator.Publish(new UpdateAgentEvent
            {
                Agent = agent,
            });
        }
    }
}