using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAmazon.Controllers;
using MiniAmazon.Data;
using MiniAmazon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MiniAmazon.Tests
{
    public class OrdersControllerTests
    {
        private StoreDbContext _context;
        private  OrdersController _controller;

        public OrdersControllerTests()
        {
            // Initialize with an empty database before each test
            InitializeContext();
        }

        // Initialize context with a new in-memory database for each test
        private void InitializeContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use unique name to avoid collisions
                .Options;

            _context = new StoreDbContext(options);

            // Seed the database with test data
            SeedDatabase();
            _controller = new OrdersController(_context);
        }

        // Seed method to populate the in-memory database with test data
        private void SeedDatabase()
        {
            var users = new List<Users>
            {
                new Users { UserID = 1, Name = "John Doe", Email = "john@example.com", Password = "password123", Role = "Customer" },
                new Users { UserID = 2, Name = "Admin User", Email = "admin@example.com", Password = "adminpassword", Role = "Admin" }
            };

            var products = new List<Products>
            {
                new Products { ProductID = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 1500.00, Stock = 10, CreatedBy = "Admin" },
                new Products { ProductID = 2, Name = "Smartphone", Description = "A latest model smartphone", Price = 800.00, Stock = 20, CreatedBy = "Admin" }
            };

            var orders = new List<Orders>
            {
                new Orders { OrderID = 1, UserID = 1, OrderDate = DateTime.Now, TotalAmount = 2300.00, Status = "Pending" },
                new Orders { OrderID = 2, UserID = 1, OrderDate = DateTime.Now, TotalAmount = 3200.00, Status = "Completed" }
            };

            var orderItems = new List<OrderItems>
            {
                new OrderItems { OrderItemID = 1, OrderID = 1, ProductID = 1, Quantity = 1, Price = 1500.00 },
                new OrderItems { OrderItemID = 2, OrderID = 1, ProductID = 2, Quantity = 1, Price = 800.00 },
                new OrderItems { OrderItemID = 3, OrderID = 2, ProductID = 1, Quantity = 1, Price = 1500.00 },
                new OrderItems { OrderItemID = 4, OrderID = 2, ProductID = 2, Quantity = 2, Price = 800.00 }
            };

            _context.Users.AddRange(users);
            _context.Products.AddRange(products);
            _context.Orders.AddRange(orders);
            _context.OrderItems.AddRange(orderItems);

            _context.SaveChanges();
        }

        [Fact]
        public void GetOrdersByUserId_ReturnsOrders_ForValidUserId()
        {
            // Arrange
            int userId = 1;  // Valid user ID

            // Act
            var result = _controller.GetOrdersByUserId(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var orders = result.Value as List<Orders>;
            Assert.Equal(2, orders.Count);  // Two orders for user ID 1
        }

        [Fact]
        public void GetOrdersByUserId_ReturnsNotFound_ForNonExistentUserId()
        {
            // Arrange
            int userId = 999;  // Non-existent user ID

            // Act
            var result = _controller.GetOrdersByUserId(userId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"No orders found for User ID {userId}.", result.Value);
        }

        [Fact]
        public void CreateOrder_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newOrder = new Orders
            {
                OrderID = 3,
                UserID = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 300,
                Status = "Pending"
            };

            // Act
            var result = _controller.CreateOrder(newOrder) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            var createdOrder = result.Value as Orders;
            Assert.Equal(newOrder.OrderID, createdOrder.OrderID);
            Assert.Equal(newOrder.UserID, createdOrder.UserID);
            Assert.Equal(newOrder.Status, createdOrder.Status);
        }
    }
}
