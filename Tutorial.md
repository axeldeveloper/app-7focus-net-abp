# Preparando terreno 
## intalando o net no macos monterrey
```bash

# install dotnet
$ brew install --cask dotnet-sdk

$ dotnet --version

# ABP tools
$ dotnet tool install -g Volo.Abp.Studio.Cli

# Entity Framework Core Tools for the .NET Command-Line Interface
$ dotnet tool install --global dotnet-ef

# export PATH  dotnet
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc

# To generate a signing certificate, you can use the following command:
$ dotnet dev-certs https -v -ep openiddict.pfx -p 1265c85b-d13d-4c84-8252-315987c2ab7b

```

# Crie sua solução ABP
## Crie uma pasta vazia, abra um terminal de linha de comando e execute o seguinte comando no terminal:

```bash
# created project
$ abp new BooStore -u angular

# Run `abp install-libs` command on your solution folder to install client-side               
$ abp install-libs

$ dotnet add src/BookStore.EntityFrameworkCore package Volo.Abp.EntityFrameworkCore.SqlServer

$ dotnet list src/BookStore.EntityFrameworkCore package

#configure o sql server
configura options.UseSqlServer();

```

# Instalando o sql servier via docker
```bash 
# run sql contaienr
$ docker compose up -d

# or 
$ docker run \
  --name sqlserver \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='pf766312!' \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

# Migrations - EF
```bash
# initial migration
$ dotnet ef migrations add Initial --project src/BookStore.EntityFrameworkCore --startup-project src/BookStore.DbMigrator

# run and generate migrate
$ dotnet run --project src/BookStore.DbMigrator

# or navigate  folder
$ cd src/BooStore.DbMigrator

$ dotnet run

``` 

# Run o backend local :
```bash

cd src/BooStore.HttpApi.Host
dotnet run
```