using Basket.API.Repositories;
using ServiceDefaults;

namespace Basket.API.Extensions;

public static class Extensions
{
   public static void AddApplicationServices(this IHostApplicationBuilder builder)
   {
      builder.AddDefaultAuthentication();
      
      builder.AddRedisClient("redis");
      
      builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();
      
      builder.AddRabbitMQClient("eventbus");   
   }
}