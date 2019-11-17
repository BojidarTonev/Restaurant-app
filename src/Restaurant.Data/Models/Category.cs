using Restaurant.Data.Models.Contracts;
using Restaurant.Data.Models.Enums;

namespace Restaurant.Data.Models
{
    public class Category : BaseModel<string>
    {
        public Categories Name { get; set; }
    }
}
