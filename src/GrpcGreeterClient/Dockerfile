FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GrpcGreeterClient/GrpcGreeterClient.csproj", "src/GrpcGreeterClient/"]
RUN dotnet restore "src/GrpcGreeterClient/GrpcGreeterClient.csproj"
COPY . .
WORKDIR "/src/src/GrpcGreeterClient"
RUN dotnet build "./GrpcGreeterClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrpcGreeterClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcGreeterClient.dll"]
