#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Accounts.Api/Accounts.Api.csproj", "Accounts.Api/"]
COPY ["Accounts.Application/Accounts.Application.csproj", "Accounts.Application/"]
COPY ["Accounts.Domain/Accounts.Domain.csproj", "Accounts.Domain/"]
COPY ["MyWallet.Common/MyWallet.Common.csproj", "MyWallet.Common/"]
COPY ["MyWallet.Common.RabbitMq/MyWallet.Common.RabbitMq.csproj", "MyWallet.Common.RabbitMq/"]
COPY ["Accounts.Infrastructure/Accounts.Infrastructure.csproj", "Accounts.Infrastructure/"]
RUN dotnet restore "./Accounts.Api/Accounts.Api.csproj"
COPY . .
WORKDIR "/src/Accounts.Api"
RUN dotnet build "./Accounts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Accounts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounts.Api.dll"]