using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;
using Newtonsoft.Json;
using OrderService.Data;
using RabbitMQ.Client;
using Serilog;

namespace OrderService.Sagas
{
    public class OrderSaga : IOrderSaga
    {
        private readonly ILogger logger;
        private readonly IPublishEndpoint publisher;

        public OrderSaga(IPublishEndpoint publisher, ILogger logger)
        {
            this.logger = logger;
            this.publisher = publisher;
        }

        public void CreatePendingOrder(OrderCreated orderCreated)
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //// Declare queue. 
            //// The durable property is set to false to enable the persistence of messages, which ensures that the messages will not be lost
            //// if the RabbitMQ service is stopped.
            //channel.QueueDeclare(
            //    queue: "order-create",
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false,
            //    arguments: null);

            //var message = JsonConvert.SerializeObject(order);
            //var body = Encoding.UTF8.GetBytes(message);

            //var properties = channel.CreateBasicProperties();

            //// Set the channel properties to persist messages.
            //properties.Persistent = true;

            //channel.BasicPublish(
            //    exchange: "",
            //    routingKey: "create-pending-order",
            //    basicProperties: properties,
            //    body: body);

            //    logger.Information("Create pending order event published.");
            //}

            publisher.Publish<OrderCreated>(orderCreated);

            logger.Information("Create pending order event published.");
        }

        public void DebitAccount()
        {

        }

        public void ApproveOrder()
        {

        }

        public void CancelOrder()
        {

        }
    }
}
