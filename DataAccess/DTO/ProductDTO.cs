using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ProductDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductSalePercent { get; set; }
        public string ProductStatus { get; set; }
        public string ProductImage { get; set; }
        public string CategoryId { get; set; }
    }
}
