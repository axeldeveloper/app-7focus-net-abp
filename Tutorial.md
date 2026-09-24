# 📘 Tutorial BookStore (ABP Framework + Angular + EF Core + SQL Server)

> Guia passo a passo para preparar o ambiente, criar a solução ABP, rodar migrations e implementar o **Desafio de Clientes**.

---

## 🛠️ 1. Preparando o Terreno

### 📋 Pré-requisitos

| Ferramenta | Versão / Observação |
|---|---|
| macOS | Monterrey ou superior |
| Homebrew | Gerenciador de pacotes padrão |
| Docker | Para rodar o SQL Server |
| .NET SDK | Instalado via Homebrew |
| ABP CLI | Via .NET tool |

---

### 🍎 Instalando o .NET no macOS (Monterrey)

```bash
# .NET SDK
brew install --cask dotnet-sdk

# Verifica a versão instalada
dotnet --version

# Ferramentas ABP
dotnet tool install -g Volo.Abp.Studio.Cli

# Entity Framework Core Tools
dotnet tool install --global dotnet-ef

# Exporta o PATH para incluir as ferramentas .NET
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc

# Gera certificado HTTPS para o OpenAPI/IdentityServer
dotnet dev-certs https -v -ep openiddict.pfx -p 1265c85b-d13d-4c84-8252-315987c2ab7b
```

---

## 📁 2. Criando uma Solução ABP

1. Crie uma pasta vazia e abra um terminal.
2. Execute o comando abaixo:

```bash
# Cria o projeto ABP com template Angular
abp new BookStore -u angular

# Instala as dependências do cliente
abp install-libs
```

---

### 📦 Configurando o SQL Server

| Passo | Comando / Ação |
|---|---|
| Adicionar o provider SQL Server | `dotnet add src/BookStore.EntityFrameworkCore package Volo.Abp.EntityFrameworkCore.SqlServer` |
| Listar packages instalados | `dotnet list src/BookStore.EntityFrameworkCore package` |
| Configurar o provider | Edite o arquivo e adicione `options.UseSqlServer()` |

---

## 🐳 3. Instalando o SQL Server via Docker

### Opção 1 — Docker Compose

```bash
$ docker compose up sqlserver -d
```

### Opção 2 — Docker Run

| Flag | Descrição |
|---|---|
| `--name sqlserver` | Nome do container |
| `-e ACCEPT_EULA=Y` | Aceita os termos de uso |
| `-e MSSQL_SA_PASSWORD` | Senha do sa (mínimo 8 chars, letra + número + símbolo) |
| `-p 1433:1433` | Mapeia a porta 1433 |
| `-d` | Executa em background |

```bash

docker run \
  --name sqlserver \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='pf766312!' \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## 🔧 4. Migrations & Seed com EF Core

> Conforme a documentação, execute pelo **DbMigrator** — tanto em dev quanto em produção.

| Ação | Comando |
|---|---|
| Criar migration inicial | `dotnet ef migrations add Initial --project src/BookStore.EntityFrameworkCore --startup-project src/BookStore.DbMigrator` |
| Executar migrations + seed | `dotnet run --project src/BookStore.DbMigrator` |
| Navegar e rodar diretamente | `cd src/BooStore.DbMigrator` → `dotnet run` |
| Adicionar nova migration | `dotnet ef migrations add Created_Customer_Entity --project src/BookStore.EntityFrameworkCore --startup-project src/BookStore.DbMigrator` |

---

## ▶️ 5. Executando o Backend e front Localmente

```bash
# Back-End
$ cd src/BooStore.HttpApi.Host
$ dotnet run

# Front-End
$ cd angular
$ npm run start

# ou simplesmente 

$  ./run-local.sh 
```

---

## 🐳 6. Docker

> Gere o certificado HTTPS dentro do projeto do backend antes de subir os containers:

```bash
dotnet dev-certs https -v -ep src/BookStore.HttpApi.Host/openiddict.pfx -p 1265c85b-d13d-4c84-8252-315987c2ab7b
```

### Fluxo de desenvolvimento

| Passo | Comando |
|---|---|
| 1. Criar DTO & Service | Implementar as classes correspondentes |
| 2. Gerar proxy Angular | `abp generate-proxy -t ng -url https://localhost:44388/` |
| 3. Build & run | Rodar backend + Angular |

---

## 🎯 7. DESAFIO — CRUD de Clientes

> Gostamos de dar a oportunidade para que todos mostrem o seu trabalho na prática. Conhecer o ABP é essencial, então siga o tutorial oficial:

🔗 **Referências úteis**

| Descrição | Link |
|---|---|
| Site oficial ABP | https://abp.io/ |
| Tutorial ABP Book Store (NG + EF) | https://abp.io/docs/latest/tutorials/book-store?UI=NG&DB=EF |
| Documentação de Validações | https://abp.io/docs/latest/framework/fundamentals/validation |

---

### 📋 Requisitos do Desafio

| Item | Detalhes |
|---|---|
| **Entidade Cliente** | Código, Nome, DataNascimento |
| **Lista de Clientes** | Pesquisa pelo Nome + botões de ação |
| **Inclusão / Alteração** | Formulário para create / update |
| **Validações Frontend (DTO)** | Nome não pode ser vazio; Data não pode ser maior que hoje |
| **Validações Backend** | Nome não pode ser vazio; Nome ≥ 3 letras; DataNascimento ≥ 18 anos |

#### 🗂️ Campos da Entidade `Cliente`

| Campo | Tipo | Observação |
|---|---|---|
| Código | `Guid` / `int` | Identificador único |
| Nome | `string` | Validações detalhadas abaixo |
| DataNascimento | `DateTime` | Validação de idade mínima |

#### ✅ Validações

**Frontend (DTO)**

| Campo | Regra | Mensagem |
|---|---|---|
| Nome | Não pode ser vazio | "O nome é obrigatório" |
| DataNascimento | Não pode ser maior que hoje | "A data não pode ser futura" |

**Backend**

| Campo | Regra | Mensagem |
|---|---|---|
| Nome | Não pode ser vazio | "O nome é obrigatório" |
| Nome | Mínimo 3 letras | "O nome precisa ter pelo menos 3 letras" |
| DataNascimento | Idade ≥ 18 anos | "O cliente precisa ter pelo menos 18 anos" |
