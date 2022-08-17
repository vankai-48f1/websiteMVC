using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Session
    {
        public Session()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Closed { get; set; }
        public bool? Status { get; set; }
        public decimal? Total { get; set; }
        public decimal? CashTotal { get; set; }
        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
