using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class OrderDetailsDTO
    {
        public string OrderDid { get; set; }
        public int OrderDquantity { get; set; }
        public int OrderDprice { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string OrderId { get; set; }
    }
}
