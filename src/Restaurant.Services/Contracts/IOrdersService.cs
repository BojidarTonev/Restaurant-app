using Restaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services.Contracts
{
    public interface IOrdersService
    {
        Task<bool> AddOrder(string tableId, DateTime timeOfOrder, int quantity, string productId, decimal totalPrice);
        IQueryable<Order> All();
        IQueryable<Order> AllOrdersForUserById(string userId);

        IQueryable<Order> AllOrdersForTableById(string tableId);

        IQueryable<Order> AllKitchenOrders();

        IQueryable<Order> AllBarOrders();
    }
}
