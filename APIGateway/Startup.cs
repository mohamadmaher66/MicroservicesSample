using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Errors.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;
using System.IO;

namespace APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                ExceptionHandler.HandleException(context);
            }));

            //app.UseOcelot();
            app.UseOcelot((ocelotBuilder, ocelotConfiguration) =>
            {
                // this sets up the downstream context and gets the config
                app.UseDownstreamContextMiddleware();
                // This is registered to catch any global exceptions that are not handled
                // It also sets the Request Id if anything is set globally
                ocelotBuilder.UseExceptionHandlerMiddleware();
                // This is registered first so it can catch any errors and issue an appropriate response
                //ocelotBuilder.UseResponderMiddleware();
                ocelotBuilder.UseMiddleware<CustomResponseMiddleware>();
                ocelotBuilder.UseDownstreamRouteFinderMiddleware();
                ocelotBuilder.UseMultiplexingMiddleware();
                ocelotBuilder.UseDownstreamRequestInitialiser();
                ocelotBuilder.UseRequestIdMiddleware();
                //  Custom middleware , Simulate a situation without permission 
                //ocelotBuilder.Use((ctx, next) =>
                //{
                //    ctx.Items.SetError(new UnauthorizedError("No permission"));
                //    return Task.CompletedTask;
                //});
                //ocelotBuilder.UseMiddleware<UrlBasedAuthenticationMiddleware>();
                ocelotBuilder.UseLoadBalancingMiddleware();
                ocelotBuilder.UseDownstreamUrlCreatorMiddleware();
                ocelotBuilder.UseHttpRequesterMiddleware();
            }).Wait();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
