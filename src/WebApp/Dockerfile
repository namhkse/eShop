FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Packages.props ./
COPY ["src/WebApp/WebApp.csproj", "src/WebApp/"]
COPY ["src/WebAppComponents/WebAppComponents.csproj", "src/WebAppComponents/"]
COPY ["src/Shop.ServiceDefaults/Shop.ServiceDefaults.csproj", "src/Shop.ServiceDefaults/"]
COPY ["src/Ordering.Contracts/Ordering.Contracts.csproj", "src/Ordering.Contracts/"]
COPY ["src/Ordering.Domain/Ordering.Domain.csproj", "src/Ordering.Domain/"]
RUN dotnet restore "src/WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/src/WebApp"
RUN dotnet build "./WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]
