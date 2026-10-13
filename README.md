# eShop

A learning-oriented .NET microservices project inspired by [dotnet/eShop](https://github.com/dotnet/eShop), built to explore and experiment with **MassTransit, RabbitMQ, asynchronous messaging, and service-to-service communication**.

## Tech Stack

- **.NET / ASP.NET Core**
- **MassTransit & RabbitMQ** — asynchronous messaging
- **Redis** — basket storage
- **gRPC** — service communication
- **OpenID Connect** — authentication
- **Docker** — containerization

## Services

- **Identity.API** — Authentication
- **Catalog.API** — Product catalog
- **Basket.API** — Shopping basket
- **Ordering.API** — Order processing
- **WebApp** — Web interface

```mermaid
flowchart TD
    User([User]) --> WebApp[WebApp]
    WebApp --> Identity[Identity.API]
    WebApp --> Catalog[Catalog.API]
    WebApp --> Basket[Basket.API]
    WebApp --> Ordering[Ordering.API]

    Basket --> Redis[(Redis)]

    WebApp -. "Messages via MassTransit" .-> RabbitMQ[(RabbitMQ)]
    Basket -. "Publish / Consume" .-> RabbitMQ
    Catalog -. "Publish / Consume" .-> RabbitMQ
    Ordering -. "Publish / Consume" .-> RabbitMQ

    classDef app fill:#dbeafe,stroke:#2563eb,color:#1e3a8a
    classDef infra fill:#dcfce7,stroke:#16a34a,color:#14532d
    class WebApp,Identity,Catalog,Basket,Ordering app
    class RabbitMQ,Redis infra
```