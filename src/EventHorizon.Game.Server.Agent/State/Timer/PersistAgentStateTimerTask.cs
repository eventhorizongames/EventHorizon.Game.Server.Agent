namespace EventHorizon.Game.Server.Agent.State.Timer
{
    using EventHorizon.Game.Server.Agent.Started;
    using EventHorizon.Game.Server.Agent.State.Event;
    using EventHorizon.TimerService;
    using MediatR;

    public class PersistAgentStateTimerTask : ITimerTask
    {
        public int Period { get; } = 30000;
        public string Tag { get; } = "PersistAgentStateTimerTask";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new PersistAgentStateEvent();
    }
}