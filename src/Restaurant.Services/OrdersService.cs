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
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<OrderStatus> _orderStatusRepository;
        private readonly IRepository<Product> _productsRepository;
        public OrdersService(IRepository<Order> ordersRepository, IRepository<Table> tablesRepository, IRepository<Category> categoriesRepostory, IRepository<OrderStatus> orderStatusRepository, IRepository<Product> productsRepository)
        {
            this._ordersRepository = ordersRepository;
            this._tablesRepository = tablesRepository;
            this._categoriesRepository = categoriesRepostory;
            this._orderStatusRepository = orderStatusRepository;
            this._productsRepository = productsRepository;
        }

        public async Task<bool> AddOrder(string tableId, DateTime orderTime, int quantity, string productId, decimal totalPrice)
        {
            if (tableId == null || productId == null) { return false; }

            var userId = this._tablesRepository.All().First(t => t.Id == tableId).UserId;
            var product = this._productsRepository.All().First(p => p.Id == productId);
            var orderStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Ready);
            if(product.Category == this._categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Kitchen) || product.Category == this._categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Dessert))
            {
                orderStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Preapring);
            } else if (product.Category == this._categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.BarSlow))
            {
                orderStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Preapring);
            }

            var order = new Order()
            {
                ProductId = productId,
                Quantity = quantity,
                TableId = tableId,
                totalPrice = totalPrice,
                OrderedOn = orderTime,
                UserId = userId,
                Status = orderStatus
            };

            await this._ordersRepository.AddAsync(order);
            await this._ordersRepository.SaveChangesAsync();

            return true;
        }

        public IQueryable<Order> All() => this._ordersRepository.All();

        public IQueryable<Order> AllOrdersForTableById(string tableId) => this._ordersRepository.All().Where(o => o.TableId == tableId);

        public IQueryable<Order> AllOrdersForUserById(string userId) => this._ordersRepository.All().Where(o => o.UserId == userId).OrderByDescending(o => o.OrderedOn);

        public IQueryable<Order> AllKitchenOrders()
        {
            var preapringStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Preapring);

            var kitchenCategory = _categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Kitchen);
            var dessertCategory = _categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Dessert);
            var orders = this._ordersRepository.All().Where(o => o.Product.Category == kitchenCategory || o.Product.Category == dessertCategory);

            var ordersForReturn = orders.Where(o => o.Status == preapringStatus).OrderByDescending(o => o.OrderedOn);

            return ordersForReturn;
        }

        public IQueryable<Order> AllBarOrders()
        {
            var preapringStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Preapring);

            var barSlowCatregory = _categoriesRepository.All().First(c => c.Name == Restaurant.Data.Models.Enums.Categories.BarSlow);
            var orders = this._ordersRepository.All().Where(o => o.Product.Category == barSlowCatregory && o.Status == preapringStatus).OrderByDescending(o => o.OrderedOn);

            return orders;
        }

        public Order GetOrderById(string orderId)
        {
            var order = this._ordersRepository.All().First(o => o.Id == orderId);

            return order;
        }

        public void ChangeOrderStatus(Order order)
        {
            var readyStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Ready);
            var preapringStatus = this._orderStatusRepository.All().First(os => os.Status == Restaurant.Data.Models.Enums.OrderStatus.Preapring);

            order.Status = readyStatus;

            this._ordersRepository.Update(order);
        }
    }
}
