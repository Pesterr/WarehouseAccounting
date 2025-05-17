using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseAccounting.Model
{
    internal class Orders
    {
        public int order_id { get; set; }
        public string client_name { get; set; }
        public string employee { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }

    }
}
