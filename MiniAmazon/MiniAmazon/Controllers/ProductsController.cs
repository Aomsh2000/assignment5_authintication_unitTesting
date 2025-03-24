using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniAmazon.Data;
using System.Data;
using System.Linq;
using MiniAmazon.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace MiniAmazon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        // Constructor to inject the Dapper connection
        public ProductsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _dbConnection = new MySqlConnection(connectionString);
        }
        // GET api/products
        [HttpGet]
        public IActionResult GetProducts()
        {
            // Define the query to fetch all products
            string query = "SELECT * FROM Products";

            // Execute the query and retrieve the products
            var products = _dbConnection.Query<Products>(query).ToList();

            if (products == null || !products.Any())
            {
                return NotFound("No products available.");
            }

            return Ok(products); // Return the list of products
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            // Define the query to fetch the product by id
            string query = "SELECT ProductID, Name, Price, Description FROM Products WHERE ProductID = @ProductID";


            var product = _dbConnection.QuerySingleOrDefault<Products>(query, new { ProductID = id });

            if (product == null)
            {
                return NotFound(); 
            }

            return Ok(product); 
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public IActionResult AddProduct(Products product)
        {
           
            return Ok("Product added successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(int id, Products product)
        {
          
            return Ok("Product updated successfully.");
        }
    }

}
