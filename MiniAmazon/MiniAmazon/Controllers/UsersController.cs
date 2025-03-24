using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniAmazon.Models;
using System.Data;
using System.Security.Claims;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace MiniAmazon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;

        // Constructor to inject the Dapper connection and IConfiguration
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _dbConnection = new MySqlConnection(connectionString);
        }

        // GET api/users/users-orders
        [HttpGet("users-orders/{userId}")]
        public IActionResult GetCustomersOrders(int userId)
        {
            // Define the query to fetch orders for a specific user
            string query = "SELECT * FROM Orders WHERE UserID = @UserID";

            // Execute the query and retrieve the orders
            var orders = _dbConnection.Query<Orders>(query, new { UserID = userId }).ToList();

            // If no orders are found, return a 404
            if (orders.Count == 0)
            {
                return NotFound($"No orders found for user with ID {userId}");
            }

            // Return the list of orders
            return Ok(orders);
        }

        // POST api/users/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistration model)
        {
            if (model == null)
                return BadRequest("Invalid registration data");

            // Check if the user already exists
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            var userExists = _dbConnection.ExecuteScalar<int>(query, new { model.Email });

            if (userExists > 0)
            {
                return Conflict("User with the given email already exists.");
            }

            // Insert the new user into the database
            string insertQuery = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
            _dbConnection.Execute(insertQuery, new { model.Name, model.Email, Password = BCrypt.Net.BCrypt.HashPassword(model.Password), model.Role });

            return Ok("User registered successfully");
        }

        // POST api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            if (model == null)
                return BadRequest("Invalid login data");

            // Fetch the user by email
            string query = "SELECT * FROM Users WHERE Email = @Email";
            var user = _dbConnection.QuerySingleOrDefault<Users>(query, new { model.Email });

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            // Create a JWT token for the authenticated user
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // GET api/users/profile (Secured endpoint)
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Get the user info from the authenticated user (JWT Token)
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Fetch user details from the database
            string query = "SELECT * FROM Users WHERE UserID = @UserID";
            var user = _dbConnection.QuerySingleOrDefault<Users>(query, new { UserID = userId });

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        // Helper method to generate JWT token
        private string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim("CanViewOrders", "true")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
