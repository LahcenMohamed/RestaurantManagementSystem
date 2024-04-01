using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Models
{
    public sealed class RestaurantManagementSystemDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(@"data source=.\SQLEXPRESS;initial catalog=RestaurantManagementSystemDbWPF;integrated security=True;Encrypt=False;MultipleActiveResultSets=True");

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
