using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using System.Linq;

namespace Restaurant.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoriesRepository;
        public CategoriesService(IRepository<Category> categoriesRepository)
        {
            this._categoriesRepository = categoriesRepository;
        }

        public IQueryable<Category> GetAllCategories()
        {
            return this._categoriesRepository.All();
        }

        public Category GetCategory(string id)
        {
            return this._categoriesRepository.All().FirstOrDefault(c => c.Id == id); 
        }
    }
}
