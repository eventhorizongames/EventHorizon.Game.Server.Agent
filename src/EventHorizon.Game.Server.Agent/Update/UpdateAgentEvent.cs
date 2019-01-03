using EventHorizon.Game.Server.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Agent.Update
{
    public struct UpdateAgentEvent : INotification
    {
        public AgentEntity Agent { get; set; }
    }
}