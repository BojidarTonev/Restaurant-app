using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Services.Contracts;
using Restourant.Web.Areas.Products.Models;

namespace Restourant.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ITablesService _tablesService;
        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, ITablesService tablesService)
        {
            this._productsService = productsService;
            this._categoriesService = categoriesService;
            this._tablesService = tablesService;
        }
        public IActionResult All()  
        {
            var products = this._productsService.All()
                .Select(p => new ProductsAllViewModel()
                {
                    Description = p.Description,
                    Name = p.Name,
                    Id = p.Id,
                    Price = p.Price.ToString(),
                    ImageUrl = p.ImageUrl
                }).ToList();

            var dto = new ProductsAllViewModelWrapper()
            {
                Products = products,
                DisplayCategory = "All",
                DisplayDefaultTable = "Select your table"
            };

            var productsCategories = this._categoriesService.GetAllCategories()
                .Select(p => new SelectListItem()
                {
                    Value = p.Id,
                    Text = p.Name.ToString()
                });

            var availableTables = this._tablesService.AllTables()
                .Select(t => new SelectListItem() { Value = t.Id, Text = t.Name });

            this.ViewData["ProductCategories"] = productsCategories;
            this.ViewData["AvailableTables"] = availableTables;
            return View(dto);
        }

        [HttpPost]
        public IActionResult All(ProductsAllViewModelWrapper model)
        {
            var categoryId = model.DisplayCategory;
            var category = this._categoriesService.GetCategory(categoryId);

            var products = this._productsService.GetAllProductsByCategory(category.Name.ToString())
               .Select(p => new ProductsAllViewModel()
               {
                   Description = p.Description,
                   Name = p.Name,
                   Id = p.Id,
                   Price = p.Price.ToString(),
                   ImageUrl = p.ImageUrl
               }).ToList();

            var dto = new ProductsAllViewModelWrapper()
            {
                Products = products,
                DisplayCategory = category.Name.ToString()
            };

            var categories = this._categoriesService.GetAllCategories()
                .Select(p => new SelectListItem()
                {
                    Value = p.Id,
                    Text = p.Name.ToString()
                });

            this.ViewData["ProductCategories"] = categories;
            return View(dto);

        }

        [Authorize(Roles="Admin")]
        public IActionResult CreateProduct()
        {




            return View();
        }
    }
}