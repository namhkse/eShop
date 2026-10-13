FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Packages.props ./
COPY ["src/Ordering.API/Ordering.API.csproj", "src/Ordering.API/"]
COPY ["src/Ordering.Domain/Ordering.Domain.csproj", "src/Ordering.Domain/"]
COPY ["src/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/Ordering.Infrastructure/"]
COPY ["src/Ordering.Contracts/Ordering.Contracts.csproj", "src/Ordering.Contracts/"]
COPY ["src/Shop.ServiceDefaults/Shop.ServiceDefaults.csproj", "src/Shop.ServiceDefaults/"]
COPY ["src/OrderProcessor.Contracts/OrderProcessor.Contracts.csproj", "src/OrderProcessor.Contracts/"]
COPY ["src/PaymentProcessor.Contracts/PaymentProcessor.Contracts.csproj", "src/PaymentProcessor.Contracts/"]
COPY ["src/Basket.Contracts/Basket.Contracts.csproj", "src/Basket.Contracts/"]
RUN dotnet restore "src/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Ordering.API"
RUN dotnet build "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
