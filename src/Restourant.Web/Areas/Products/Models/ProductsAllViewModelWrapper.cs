using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restourant.Web.Areas.Products.Models
{
    public class ProductsAllViewModelWrapper
    {
        public IEnumerable<ProductsAllViewModel> Products { get; set; }

        public string DisplayCategory { get; set; }
    }
}
