# Preparando terreno 
## Instalando o net no macos monterrey
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

# Crie uma Solução ABP
## Crie uma pasta vazia, abra um terminal de linha de comando e execute o seguinte comando no terminal:

```bash
# created project
$ abp new BooStore -u angular

# Run `abp install-libs` command on your solution folder to install client-side               
$ abp install-libs

# add dll sqlserver
$ dotnet add src/BookStore.EntityFrameworkCore package Volo.Abp.EntityFrameworkCore.SqlServer

# list of packages
$ dotnet list src/BookStore.EntityFrameworkCore package

#configure o sql server
configura options.UseSqlServer();

```

# Instalando o sql server via docker
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
# Executando o seed
# Segundo a doc, roda pelo DbMigrator — tanto em dev quanto produção:
```bash
# initial migration
$ dotnet ef migrations add Initial --project src/BookStore.EntityFrameworkCore --startup-project src/BookStore.DbMigrator

# run and generate migrate
$ dotnet run --project src/BookStore.DbMigrator

# or navigate  folder
$ cd src/BooStore.DbMigrator
# run 
$ dotnet run

# add new migrate

$ dotnet ef migrations add Created_Customer_Entity --project src/BookStore.EntityFrameworkCore --startup-project src/BookStore.DbMigrator


``` 

# Run o backend local :
```bash

$ cd src/BooStore.HttpApi.Host
$ dotnet run
```

cria o dto service e depois  application


DESAFIO:  Aqui, gostamos de dar a oportunidade para que todos mostrem o seu trabalho na prática, afinal acreditamos em mais mão na massa e menos blábláblá. Então vamos juntar o útil ao agradável.. 

Conhecer o ABP https://abp.io/  

Implementar o tutorial do ABP Book Store https://abp.io/docs/latest/tutorials/book-store?UI=NG&DB=EF 

Incluir um Exemplo de CRUD de Clientes 

Campos: Código, Nome, DataNascimento 

Lista de Clientes com pesquisa pelo Nome e botões de ação 

Inclusão/Alteração de clientes 

Validations: https://abp.io/docs/latest/framework/fundamentals/validation  

Validações Frontend (DTO): 

Nome não pode ser vazio 

Data não pode ser maior do que hoje 

Validações Backend 

Nome não pode ser vazio 

Nome precisa ter no mínimo 3 letras 

DataNascimento precisa ser maior de 18 anos