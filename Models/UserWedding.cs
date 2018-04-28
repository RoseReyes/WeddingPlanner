using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace wedding.Models
{
    public class UserWedding 
    {
            [Key]
            public int UserWeddingId { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }
            public int WeddingId { get; set; }
            public WeddingPlanner Wedding { get; set;}
            public DateTime Created_at { get; set; }
            public DateTime Updated_at { get; set; }
            public UserWedding () {
                this.Created_at = DateTime.Now;
                this.Updated_at = DateTime.Now;
            }
    }
}