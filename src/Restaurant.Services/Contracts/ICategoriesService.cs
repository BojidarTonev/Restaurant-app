using Restaurant.Data.Models;
using System.Linq;

namespace Restaurant.Services.Contracts
{
    public interface ICategoriesService
    {
        Category GetCategory(string id);

        IQueryable<Category> GetAllCategories();
    }
}
