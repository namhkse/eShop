using Catalog.API.Infrastructure;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.API.Model;

public class CatalogServices(
    CatalogContext context,
    [FromServices] ICatalogAI catalogAI,
    IOptions<CatalogOptions> options,
    ILogger<CatalogServices> logger)
{
    public CatalogContext Context { get; } = context;
    public ICatalogAI CatalogAI { get; } = catalogAI;
    public IOptions<CatalogOptions> Options { get; } = options;
    public ILogger<CatalogServices> Logger { get; } = logger;
};