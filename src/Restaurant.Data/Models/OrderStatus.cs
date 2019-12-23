using Restaurant.Data.Models.Contracts;

namespace Restaurant.Data.Models
{
    public class OrderStatus : BaseModel<string>
    {
        public Enums.OrderStatus Status { get; set; }
    }
}
