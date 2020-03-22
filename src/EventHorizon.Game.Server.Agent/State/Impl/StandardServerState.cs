namespace EventHorizon.Game.Server.Agent.State.Impl
{
    public class StandardServerState : ServerState
    {
        public bool IsServerStarted { get; private set; } = false;

        public void SetIsServerStarted(
            bool isServerStarted
        )
        {
            IsServerStarted = isServerStarted;
        }
    }
}