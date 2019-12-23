using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using Restourant.Web.Areas.Orders.Models;

namespace Restourant.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ITablesService _tablesService;
        private readonly IProductsService _productsService;
        private readonly UserManager<RestaurantUser> _userManager;
        public OrdersController(IOrdersService ordersService, 
            ITablesService tablesService, 
            IProductsService productsService, 
            UserManager<RestaurantUser> userManager)
        {
            this._ordersService = ordersService;
            this._tablesService = tablesService;
            this._productsService = productsService;
            this._userManager = userManager;
        }
        public IActionResult CreateOrder(string id)
        {
            var product = this._productsService.GetProductById(id);
            var availableTables = this._tablesService.AllTables()
                .Select(t => new SelectListItem() { Value = t.Id, Text = t.Name });

            this.ViewData["AvailableTables"] = availableTables;

            var dto = new CreateOrderDto()
            {
                productName = product.Name,
                productDesription = product.Description,
                productId = product.Id,
                productImageUrl = product.ImageUrl,
                DisplayTable = "Select table"
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto dto)
        {
            if (!TryValidateModel(dto))
            {
                return this.View(dto);
            };

            var quantity = int.Parse(dto.Quantity);
            var productId = dto.productId;
            var selectedTableId = dto.DisplayTable;

            var product = this._productsService.GetProductById(productId);

            var totalPrice = quantity * product.Price;

            var orderSuccessful = this._ordersService.AddOrder(selectedTableId, DateTime.Now, quantity, product.Id, totalPrice).Result;

            if (!orderSuccessful)
            {
                return View("OrderNotCompleted");
            }

            return Redirect("/");
        }

        [Authorize]
        public IActionResult OrdersForUser()
        {
            var userId = this._userManager.GetUserId(HttpContext.User);
            var userName = this._userManager.GetUserName(HttpContext.User);

            var ordersForUser = this._ordersService.AllOrdersForUserById(userId)
                .Select(o => new UsersOrderDto()
                {
                    orderedOn = o.OrderedOn.ToLongTimeString(),
                    ProductName = o.Product.Name,
                    quantity = o.Quantity.ToString(),
                    TableName = o.Table.Name,
                    totalPrice = o.totalPrice.ToString(),
                    Status = o.Status.Status.ToString()
                }).ToList();

            var dto = new UserOrdersDtoWrapper()
            {
                userOrders = ordersForUser,
                waiterName = userName
            };

            return View(dto);
        }

        [Authorize(Roles = "Chef")]
        public IActionResult OrdersForKitchen()
        {
            var userId = this._userManager.GetUserId(HttpContext.User);
            var userName = this._userManager.GetUserName(HttpContext.User);

            var ordersForUser = this._ordersService.AllKitchenOrders()
             .Select(o => new UsersOrderDto()
             {
                 orderedOn = o.OrderedOn.ToLongTimeString(),
                 ProductName = o.Product.Name,
                 quantity = o.Quantity.ToString(),
                 ProductId = o.ProductId
             }).ToList();

            var dto = new UserOrdersDtoWrapper()
            {
                userOrders = ordersForUser,
                waiterName = userName
            };

            return View(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Chef")]
        public IActionResult CreatedKitchenOrder(string productId)
        {
            var order = this._ordersService.All().First(o => o.ProductId == productId);
            
            
            return Redirect("/");
        }

        [Authorize(Roles = "Barman")]
        public IActionResult OrdersForBar()
        {
            var userId = this._userManager.GetUserId(HttpContext.User);
            var userName = this._userManager.GetUserName(HttpContext.User);

            var ordersForUser = this._ordersService.AllBarOrders()
             .Select(o => new UsersOrderDto()
             {
                 orderedOn = o.OrderedOn.ToLongTimeString(),
                 ProductName = o.Product.Name,
                 quantity = o.Quantity.ToString(),
                 ProductId = o.ProductId
             }).ToList();

            var dto = new UserOrdersDtoWrapper()
            {
                userOrders = ordersForUser,
                waiterName = userName
            };

            return View(dto);
        }
    }
}