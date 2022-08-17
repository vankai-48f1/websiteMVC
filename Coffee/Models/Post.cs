using System;
using System.Collections.Generic;

namespace Coffee.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Status { get; set; }
        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
