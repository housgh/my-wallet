using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace MyWallet.Common.RabbitMq;

public interface IRabbitMqProducer
{
    void SendMessage<TMessage>(string queueName, TMessage model, string exchangeName = "");
}

internal class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IModel _channel;

    public RabbitMqProducer(
        IConnectionFactory connectionFactory)
    {
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public void SendMessage<TMessage>(string queueName, TMessage model, string exchangeName = "")
    {
        var serializedBody = JsonSerializer.Serialize(model);
        var messageBodyBytes = Encoding.UTF8.GetBytes(serializedBody);

        var properties = _channel.CreateBasicProperties();
        properties.ContentType = "application/json";
        properties.Persistent = true;
        properties.Expiration = "60000";

        _channel.BasicPublish(exchangeName, queueName, properties, messageBodyBytes);
    }
}
