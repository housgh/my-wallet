using Transactions.Grpc;
using MyWallet.Common.RabbitMq;
using Transactions.Infrastructure;
using Transactions.Application;
using Transactions.Grpc.Services;
using MyWallet.Common.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddGrpc();


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TransactionGrpcMapper>());

builder.Services.AddHostedService<Worker>();
builder.Services.AddTransactionsApplicationServices();
builder.Services.AddRabbitMqConsumer(opt =>
{
    opt.HostName = builder.Configuration[$"{nameof(RabbitMqConnectionSettings)}:{nameof(RabbitMqConnectionSettings.HostName)}"]!;
    opt.Port = builder.Configuration.GetValue<int>($"{nameof(RabbitMqConnectionSettings)}:{nameof(RabbitMqConnectionSettings.Port)}");
    opt.UserName = builder.Configuration[$"{nameof(RabbitMqConnectionSettings)}:{nameof(RabbitMqConnectionSettings.UserName)}"]!;
    opt.Password = builder.Configuration[$"{nameof(RabbitMqConnectionSettings)}:{nameof(RabbitMqConnectionSettings.Password)}"]!;
});

builder.Services.AddTransactionInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TransactionService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
