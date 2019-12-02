using System.Collections.Generic;

namespace Restourant.Web.Areas.Tables.Models
{
    public class TableDetailsDto
    {
        public string Name { get; set; }
        public ICollection<TableDetailsOrdersDto> Orders { get; set; }
        public string waiterName { get; set; }

        public string totalProfit { get; set; }
    }
}
