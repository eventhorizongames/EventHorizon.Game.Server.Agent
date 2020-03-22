using System;
using System.Linq;
using System.Net;
using EventHorizon.Game.Server.Agent.Bus;
using EventHorizon.Game.Server.Agent.Controllers;
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Game.Server.Agent.State;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace EventHorizon.Game.Server.Agent
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Enabled TLS 1.2
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            services.AddHttpClient();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = HostingEnvironment.IsProduction() || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                    options.TokenRetriever = WebSocketTokenRetriever.FromHeaderAndQueryString;
                });
            services.AddMvc();
            services.AddSignalR();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(
                            Configuration
                                .GetSection("Cors:Hosts")
                                .GetChildren()
                                .AsEnumerable()
                                .Select(a => a.Value)
                                .ToArray()
                        )
                        .AllowCredentials();
                }));

            services.AddAgentState(Configuration);

            services.AddSingleton<IPerformanceTracker, PerformanceTracker>();

            services.AddTimer();
            services.AddMediatR(
                typeof(Startup).Assembly
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAgentState();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapHub<AgentBus>("/agent");
            });
        }
    }
}