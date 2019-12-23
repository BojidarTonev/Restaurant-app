using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Category> _categoriesRepository;

        public ProductsService(IRepository<Product> productsRepository, IRepository<Category> categoriesRepository)
        {
            this._productsRepository = productsRepository;
            this._categoriesRepository = categoriesRepository;
        }
        public IQueryable<Product> All() => this._productsRepository.All();

        public async Task<bool> CreateProduct(string productName, string productImageUrl, decimal productPrice, string productDescription, string categoryId)
        {
            if (productName == null || productImageUrl == null || productDescription == null || categoryId == null) { return false; }

            var category = this._categoriesRepository.All().First(c => c.Id == categoryId);

            var product = new Product()
            {
                 Name = productName,
                 ImageUrl = productImageUrl,
                 Price = productPrice,
                 Description = productDescription,
                 Category = category
            };

            await this._productsRepository.AddAsync(product);
            await this._productsRepository.SaveChangesAsync();

            return true;
        }

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
