FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

ARG SSL_CERT_PASSWORD=1265c85b-d13d-4c84-8252-315987c2ab7b

COPY common.props NuGet.Config ./
COPY src/BookStore.Domain.Shared/*.csproj src/BookStore.Domain.Shared/
COPY src/BookStore.Domain/*.csproj src/BookStore.Domain/
COPY src/BookStore.Application.Contracts/*.csproj src/BookStore.Application.Contracts/
COPY src/BookStore.Application/*.csproj src/BookStore.Application/
COPY src/BookStore.EntityFrameworkCore/*.csproj src/BookStore.EntityFrameworkCore/
COPY src/BookStore.HttpApi/*.csproj src/BookStore.HttpApi/
COPY src/BookStore.HttpApi.Host/*.csproj src/BookStore.HttpApi.Host/
RUN dotnet restore src/BookStore.HttpApi.Host/BookStore.HttpApi.Host.csproj

COPY . .

RUN mkdir -p /app \
    && dotnet dev-certs https -v -ep /app/openiddict.pfx -p "$SSL_CERT_PASSWORD" \
    && chmod 644 /app/openiddict.pfx

RUN dotnet publish src/BookStore.HttpApi.Host/BookStore.HttpApi.Host.csproj \
    -c Release \
    --no-restore \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
COPY --from=build /app/openiddict.pfx .
ENTRYPOINT ["dotnet", "BookStore.HttpApi.Host.dll"]