using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseAccounting.Model
{
    public class Employees
    {
        public int id {  get; set; }
        public string full_name { get; set; }
        public string position { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
