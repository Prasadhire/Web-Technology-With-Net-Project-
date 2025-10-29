using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index(string searchString, string category, int page = 1, int pageSize = 8)
        {
            var books = _context.Books.AsQueryable();

            // Search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => 
                    b.Title.Contains(searchString) || 
                    b.Author.Contains(searchString) ||
                    b.Category.Contains(searchString)
                );
            }

            // Category filter
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                books = books.Where(b => b.Category == category);
            }

            // Get distinct categories for dropdown
            ViewBag.Categories = await _context.Books
                .Select(b => b.Category)
                .Distinct()
                .ToListAsync();

            // Pass current filters to view
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = category;

            // Pagination
            var totalBooks = await books.CountAsync();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

            var bookList = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            // Statistics for view
            ViewBag.TotalBooks = totalBooks;
            ViewBag.TotalCategories = ViewBag.Categories.Count;
            ViewBag.TotalAvailableCopies = bookList.Sum(b => b.AvailableCopies);
            ViewBag.AvailableBooks = bookList.Count(b => b.AvailableCopies > 0);
            
            // Pagination info
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(bookList);
        }

        // GET: Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: Home/Privacy  
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Home/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}