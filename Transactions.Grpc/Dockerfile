#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Transactions.Grpc/Transactions.Grpc.csproj", "Transactions.Grpc/"]
COPY ["MyWallet.Common.RabbitMq/MyWallet.Common.RabbitMq.csproj", "MyWallet.Common.RabbitMq/"]
COPY ["MyWallet.Common/MyWallet.Common.csproj", "MyWallet.Common/"]
COPY ["Transaction.Application/Transactions.Application.csproj", "Transaction.Application/"]
COPY ["Transactions.Domain/Transactions.Domain.csproj", "Transactions.Domain/"]
COPY ["Transactions.Infrastructure/Transactions.Infrastructure.csproj", "Transactions.Infrastructure/"]
RUN dotnet restore "./Transactions.Grpc/Transactions.Grpc.csproj"
COPY . .
WORKDIR "/src/Transactions.Grpc"
RUN dotnet build "./Transactions.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Transactions.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transactions.Grpc.dll"]