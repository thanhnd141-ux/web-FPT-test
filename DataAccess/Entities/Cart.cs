using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int CartQuantity { get; set; }
        public string ProductId { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
    }
}
