using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
 
namespace wedding.Models
{
    public class weddingContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public weddingContext(DbContextOptions<weddingContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<WeddingPlanner> Weddings { get; set; }
        public DbSet<UserWedding> Guests { get; set; }
    }
}
