using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public string Gender { get; set; }
        public DateTime Birth { get; set; }
        public string Phone { get; set; }
        public int? Point { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
