using Restaurant.Data.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Restaurant.Data.Models
{
    public class Table : BaseModel<string>
    {
        public Table()
        {
            this.Orders = new List<Order>();
        }
        public string Name { get; set; }

        public string UserId { get; set; }
        public RestaurantUser User { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
