using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public string OrderNote { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string AccountId { get; set; }
        public string BuyerFullName { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerAddress { get; set; }
        public string VoucherId { get; set; }
        public int? ProductSalePercent { get; set; }
        public string AccIdofChef { get; set; }
        public string AccIdofShipper { get; set; }
    }
}
