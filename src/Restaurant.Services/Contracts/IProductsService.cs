using Restaurant.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Services.Contracts
{
    public interface IProductsService
    {
        IQueryable<Product> All();

        Product ProductDetails<TViewModel>(string productId);

        IQueryable<Product> GetAllProductsByCategory(string category);
    }
}
