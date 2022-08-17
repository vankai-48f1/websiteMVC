using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class DetailsPayment
    {
        public int Id { get; set; }
        public decimal? PaymentAmount { get; set; }
        public int? BillId { get; set; }
        public int? PaymentId { get; set; }

        public virtual Bill Bill { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
