using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CustomerService.Receivers
{
    public class Receiver : HostedService
    {
        private readonly ILogger logger;

        public Receiver(ILogger logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "order-create", 
                    durable: true, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    logger.Information(message);

                    // Check customer credit
                    // Raise credit alloctated or credit check failed events
                };

                channel.BasicConsume(queue: "order-create", autoAck: false, consumer: consumer);
                logger.Information("Receiver activated.");
            }
        }
    }
}
