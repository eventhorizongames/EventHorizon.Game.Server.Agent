using System.Collections.Generic;
using EventHorizon.Game.Server.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Get
{
    public struct GetAgentsByZoneTagEvent : IRequest<IList<AgentEntity>>
    {
        public string ZoneTag { get; set; }
        public GetAgentsByZoneTagEvent(
            string zoneTag
        )
        {
            this.ZoneTag = zoneTag;
        }
    }
}