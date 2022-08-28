using CustomerMicroservice.DbContext;
using CustomerMicroservice.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerMicroservice.RabbitMQ
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Hostname"],
                Port = int.Parse(_configuration["RabbitMQ:Port"])
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("Products", exclusive: false);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body;
                string objectAsString = Encoding.UTF8.GetString(body.ToArray());

                // Do anything
                // For example Add new customer with product data
                ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(objectAsString);
                CustomersData.customers.Add(new CustomerDTO()
                {
                    Id = product.Id,
                    FirstName = product.Name,
                    LastName = product.Category,
                });

            };

            _channel.BasicConsume(queue: "Products", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
