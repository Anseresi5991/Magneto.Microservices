using Magneto.Microservice.Orchestrator.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Bus.Commands;
using RabbitMQ.Bus.Events;
using RabbitMQ.Bus.RabbitBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Bus.Implementations
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IConfiguration _configuration;
        private const string HOSTNAME = "RabbitMQ:HostName";
        private const string VHOST = "RabbitMQ:VHost";
        private const string PASSWORD = "RabbitMQ:Password";
        private const string USERNAME = "RabbitMQ:UserName";
        private const string QUEUE = "RabbitMQ:Queue";
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IContextMutant _contextMutant;
        public RabbitEventBus(IMediator mediator, IConfiguration configuration, IContextMutant contextMutant)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _configuration = configuration;
            _contextMutant = contextMutant;
        }
        public Task SendCommands<T>(T comando) where T : Command
        {
            return _mediator.Send(comando);
        }
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name.ToLower();
            var handlerEventType = typeof(TH);
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }
            if (_handlers[eventName].Any(x => x.GetType() == handlerEventType))
            {
                throw new ArgumentException($"El manejador {handlerEventType.Name} fue registrado anteriormente por {eventName}");
            }
            string a = _configuration.GetSection(HOSTNAME).Value;
            _handlers[eventName].Add(handlerEventType);
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection(HOSTNAME).Value,
                VirtualHost = _configuration.GetSection(VHOST).Value,
                Password = _configuration.GetSection(PASSWORD).Value,
                UserName = _configuration.GetSection(USERNAME).Value,
                DispatchConsumersAsync = true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(eventName, true, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);

        }
        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            string nameEvent = _configuration.GetSection(QUEUE).Value;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var subscriptions = _handlers[nameEvent];
            foreach (var sb in subscriptions)
            {
                var handler = Activator.CreateInstance(sb, _contextMutant);
                if (handler == null) continue;
                var eventType = _eventTypes.SingleOrDefault(x => x.Name.ToLower() == nameEvent);
                var eventDS = JsonConvert.DeserializeObject(message, eventType);
                var concretoTipo = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concretoTipo.GetMethod("Handle").Invoke(handler, new object[] { eventDS });
            }
        }
    }
}
