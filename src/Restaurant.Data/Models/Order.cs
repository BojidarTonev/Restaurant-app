using Restaurant.Data.Models.Contracts;
using System;

namespace Restaurant.Data.Models
{
    public class Order : BaseModel<string>
    {
        public string UserId { get; set; }
        public RestaurantUser User { get; set; }

        public DateTime OrderedOn { get; set; }

        public int Quantity { get; set; }

        public string TableId { get; set; }
        public Table Table { get; set; }

        public decimal totalPrice { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

    }
}
