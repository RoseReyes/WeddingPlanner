using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace wedding.Models
{
    public class User 
    {
            [Key]
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            List<UserWedding> Guestwedding { get; set; }
            List<WeddingPlanner> Weddings { get; set; }
            
            public User(){
                Weddings = new List<WeddingPlanner>();
                Guestwedding = new List<UserWedding> ();
                this.Created_at = DateTime.Now;
                this.Updated_at = DateTime.Now;
            }
            public DateTime Created_at { get; set; }
            public DateTime Updated_at { get; set; }

    }
}