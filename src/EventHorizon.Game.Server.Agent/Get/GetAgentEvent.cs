using EventHorizon.Game.Server.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Get
{
    public struct GetAgentEvent : IRequest<AgentEntity>
    {
        public string Id { get; set; }
    }
}