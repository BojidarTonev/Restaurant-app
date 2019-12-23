using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Restaurant.Data.Models
{
    public class RestaurantUser : IdentityUser
    {
        public RestaurantUser()
        {
            this.TablesServed = new List<Table>();
            this.TakenOrders = new List<Order>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Table> TablesServed { get; set; }

        public virtual ICollection<Order> TakenOrders { get; set; }
    }
}
