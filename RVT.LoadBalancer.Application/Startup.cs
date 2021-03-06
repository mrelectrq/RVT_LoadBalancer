using AutoMapper;
using Microsoft.AspNetCore.Authentication.Certificate;
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
using RVT.Common.Models;
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
            services.AddAutoMapper(opt =>
            {
                opt.CreateMap<Node, NodeData>();
            });
            services.AddSingleton<CertificateValidationService>();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RVT.LoadBalancer.Application", Version = "v1" });
            });

            services.AddSingleton(opt =>
            {
                var logger = opt.GetRequiredService<ILogger<RabbitMQQueueConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["QueueHost"],
                    UserName = Configuration["RabbitMQUsername"],
                    Password = Configuration["RabbitMQPassword"],
                    Port= 5672
                };
                return new RabbitMQQueueConnection(factory);
            });

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
                    options.Events = new CertificateAuthenticationEvents
                    {
                        OnCertificateValidated = context =>
                        {

                            var validatorService = context.HttpContext.RequestServices.GetService<CertificateValidationService>();

                            if (validatorService.ValidateCertificate(context.ClientCertificate.Thumbprint))
                            {
                                context.Success();
                            }
                            else context.Fail(new Exception("Unauthenticated user. Access diened"));
                            return Task.CompletedTask;
                        }
                    };
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

        [Obsolete]
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder application)
        {
            Listener = application.ApplicationServices.GetRequiredService<RabbitMQQueueConnection>();

            var liftime = application.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IApplicationLifetime>();

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
