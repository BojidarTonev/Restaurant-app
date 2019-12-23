using Restaurant.Data.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Restaurant.Data.Models
{
    public class Table : BaseModel<string>
    {
        private readonly string OFF_STATUS = "Off";

        public Table()
        {
            this.Orders = new List<Order>();
            this.Status = this.OFF_STATUS;
        }
        public string Name { get; set; }

        public string UserId { get; set; }
        public virtual RestaurantUser User { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
