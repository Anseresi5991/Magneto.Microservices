using Magneto.Microservice.Orchestrator.Application.Dto;
using Magneto.Microservice.Orchestrator.Application.Main;
using Magneto.Microservice.Orchestrator.Infrastructure.Data;
using Magneto.Microservice.Orchestrator.Infrastructure.Interfaces;
using MediatR;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RabbitMQ.Bus.Implementations;
using RabbitMQ.Bus.RabbitBus;

namespace Magneto.Microservices.Orchestrator
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
            var assembly = AppDomain.CurrentDomain.Load("Magneto.Microservice.Orchestrator.Application.Main");
            services.AddSingleton<IContextMutant, ContextMutant>();
            services.AddTransient<IRabbitEventBus, RabbitEventBus>();
            services.AddTransient<IEventHandler<DemoQueue>, QueueEventHandler>();
            services.AddMediatR(assembly);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Magneto.Microservice.Orchestrator", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Magneto.Microservice.Orchestrator v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<DemoQueue, QueueEventHandler>();
        }
    }
}
