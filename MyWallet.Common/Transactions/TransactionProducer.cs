using MyWallet.Common.Models;
using MyWallet.Common.RabbitMq;

namespace MyWallet.Common.Transactions;

public interface ITransactionProducer
{
    void SendMessage(TransactionDto transaction);
}

internal class TransactionProducer : ITransactionProducer
{
    private readonly IRabbitMqProducer _rabbitMqProducer;

    public TransactionProducer(IRabbitMqProducer rabbitMqProducer)
    {
        _rabbitMqProducer = rabbitMqProducer;
    }

    public void SendMessage(TransactionDto transaction)
    {
        _rabbitMqProducer.SendMessage(QueueNames.TransactionQueue, transaction);
    }
}
