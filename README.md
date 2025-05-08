
## Description

UsersApp is a web application built with ASP.NET Core MVC, allowing users to browse and purchase products, manage orders, write reviews, and interact with a customer support ticket system. The project follows the **Clean Architecture** approach, separating logic into different layers for better scalability and maintainability.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features
- User authentication and role-based authorization
- Product browsing, search, and detailed product pages
- Order management and purchase history
- Review system for products
- Customer support ticket system
- Admin panel for managing users, products, and orders## Installation
Open the solution and navigate in the solution explorer to appsettings.json and connect your database example:
"ConnectionStrings": {
    "Default": "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=UserApp;Integrated Security=True;Encrypt=True"
},
And run these commands
```sh
Add-Migration UserApp
```
```sh
Update-Database
```

### Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- [Visual Studio](https://visualstudio.microsoft.com/) with the ASP.NET and web development workload
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or another database provider if applicable)

### Steps

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/aceturic/ASP.NET-MVC-PROJECT.git

