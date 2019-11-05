using Restaurant.Data.Models.Contracts;
using Restaurant.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Data.Models
{
    public class Product : BaseModel<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Categories Category { get; set; }
    }
}
