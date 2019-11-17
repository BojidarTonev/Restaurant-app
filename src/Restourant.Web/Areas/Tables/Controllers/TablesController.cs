using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly UserManager<RestaurantUser> _userManager;
        public TablesController(ITablesService tablesService, UserManager<RestaurantUser> userManager)
        {
            this._tablesService = tablesService;
            this._userManager = userManager;
        }
        public IActionResult All()
        {
            var waiterName = this.User.Identity.Name;
            var userId = this._userManager.GetUserId(HttpContext.User);

            var tables = this._tablesService.AllTables()
                .Select(t => new TableViewModel()
                {
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
                    Name = t.Name,
                    WaiterName = this.User.Identity.Name,
                    Status = t.Status
                }).ToList();

            var dto = new TablesAllViewModel() { Tables = tables };

            return View(dto);
        }
    }
}