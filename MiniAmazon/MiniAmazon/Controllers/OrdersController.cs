using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAmazon.Data;
using System.Security.Claims;
using MiniAmazon.Models;
using Mysqlx.Crud;

namespace MiniAmazon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public OrdersController(StoreDbContext context)
        {
            _context = context;
        }

        // Endpoint to fetch all orders with products
        [HttpGet]
        public IActionResult GetOrdersWithProducts()
        {
            // Eager loading of Orders with their OrderItems and Products
            var ordersWithProducts = _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .ToList();

            return Ok(ordersWithProducts);
        }

        // Endpoint to fetch a specific order by ID
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }
        // Endpoint to get all orders for a specific user by their UserID
        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();

            if (!orders.Any())
            {
                return NotFound($"No orders found for User ID {userId}.");
            }

            return Ok(orders);
        }

        // Endpoint to create a new order
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] Orders order)
        {
            if (order == null)
            {
                return BadRequest("Order data is invalid.");
            }

            // Set the order date to the current date and time
            order.OrderDate = DateTime.UtcNow;

            // Add the order to the database
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, order);
        }

        // Endpoint to get all orders (authorized)
        [Authorize(Policy = "CanViewOrders")]
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            return Ok("All orders are displayed.");
        }

        // Endpoint to refund an order (authorized)
        [Authorize(Policy = "CanRefundOrdersPolicy")]
        [HttpPost("refund")]
        public IActionResult RefundOrder(Orders order)
        {
            return Ok("Order refunded successfully.");
        }
    }
}