using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseAccounting.Model
{
    public class Products
    {
        public int product_id  { get; set; }
        public string product_name { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }

    }
}
