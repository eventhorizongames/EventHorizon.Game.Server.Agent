namespace EventHorizon.Game.Server.Agent.State
{
    public interface ServerState
    {
        bool IsServerStarted { get; }
        void SetIsServerStarted(bool isServerStarted);
    }
}