using EventHorizon.Game.Server.Agent.State.Event;
using EventHorizon.Game.Server.Agent.State.Impl;
using EventHorizon.Game.Server.Agent.State.Schedule;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Agent.State
{
    public static class CoreExtensions
    {
        public static void AddAgentState(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAgentRepository, AgentRepository>()
                .AddSingleton<IScheduledTask, PersistAgentStateScheduledTask>();
        }
        public static void UseAgentState(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new LoadAgentStateEvent()).GetAwaiter().GetResult();
            }
        }
    }
}