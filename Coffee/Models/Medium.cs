using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee.Models
{
    public partial class Medium
    {
        public Medium()
        {
            Accounts = new HashSet<Account>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime TimeCreate { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
