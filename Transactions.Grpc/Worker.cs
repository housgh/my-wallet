using MyWallet.Common.Models;
using MyWallet.Common.RabbitMq;
using MyWallet.Common.Transactions;
using Transactions.Domain.Transaction.Services;

namespace Transactions.Grpc;

public class Worker : BackgroundService
{
    private readonly IServiceScope _scope;
    private IRabbitMqConsumer _consumer;
    private ITransactionService _transactionService;

    public Worker(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        var serviceProvider = _scope.ServiceProvider;
        _consumer = serviceProvider.GetRequiredService<IRabbitMqConsumer>();
        _transactionService = serviceProvider.GetRequiredService<ITransactionService>();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Consume<TransactionDto>(QueueNames.TransactionQueue, AddTransaction);
        return Task.CompletedTask;
    }

    private async void AddTransaction(TransactionDto? transaction)
    {
        Console.WriteLine("Received message from queue");
        if (transaction is null) return;
        transaction.TraceId = Guid.NewGuid();
        await _transactionService.AddAsync(transaction);
    }

    public override void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }
}
