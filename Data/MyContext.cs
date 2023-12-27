using Microsoft.EntityFrameworkCore;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Customer;
using POS_System_DotNet.Models.Order;
using POS_System_DotNet.Models.Product;

namespace POS_System_DotNet.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make the 'username' property unique
            modelBuilder.Entity<Account>().HasIndex(a => a.Username).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(a => a.Barcode).IsUnique();
            modelBuilder.Entity<Account>().Property(a => a.Picture).IsRequired(false);
            modelBuilder.Entity<Account>().Property(a => a.FullName).IsRequired(false);
            modelBuilder.Entity<Account>().Property(a => a.Phone).IsRequired(false);
            modelBuilder.Entity<Customer>().HasIndex(a => a.PhoneNumber).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
