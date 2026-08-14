using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF;

public static class IntegrationLogExtensions
{
   public static void UseIntegrationEventLogs(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<IntegrationEventLogEntry>(builder =>
      {
         builder.ToTable("IntegrationEventLog");
         builder.HasKey(e => e.EventId);
      });
   }
}