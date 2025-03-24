using Microsoft.EntityFrameworkCore;
using MiniAmazon.Models;

namespace MiniAmazon.Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defining PKs
            modelBuilder.Entity<OrderItems>()
                .HasKey(o => o.OrderItemID);
            modelBuilder.Entity<Orders>()
               .HasKey(o => o.OrderID);
            modelBuilder.Entity<Products>()
               .HasKey(o => o.ProductID);
            modelBuilder.Entity<Users>()
               .HasKey(o => o.UserID);


            // One-to-many relationship: User to Orders
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID);

            // Many-to-many relationship: Order and Products through OrderItems
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID);


            // Seeding Users
            modelBuilder.Entity<Users>().HasData(
                new Users { UserID = 1, Name = "John Doe", Email = "john@example.com", Password = "password123", Role = "Customer" },
                new Users { UserID = 2, Name = "Admin User", Email = "admin@example.com", Password = "adminpassword", Role = "Admin" }
            );

            // Seeding Products
            modelBuilder.Entity<Products>().HasData(
                new Products { ProductID = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 1500.00, Stock = 10, CreatedBy = "Admin" },
                new Products { ProductID = 2, Name = "Smartphone", Description = "A latest model smartphone", Price = 800.00, Stock = 20, CreatedBy = "Admin" }
            );

            // Seeding Orders (using the UserID from seeding Users)
            modelBuilder.Entity<Orders>().HasData(
                new Orders { OrderID = 1, UserID = 1, OrderDate = DateTime.Now, TotalAmount = 2300.00, Status = "Pending" }
            );

            // Seeding OrderItems (using the OrderID and ProductID from the seeded Orders and Products)
            modelBuilder.Entity<OrderItems>().HasData(
                new OrderItems { OrderItemID = 1, OrderID = 1, ProductID = 1, Quantity = 1, Price = 1500.00 },
                new OrderItems { OrderItemID = 2, OrderID = 1, ProductID = 2, Quantity = 1, Price = 800.00 }
            );


        }


       


    }
}
