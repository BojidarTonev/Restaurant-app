using Restaurant.Data.Models.Contracts;

namespace Restaurant.Data.Models
{
    public class Product : BaseModel<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }
    }
}
