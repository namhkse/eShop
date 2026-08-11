using Catalog.API.Database;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.API.Modules.Catalogs;

public static class GetCatalogs
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/catalogs/items", ItemsAsync).WithTags("Catalogs");
    }

    private static async Task<List<CatalogItem>> GetItemsByIdsAsync(
        string ids,
        CatalogContext _catalogContext,
        CatalogSettings _settings)
    {
        var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

        if (!numIds.All(nid => nid.Ok))
        {
            return new List<CatalogItem>();
        }

        var idsToSelect = numIds
            .Select(id => id.Value);

        var items = await _catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

        items = ChangeUriPlaceholder(items, _settings);

        return items;
    }

    private static List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items, CatalogSettings _settings)
    {
        var baseUri = _settings.PicBaseUrl;
        var azureStorageEnabled = _settings.AzureStorageEnabled;

        foreach (var item in items)
        {
            item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
        }

        return items;
    }

    // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
    public static async Task<IResult> ItemsAsync(
        CatalogContext _catalogContext,
        IOptionsSnapshot<CatalogSettings> settings,
        int pageSize = 10,
        int pageIndex = 0,
        string ids = null)
    {
        var _settings = settings.Value;

        _catalogContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        if (!string.IsNullOrEmpty(ids))
        {
            var items = await GetItemsByIdsAsync(ids, _catalogContext, _settings);

            if (!items.Any())
            {
                return Results.BadRequest("ids value invalid. Must be comma-separated list of numbers");
            }

            return Results.Ok(items);
        }

        var totalItems = await _catalogContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _catalogContext.CatalogItems
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        /* The "awesome" fix for testing Devspaces */

        /*
        foreach (var pr in itemsOnPage) {
            pr.Name = "Awesome " + pr.Name;
        }

        */

        itemsOnPage = ChangeUriPlaceholder(itemsOnPage, _settings);

        var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

        return Results.Ok(model);
    }
}