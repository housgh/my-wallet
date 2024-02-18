namespace MyWallet.Common.RabbitMq;

public class RabbitMqConnectionSettings
{
    public RabbitMqConnectionSettings(Action<RabbitMqConnectionSettings> action)
    {
        action(this);
    }

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
