using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL
{
    public class CustOrdersDetail
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public decimal ExtendedPrice { get; set; }
    }
}
