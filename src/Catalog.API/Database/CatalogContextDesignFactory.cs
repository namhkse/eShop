using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.API.Database;

public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
            .UseSqlServer("Server=tcp:127.0.0.1,1433;Database=Microsoft.eShopOnContainers.Services.CatalogDb;User Id=sa;Password=Pass@word;TrustServerCertificate=True");

        return new CatalogContext(optionsBuilder.Options);
    }
}