using Restaurant.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services.Contracts
{
    public interface IProductsService
    {
        IQueryable<Product> All();

        Product ProductDetails<TViewModel>(string productId);

        IQueryable<Product> GetAllProductsByCategory(string category);

        Product GetProductById(string id);

        decimal GetPriceByName(string productName);

        Task<bool> CreateProduct(string productName, string productImageUrl, decimal productPrice, string productDescription, string categoryId);
    }
}
