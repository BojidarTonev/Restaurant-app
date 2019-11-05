using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Restaurant.Data.Models.Enums;

namespace Restaurant.Data.Models
{
    public class RestaurantUser : IdentityUser
    {
        public RestaurantUser()
        {
            this.TablesServed = new List<Table>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public Roles Role { get; set; }

        public ICollection<Table> TablesServed { get; set; }
    }
}
