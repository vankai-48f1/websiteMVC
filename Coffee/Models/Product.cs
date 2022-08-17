using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Product
    {
        public Product()
        {
            DetailBills = new HashSet<DetailBill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? UnitId { get; set; }
        public int? MediaId { get; set; }
        public int? ProductCategoryId { get; set; }

        public virtual Medium Media { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<DetailBill> DetailBills { get; set; }
    }
}
