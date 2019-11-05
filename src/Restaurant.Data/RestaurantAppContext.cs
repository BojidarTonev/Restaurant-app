using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Models;

namespace Restaurant.Data
{
    public class RestaurantAppContext : IdentityDbContext<RestaurantUser>
    {
        public RestaurantAppContext(DbContextOptions<RestaurantAppContext> options)
            : base(options)
        {
        }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}