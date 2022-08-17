using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee.Models
{
    public partial class Account
    {
        public Account()
        {
            Posts = new HashSet<Post>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? CustomerId { get; set; }
        public int? MediaId { get; set; }
        public bool? Status { get; set; }

        [NotMapped]
        public IFormFile FileUload { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Medium Media { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
