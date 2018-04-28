using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace wedding.Models
{
    public class WeddingPlanner 
    {
            [Key]
            public int WeddingPlannerId { get; set; }

            public string WedderOne { get; set; }
            public string WedderTwo { get; set; }
            public DateTime Date { get; set; }
            public string WedAddress { get; set; }

            public int UserId { get; set; }
            public User user { get; set; }
            public List<UserWedding> guests { get; set; }
            public WeddingPlanner() {
                
                guests = new List<UserWedding>();
                this.Created_at = DateTime.Now;
                this.Updated_at = DateTime.Now;
                this.Date = DateTime.Now;
            }
            public DateTime Created_at { get; set; }
            public DateTime Updated_at { get; set; }
    }
}