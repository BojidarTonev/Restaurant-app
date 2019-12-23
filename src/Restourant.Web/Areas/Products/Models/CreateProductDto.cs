using System.ComponentModel.DataAnnotations;

namespace Restourant.Web.Areas.Products.Models
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }  

        [Required(ErrorMessage = "Image is required")]
        public string ProductImageUrl { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "Can't add negative price, sry")]
        public string ProductPrice { get; set; }

        public string ProductCategory { get; set; }
    }
}
