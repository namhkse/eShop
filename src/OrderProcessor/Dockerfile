FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Packages.props ./
COPY ["src/OrderProcessor/OrderProcessor.csproj", "src/OrderProcessor/"]
COPY ["src/OrderProcessor.Contracts/OrderProcessor.Contracts.csproj", "src/OrderProcessor.Contracts/"]
RUN dotnet restore "src/OrderProcessor/OrderProcessor.csproj"
COPY . .
WORKDIR "/src/src/OrderProcessor"
RUN dotnet build "./OrderProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessor.dll"]
