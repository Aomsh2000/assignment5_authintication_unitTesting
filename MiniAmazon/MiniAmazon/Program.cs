using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MiniAmazon.Data;
using MySqlConnector;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretkey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretkey)
        };
    });

//Refund Orders Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRefundOrdersPolicy", policy =>
        policy.RequireRole("Admin")
              .RequireClaim("CanRefundOrders", "true"));
    options.AddPolicy("CanViewOrders", policy =>
       policy.RequireClaim("CanViewOrders", "true"));
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
