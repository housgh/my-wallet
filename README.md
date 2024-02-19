
# MyWallet

__MyWallet__ is a __Fintech app for managing users and their respective accounts and wallets__.

__MyWallet__ was developed using C# and .Net, and uses an SQL Server database for data persistence, as well as RabbitMQ as a message broker.

## Services

The application is divided into _three_ main services.

The __Users service__, a .NET 8 gRPC service responsible for creating and reading the customers of the system.

The __Transactions service__, a .NET 8 gRPC service responsible for reading and modifying transactions under a specific account/wallet.\
This service also has a background service that consumes messages from RabbitMQ. The background service is mainly used for creating new transactions.

The __Accounts service__, a .NET 8 Web API service responsible for creating, reading, and modifying accounts and wallets. \
The Accounts service communicates with the Users and the Transactions services through gRPC, it also communicates with the Transactions service through RabbitMQ for creating transactions.

## Running the Application

To run the application, we will be using a __docker compose file__ to insure isolation and to make sure that all the necessary assets are available regardless of the machine it is running on it.

First we have to __make sure that the latest Docker Server is installed on your machine__. If it is not installed, we can install it from here: https://docs.docker.com/get-docker/

Once Docker is installed, we can now __run the docker compose file__. To run the docker compose file, navigate to the root of the solution and then run the following command:
```
$ docker compose up -d --build
```

Once all the services are started, we have to __wait for 30 seconds for all the required services to be up and running__, and then we can start our testing.

## Testing the application

__MyWallet__ uses the _OpenAPI_ conventions for the API documentation. Knowing that, we can test the application using the Swagger UI provided.

First we should __open our browser and go to the following link__: http://localhost:5000/Swagger/index.html.

In the Swagger documentation, we will see three sections, __User__ section, __Account__ section, and the __Wallet__ section, each with its own set of endpoints.
