using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int CartQuantity { get; set; }
        public string ProductId { get; set; }
        public string AccountId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int ProductSalePercent { get; set; }
        public string ProductImage { get; set; }
    }
}
