CSharp server CSharp client - Experiment
==========================================

This project try to simplify the web development and prototyping for Single Page Applications over existing databases.
The project contains two components, the **Client** and the **Server** which communicate over *HTTP* using a very simple protocol to query the data. This query protocol cover usual cases when it is needed to retrieve data from database. For more complex queries it is needed to write the necessary code on the server and expose the data as a service operation.

Prerequisites
-------------

Item | Version
---- | -------
MySQL | v5.7 or newer;
.NET Core | v2.2 or newer;


Install and running the application
-----------------------------------

Install **sakila** database on local machine following the instructions from [here](https://dev.mysql.com/doc/sakila/en/sakila-installation.html).

Retrieve the application from the repository.

Open command prompt into the **Server** folder.

Restore dependencies:

```bash
dotnet restore
```

Start the server:

```bash
dotnet run --project Server
```

Open command prompt into the **Client** folder.

Restore dependencies:

```bash
dotnet restore
```

Start the client:

```bash
dotnet run --project Client
```
