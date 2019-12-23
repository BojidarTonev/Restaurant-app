using System.Linq;
using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;

namespace Restaurant.Services
{
    public class TablesService : ITablesService
    {
        private readonly IRepository<Table> _tablesRepository;
        public TablesService(IRepository<Table> tablesRepository) 
        {
            this._tablesRepository = tablesRepository;
        }

        public IQueryable<Table> AllTables()
        {
            return this._tablesRepository.All().OrderBy(t => t.Name);
        }
        public IQueryable<Table> AllAvailableTables()
        {
            return this._tablesRepository.All().Where(t => t.Status != "Off");
        }
        public IQueryable<Table> AllTablesForUserById(string id)
        {
            return this._tablesRepository.All().Where(t => t.UserId == id);

        }

        public Table GetTableById(string id)
        {
            return this._tablesRepository.All().First(t => t.Id == id);
        }
    }
}
