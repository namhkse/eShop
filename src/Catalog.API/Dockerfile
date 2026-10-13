FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Packages.props ./
COPY ["src/Catalog.API/Catalog.API.csproj", "src/Catalog.API/"]
COPY ["src/Ordering.Domain/Ordering.Domain.csproj", "src/Ordering.Domain/"]
COPY ["src/Shop.ServiceDefaults/Shop.ServiceDefaults.csproj", "src/Shop.ServiceDefaults/"]
COPY ["src/Ordering.Contracts/Ordering.Contracts.csproj", "src/Ordering.Contracts/"]
COPY ["src/Basket.Contracts/Basket.Contracts.csproj", "src/Basket.Contracts/"]
RUN dotnet restore "src/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/src/Catalog.API"
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/Catalog.API/Pics/ ./Pics/
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
