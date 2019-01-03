using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Bus;
using EventHorizon.Game.Server.Agent.Controllers;
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Game.Server.Agent.State;
using EventHorizon.Game.Server.Agent.State.Impl;
using EventHorizon.Game.Server.Agent.State.Schedule;
using EventHorizon.Schedule;
using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.Game.Server.Agent
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Enabled TLS 1.2
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            services.AddHttpClient();
            services.AddMediatR();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = HostingEnvironment.IsProduction() || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                    options.TokenRetriever = WebSocketTokenRetriever.FromHeaderAndQueryString;
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            services.AddScheduler((sender, args) =>
            {
                Console.WriteLine(args.Exception.Message);
                args.SetObserved();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAgentState();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AgentBus>("/agent");
            });
            app.UseMvc();
        }
    }
}