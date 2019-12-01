using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> _productsRepository;

        public ProductsService(IRepository<Product> productsRepository)
        {
            this._productsRepository = productsRepository;
        }
        public IQueryable<Product> All() => this._productsRepository.All();

        public IQueryable<Product> GetAllProductsByCategory(string category) => this._productsRepository.All().Where(p => p.Category.Name.ToString() == category);

        public decimal GetPriceByName(string productName) => this._productsRepository.All().First(p => p.Name == productName).Price;

        public Product GetProductById(string id) => this._productsRepository.All().First(p => p.Id == id);

        public Product ProductDetails<TViewModel>(string productId)
        {
            var product = this._productsRepository.All()
                .Where(p => p.Id == productId)
                .FirstOrDefault();

            return product;
        }
    }
}
