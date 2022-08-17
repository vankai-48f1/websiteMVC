using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Payment
    {
        public Payment()
        {
            DetailsPayments = new HashSet<DetailsPayment>();
        }

        public int Id { get; set; }
        public string PaymentName { get; set; }
        public bool? PaymentType { get; set; }

        public virtual ICollection<DetailsPayment> DetailsPayments { get; set; }
    }
}
