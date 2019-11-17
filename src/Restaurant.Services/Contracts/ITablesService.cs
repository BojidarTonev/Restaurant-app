using Restaurant.Data.Models;
using System.Linq;

namespace Restaurant.Services.Contracts
{
    public interface ITablesService
    {
        IQueryable<Table> AllTables();

        IQueryable<Table> AllTablesForUserById(string id);
    }
}
