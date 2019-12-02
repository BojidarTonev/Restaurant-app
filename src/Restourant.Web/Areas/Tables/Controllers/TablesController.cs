using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
        public TablesController(ITablesService tablesService,
            UserManager<RestaurantUser> userManager,
            IOrdersService ordersService,
            IProductsService productsService)
        {
            this._tablesService = tablesService;
            this._userManager = userManager;
            this._ordersService = ordersService;
            this._productsService = productsService;
        }

        [Authorize(Roles="Admin")]
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
            var orders = new List<TableDetailsOrdersDto>();
            var totalProfit = 0m;

            var user = this._userManager.FindByIdAsync(table.UserId).Result.FirstName + " " + this._userManager.FindByIdAsync(table.UserId).Result.LastName;

            foreach (var order in tableOrders)
            {
                var product = this._productsService.GetProductById(order.ProductId);
                var orderDto = new TableDetailsOrdersDto()
                {
                    productName = product.Name,
                    quantity = order.Quantity.ToString(),
                    totalPrice = order.totalPrice.ToString(),
                    orderedOn = order.OrderedOn.ToLongTimeString()
                };
                totalProfit += order.totalPrice;
                orders.Add(orderDto);
            }

            var tableDto = new TableDetailsDto()
            {
                 Name = table.Name,
                 waiterName = user,
                 Orders = orders,
                 totalProfit = totalProfit.ToString()
            };

            return View(tableDto);
        }
    }
}