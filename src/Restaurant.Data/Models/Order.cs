using Restaurant.Data.Models.Contracts;
using System;

namespace Restaurant.Data.Models
{
    public class Order : BaseModel<string>
    {
        public string UserId { get; set; }
        public virtual RestaurantUser User { get; set; }

        public DateTime OrderedOn { get; set; }

        public int Quantity { get; set; }

        public string TableId { get; set; }
        public virtual Table Table { get; set; }

        public decimal totalPrice { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual OrderStatus Status { get; set; }

    }
}
