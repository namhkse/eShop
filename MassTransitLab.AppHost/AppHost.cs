var builder = DistributedApplication.CreateBuilder(args);

// var postgres = builder.AddPostgres("postgres")
//     .WithImage("ankane/pgvector")
//     .WithImageTag("latest")
//     .WithLifetime(ContainerLifetime.Persistent);
//
// var redis = builder.AddRedis("redis");
// var rabbitmq = builder.AddRabbitMQ("rabbitmq");
//
// var catalogDb = postgres.AddDatabase("catalogdb");

builder.AddProject<Projects.Basket_API>("basket-api");
builder.AddProject<Projects.Catalog_API>("catalog-api");
builder.AddProject<Projects.Ordering_API>("ordering-api");
builder.AddProject<Projects.Identity_API>("identity-api");
builder.AddProject<Projects.OrderProcessor>("order-processor");
builder.AddProject<Projects.PaymentProcessor>("payment-processor");
builder.AddProject<Projects.WebApp>("web-app");

builder.Build().Run();
