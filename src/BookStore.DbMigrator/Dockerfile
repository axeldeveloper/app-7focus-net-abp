FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY common.props NuGet.Config ./
COPY src/BookStore.Domain.Shared/*.csproj src/BookStore.Domain.Shared/
COPY src/BookStore.Domain/*.csproj src/BookStore.Domain/
COPY src/BookStore.Application.Contracts/*.csproj src/BookStore.Application.Contracts/
COPY src/BookStore.EntityFrameworkCore/*.csproj src/BookStore.EntityFrameworkCore/
COPY src/BookStore.DbMigrator/*.csproj src/BookStore.DbMigrator/
RUN dotnet restore src/BookStore.DbMigrator/BookStore.DbMigrator.csproj

COPY . .

RUN dotnet publish src/BookStore.DbMigrator/BookStore.DbMigrator.csproj \
    -c Release \
    --no-restore \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookStore.DbMigrator.dll"]