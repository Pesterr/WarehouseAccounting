using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseAccounting.Model
{
    internal class ShipmentItems
    {
        public int id { get; set;}
        public int shipment { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public decimal price_per_unit { get; set; }
    }
}
