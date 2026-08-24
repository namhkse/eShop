using System.Collections.Generic;
using System.Threading.Tasks;
using eShop.WebAppComponents.Catalog;

namespace Shop.WebApp.Services
{
    public interface IBasketState
    {
        public Task<IReadOnlyCollection<BasketItem>> GetBasketItemsAsync();

        public Task AddAsync(CatalogItem item);
    }
}
