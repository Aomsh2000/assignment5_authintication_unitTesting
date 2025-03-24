# assignment5_authintication_unitTesting

## Project Overview

Mini Amazon Clone is a mini e-commerce platform built using .NET Core. This platform supports user authentication, product listings, order management, and secure access control. Users can sign up, log in, view products, and place orders, while admins can manage products and perform other administrative tasks.

## Features

- User Registration and Login (JWT Authentication)
- Admin role for managing products
- Customer role for browsing products and placing orders
- Secure access to user profile and order details using claims-based and policy-based authorization
- Unit testing to ensure the system functions as expected

## Technologies Used

- **.NET Core** for backend API development
- **Entity Framework Core** for ORM-based database management
- **Dapper** for optimized queries
- **JWT Authentication** for secure API access
- **BCrypt.Net** for password hashing
- **xUnit** and **Moq** for unit testing

## Installation Instructions

### Prerequisites

- Install the [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (version 5 or above).
- Install a code editor like [Visual Studio Code](https://code.visualstudio.com/).

### Setup

1. **Clone the repository:**
```bash
   git clone https://github.com/your-username/mini-amazon-clone.git
   cd mini-amazon-clone
   ```
Install dependencies: Run the following command to restore dependencies:

```bash
dotnet restore
```
Apply database migrations: If you’re using Entity Framework, you need to apply migrations to set up your database:

```bash
dotnet ef database update
```
Run the application: To start the application, run:

```bash

dotnet run
```
## API Endpoints
- POST /register - Register a new user
- POST /login - Authenticate and return JWT token
- GET /products - Fetch all products
- POST /orders/create - Place a new order
- GET /orders/{id} - Get details of a specific order

## Running Tests
To run the tests:

Ensure all dependencies are installed.
Run the following command to execute the unit tests:
```bash
 
dotnet test
```