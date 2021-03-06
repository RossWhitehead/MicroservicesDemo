﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CustomerService.Consumers;
using CustomerService.Receivers;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace CustomerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Customer Service", Version = "v1" });
            });

            var logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(logger);

            // Add Autofac container and register MassTransit
            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc => {
                    var host = sbc.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    sbc.ReceiveEndpoint(host, "OrderCreated", e =>
                    {
                        e.Consumer<OrderCreatedConsumer>();
                    });

                    sbc.UseSerilog(logger);
                });
            })
                .As<IBusControl>()
                .As<IPublishEndpoint>()
                .SingleInstance();

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service v1");
            });

            loggerFactory.AddSerilog();

            //resolve the bus from the container
            var bus = this.ApplicationContainer.Resolve<IBusControl>();

            //start the bus
            var busHandle = TaskUtil.Await(() => bus.StartAsync());

            //register an action to call when the application is shutting down
            appLifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
