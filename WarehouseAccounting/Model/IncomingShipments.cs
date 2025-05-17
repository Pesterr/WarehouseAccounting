using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseAccounting.Model
{
    internal class IncomingShipments
    {
        public int id {  get; set; }
        public string supplier { get; set; }
        public string category { get; set; }
        public DateTime shipment_date { get; set; }
    }
}
