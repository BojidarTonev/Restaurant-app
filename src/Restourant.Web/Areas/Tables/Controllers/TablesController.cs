using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.Contracts;
using Restourant.Web.Areas.Tables.Models;

namespace Restourant.Web.Areas.Tables.Controllers
{
    [Area("Tables")]
    public class TablesController : Controller
    {
        private readonly ITablesService _tablesService;
        public TablesController(ITablesService tablesService)
        {
            this._tablesService = tablesService;
        }
        public IActionResult All()
        {
            var waiterName = this.User.Identity.Name;
            var tables = this._tablesService.AllTables()
                .Select(t => new TableViewModel()
                {
                    Name = t.Name,
                    WaiterName = waiterName,
                    Status = t.Status,
                    UserId = waiterName
                }).ToList();

            var dto = new TablesAllViewModel() { Tables = tables };

            return View(dto);
        }

        public IActionResult AllTablesForUser()
        {
            var userId = this.User.Identity.Name;

            var tables = this._tablesService.AllTablesForUserById(userId);

            return View(tables);
        }
    }
}