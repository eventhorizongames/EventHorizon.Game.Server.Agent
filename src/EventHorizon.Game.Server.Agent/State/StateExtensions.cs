namespace EventHorizon.Game.Server.Agent.State
{
    using EventHorizon.Game.Server.Agent.Started;
    using EventHorizon.Game.Server.Agent.State.Event;
    using EventHorizon.Game.Server.Agent.State.Impl;
    using EventHorizon.Game.Server.Agent.State.Timer;
    using EventHorizon.TimerService;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreExtensions
    {
        public static void AddAgentState(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IAgentRepository, AgentRepository>()
                .AddSingleton<ServerState, StandardServerState>()
                .AddSingleton<ITimerTask, PersistAgentStateTimerTask>()
            ;
        }
        public static void UseAgentState(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Publish(
                    new LoadAgentStateEvent()
                ).GetAwaiter().GetResult();
                mediator.Send(
                    new StartServerCommand()
                ).GetAwaiter().GetResult();
            }
        }
    }
}