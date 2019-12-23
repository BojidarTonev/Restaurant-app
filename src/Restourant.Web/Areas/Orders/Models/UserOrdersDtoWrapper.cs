using System.Collections.Generic;

namespace Restourant.Web.Areas.Orders.Models
{
    public class UserOrdersDtoWrapper
    {
        public List<UsersOrderDto> userOrders { get; set; }

        public string userName { get; set; }
    }
}
