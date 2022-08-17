using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class DetailBill
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public decimal? SubTotal { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? ProductId { get; set; }
        public int? BillId { get; set; }

        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
