using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace MyWallet.Common.RabbitMq;

public interface IRabbitMqConsumer
{
    void Consume<TMessage>(string queueName, Action<TMessage?> callback, string exchangeName = "", string exchangeType = "direct");
}

internal class RabbitMqConsumer : IRabbitMqConsumer
{
    private readonly IModel _channel;

    public RabbitMqConsumer(IConnectionFactory connectionFactory)
    {
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public void Consume<TMessage>(string queueName, Action<TMessage?> callback, string exchangeName = "", string exchangeType = ExchangeType.Direct)
    {
        _channel.QueueDeclare(queueName, true, false, autoDelete: false);

        if (!string.IsNullOrEmpty(exchangeName))
        {
            _channel.ExchangeDeclare(exchangeName, exchangeType);
            _channel.QueueBind(queueName, exchangeName, string.Empty);
        }

        var consumer = new EventingBasicConsumer(_channel);
        var deadLetterConsumer = new EventingBasicConsumer(_channel);


        void consumerOnReceived(object? model, BasicDeliverEventArgs ea)
        {
            var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<TMessage>(messageBody);
            callback(message);
        }

        consumer.Received += consumerOnReceived;
        deadLetterConsumer.Received += consumerOnReceived;

        _channel.BasicConsume(queueName, true, consumer);
    }
}
