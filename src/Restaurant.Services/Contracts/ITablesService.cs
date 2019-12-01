using Restaurant.Data.Models;
using System.Linq;

namespace Restaurant.Services.Contracts
{
    public interface ITablesService
    {
        IQueryable<Table> AllTables();
        IQueryable<Table> AllAvailableTables();
        Table GetTableById(string id);
        IQueryable<Table> AllTablesForUserById(string id);
    }
}
