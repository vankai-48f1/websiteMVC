using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Bill
    {
        public Bill()
        {
            DetailBills = new HashSet<DetailBill>();
            DetailsPayments = new HashSet<DetailsPayment>();
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool? Status { get; set; }
        public decimal? Total { get; set; }
        public int? SessionId { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Session Session { get; set; }
        public virtual ICollection<DetailBill> DetailBills { get; set; }
        public virtual ICollection<DetailsPayment> DetailsPayments { get; set; }
    }
}
