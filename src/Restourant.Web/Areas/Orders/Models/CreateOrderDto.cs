using System.ComponentModel.DataAnnotations;

namespace Restourant.Web.Areas.Orders.Models
{
    public class CreateOrderDto
    {
        public string productId { get; set; }

        public string productName { get; set; }

        public string productDesription { get; set; }

        public string productImageUrl { get; set; }

        public string DisplayTable { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "You want just the shipping fee?")]
        public string Quantity { get; set; }
    }
}
