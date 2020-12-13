using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RVT.LoadBalancer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RVT.LoadBalancer.Application", Version = "v1" });
            });

            services.AddSingleton<RabbitMQQueueConnection>(opt =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["QueueHost"],
                    UserName = Configuration["RabbitMQUsername"],
                    Password = Configuration["RabbitMQPassword"],
                    Port= Convert.ToInt32(Configuration["RabbotMQPort"])
                };
                return new RabbitMQQueueConnection(factory);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RVT.LoadBalancer.Application v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitListener();
        }
    }
    public static class ApplicationBuilderExtensions
    {
        public static RabbitMQQueueConnection Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder application)
        {
            Listener = application.ApplicationServices.GetRequiredService<RabbitMQQueueConnection>();

            var liftime = application.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            liftime.ApplicationStarted.Register(OnStarted);
            liftime.ApplicationStopping.Register(OnStopping);

            return application;
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }

        private static void OnStarted()
        {
            Listener.InitReceiverChannel();
        }
    }

}
