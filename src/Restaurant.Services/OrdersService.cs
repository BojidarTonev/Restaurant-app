using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using Restaurant.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> _ordersRepository;
        private readonly IRepository<Table> _tablesRepository;
        public OrdersService(IRepository<Order> ordersRepository, IRepository<Table> tablesRepository)
        {
            this._ordersRepository = ordersRepository;
            this._tablesRepository = tablesRepository;
        }

        public async Task<bool> AddOrder(string tableId, DateTime orderTime, int quantity, string productId, decimal totalPrice)
        {
            if(tableId == null || productId == null) { return false; }

            var userId = this._tablesRepository.All().First(t => t.Id == tableId).UserId;

            var order = new Order()
            {
                ProductId = productId,
                Quantity = quantity,
                TableId = tableId,
                totalPrice = totalPrice,
                OrderedOn = orderTime,
                UserId = userId
            };

            await this._ordersRepository.AddAsync(order);
            await this._ordersRepository.SaveChangesAsync();

            return true;
        }

        public IQueryable<Order> All() => this._ordersRepository.All();

        public IQueryable<Order> AllOrdersForTableById(string tableId) => this._ordersRepository.All().Where(o => o.TableId == tableId);

        public IQueryable<Order> AllOrdersForUserById(string userId) => this._ordersRepository.All().Where(o => o.UserId == userId).OrderByDescending(o => o.OrderedOn);

    }
}
