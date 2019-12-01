using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using Restourant.Web.Areas.Tables.Models;

namespace Restourant.Web.Areas.Tables.Controllers
{
    [Area("Tables")]
    public class TablesController : Controller
    {
        private readonly ITablesService _tablesService;
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly UserManager<RestaurantUser> _userManager;
        public TablesController(ITablesService tablesService, UserManager<RestaurantUser> userManager, IOrdersService ordersService, IProductsService productsService)
        {
            this._tablesService = tablesService;
            this._userManager = userManager;
            this._ordersService = ordersService;
            this._productsService = productsService;
        }
        public IActionResult All()
        {
            var waiterName = this.User.Identity.Name;
            var userId = this._userManager.GetUserId(HttpContext.User);

            var tables = this._tablesService.AllTables()
                .Select(t => new TableViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    WaiterName = waiterName,
                    Status = t.Status,
                    UserId = userId
                }).ToList();

            var dto = new TablesAllViewModel() { Tables = tables };

            return View(dto);
        }

        public IActionResult AllTablesForUser()
        {
            var userId = this._userManager.GetUserId(HttpContext.User);

            var tables = this._tablesService.AllTablesForUserById(userId)
                .Select(t => new TableViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    WaiterName = this.User.Identity.Name,
                    Status = t.Status
                }).ToList();

            var dto = new TablesAllViewModel() { Tables = tables };

            return View(dto);
        }

        public IActionResult Details(string tableId)
        {
            var table = this._tablesService.GetTableById(tableId);
            var tableOrders = this._ordersService.AllOrdersForTableById(tableId);
            var orders = new List<string>();

            var userName = this._userManager.GetUserName(HttpContext.User);

            foreach (var order in tableOrders)
            {
                var product = this._productsService.GetProductById(order.ProductId);
                var str = $"{product.Name} x {order.Quantity} = ${order.totalPrice}";
                orders.Add(str);
            }

            var tableDto = new TableDetailsDto()
            {
                 Name = table.Name,
                 waiterName = userName,
                 Orders = orders
            };

            return View(tableDto);
        }
    }
}