using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            // Add Admin User
            var admin = new User
            {
                Name = "Admin User",
                Email = "admin@library.com",
                MobileNumber = "9876543210",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            };
            context.Users.Add(admin);

            // Add Librarian User
            var librarian = new User
            {
                Name = "Librarian User", 
                Email = "librarian@library.com",
                MobileNumber = "9876543211",
                Password = BCrypt.Net.BCrypt.HashPassword("lib123"),
                Role = "Librarian"
            };
            context.Users.Add(librarian);

            // Add Sample Books
            var books = new Book[]
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Category = "Fiction",
                    AvailableCopies = 5
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Category = "Fiction",
                    AvailableCopies = 3
                },
                new Book
                {
                    Title = "The C Programming Language",
                    Author = "Brian Kernighan, Dennis Ritchie",
                    Category = "Programming",
                    AvailableCopies = 2
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    Category = "Science Fiction",
                    AvailableCopies = 4
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Category = "Romance",
                    AvailableCopies = 3
                }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}