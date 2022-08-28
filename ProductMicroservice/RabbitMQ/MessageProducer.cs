using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ProductMicroservice.RabbitMQ
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public MessageProducer(IConfiguration configuration)
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
        public void SendMessage<T>(T message)
        {
            string messageObject = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageObject);

            _channel.BasicPublish(exchange: "", routingKey: "Products", body: body);
        }
    }
}
