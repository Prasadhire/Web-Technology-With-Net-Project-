# Digital Library Management System

A comprehensive ASP.NET Core MVC Library Management System with role-based authentication, book management, borrowing system, and admin dashboard.

## 🚀 Features

### 🔐 Authentication & Authorization
- **User Registration & Login** with email and password
- **Role-based Access Control** (Admin, Librarian, User)
- **Secure Password Hashing** using BCrypt
- **Session Management** with cookies

### 👥 User Roles
- **Admin**: Full system access, user management, system analytics
- **Librarian**: Book management, transaction oversight
- **User**: Browse books, borrow/return, wishlist

### 📚 Book Management
- **Add/Edit/Delete Books** (Admin & Librarian)
- **Book Search & Filtering** by title, author, category
- **Available Copies Tracking**
- **Book Categories** organization

### 🔄 Borrowing System
- **Borrow & Return Books** with availability checks
- **Borrowing History** for each user
- **Due Date Management**
- **Transaction Records**

### ❤️ Wishlist Feature
- **Save Books** for later reading
- **Move to Borrow** directly from wishlist
- **Wishlist Count** in navigation

### 📊 Admin Dashboard
- **System Statistics** with charts and metrics
- **User Management** (promote/demote/delete)
- **Transaction Overview**
- **Popular Books Analytics**

### 🎨 User Experience
- **Responsive Design** with Bootstrap 5
- **Modern UI** with animations and hover effects
- **SweetAlert2** for beautiful notifications
- **Pagination** for better performance
- **Loading Animations**

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core MVC, Entity Framework Core
- **Database**: SQL Server with EF Core Migrations
- **Frontend**: Bootstrap 5, jQuery, Font Awesome
- **Authentication**: Cookie-based with Claims
- **Security**: BCrypt for password hashing
- **Notifications**: SweetAlert2
- **Animations**: Animate.css

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

## 🚀 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/library-management-system.git
cd library-management-system
```

### 2. Database Setup
Update connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### 3. Run Database Migrations
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

Visit: `https://localhost:7000` or `http://localhost:5000`

## 👤 Default Accounts

### Admin Account
- **Email**: `admin@library.com`
- **Password**: `admin123`
- **Permissions**: Full system access

### Librarian Account  
- **Email**: `librarian@library.com`
- **Password**: `lib123`
- **Permissions**: Book management, transaction view

### User Account
- Register new account through registration page

## 📁 Project Structure

```
LibraryManagementSystem/
├── Controllers/
│   ├── AccountController.cs      # Authentication
│   ├── AdminController.cs        # Admin dashboard & user management
│   ├── BooksController.cs        # Book CRUD operations
│   ├── HomeController.cs         # Home page & browsing
│   ├── TransactionsController.cs # Borrow/return operations
│   └── WishlistController.cs     # Wishlist management
├── Models/
│   ├── User.cs                   # User model with roles
│   ├── Book.cs                   # Book model
│   ├── Transaction.cs            # Borrowing transactions
│   └── Wishlist.cs               # Wishlist items
├── Views/
│   ├── Account/                  # Login, register, profile
│   ├── Admin/                    # Dashboard, user management
│   ├── Books/                    # Book listings, details, forms
│   ├── Home/                     # Home page with books
│   ├── Transactions/             # Borrowing history
│   ├── Wishlist/                 # Wishlist management
│   └── Shared/                   # Layout, partial views
├── Data/
│   ├── ApplicationDbContext.cs   # Database context
│   └── DbInitializer.cs          # Seed data
├── wwwroot/
│   ├── css/site.css              # Custom styles
│   └── js/site.js                # Client-side scripts
└── Program.cs                    # Application startup
```

## 🔧 Configuration

### Database Connection
Update `appsettings.json` with your SQL Server instance:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Session Configuration
Sessions are configured for 30 minutes with secure cookies.

### Authentication Settings
- Cookie-based authentication
- 7-day persistent login
- Role-based authorization

## 🎯 Key Features Detailed

### Book Management
- **CRUD Operations**: Create, read, update, delete books
- **Search & Filter**: By title, author, category
- **Availability Tracking**: Real-time copy availability
- **Pagination**: Efficient loading for large catalogs

### User Management
- **Role Assignment**: Admin, Librarian, User roles
- **Profile Management**: Update personal information
- **Activity Tracking**: Borrowing history and statistics

### Borrowing System
- **Availability Checks**: Prevent over-borrowing
- **Transaction History**: Complete borrowing records
- **Return Management**: Easy book returns

### Admin Features
- **Dashboard Analytics**: System overview with metrics
- **User Management**: Role changes and user administration
- **System Reports**: Popular books, active users, trends

## 🚀 Deployment

### Local Development
```bash
dotnet run
```

### Production Deployment
1. Publish the application:
```bash
dotnet publish -c Release
```

2. Deploy to IIS or cloud platform (Azure, AWS)

3. Update production connection string

4. Run migrations on production database

## 🤝 Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify SQL Server is running
   - Check connection string in appsettings.json
   - Ensure database exists

2. **Migration Errors**
   - Delete existing database
   - Run `dotnet ef database update`
   - Or use `EnsureCreated()` in development

3. **Authentication Issues**
   - Clear browser cookies
   - Restart application
   - Check role assignments

### Support

For support, email hireprasad002@gmail.com.

## 📞 Contact

Your Name - Prasad Hire | hireprasad002@gmail.com

Project Link: https://github.com/Prasadhire/Web-Technology-With-Net-Project-.git

---

## 🎉 Acknowledgments

- [Bootstrap](https://getbootstrap.com/) for responsive design
- [Font Awesome](https://fontawesome.com/) for icons
- [SweetAlert2](https://sweetalert2.github.io/) for beautiful alerts
- [Animate.css](https://animate.style/) for animations

---

**⭐ Star this repo if you found it helpful!**

# How to Upload This Project to GitHub

## Step 1: Create GitHub Repository

1. Go to [GitHub.com](https://github.com)
2. Click the **+** icon in top right → **New repository**
3. Repository name: `library-management-system`
4. Description: `A comprehensive ASP.NET Core MVC Library Management System`
5. Choose **Public** or **Private**
6. **Don't** initialize with README (we'll add ours)
7. Click **Create repository**

## Step 2: Prepare Your Local Project

### Initialize Git (if not already done)
```bash
# Check if git is initialized
git status

# If not initialized, run:
git init
```

### Create .gitignore File
Create `.gitignore` file in project root:
```
## .NET ##
*.swp
*.*~
project.lock.json
.DS_Store
*.pyc

# OS generated files
.DS_Store
.DS_Store?
._*
.Spotlight-V100
.Trashes
ehthumbs.db
Thumbs.db

# VS Code
.vscode/
```

