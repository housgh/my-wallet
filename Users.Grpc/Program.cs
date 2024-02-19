using MyWallet.Common.Mappers;
using Users.Grpc.Services;
using Users.Infrastructure;
using Users.Application;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();

Console.WriteLine(builder.Configuration.GetConnectionString("UserDbContext"));

builder.Services.AddGrpc();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CustomerGrpcMapper>());

builder.Services.AddUsersInfrastructureServices(builder.Configuration);
builder.Services.AddUserApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CustomerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
