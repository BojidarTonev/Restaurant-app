using Restaurant.Data.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Restaurant.Data.Models
{
    public class Order : BaseModel<string>
    {
        public Order()
        {
            this.Products = new List<Product>();
        }

        public string UserId { get; set; }
        public RestaurantUser User { get; set; }

        public DateTime OrderedOn { get; set; }

        public int Quantity { get; set; }

        public string TableId { get; set; }
        public Table Table { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
