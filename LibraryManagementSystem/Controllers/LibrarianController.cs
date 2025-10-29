using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class LibrarianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrarianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Librarian/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                ActiveBorrowings = await _context.Transactions.CountAsync(t => t.ReturnDate == null),
                TotalTransactions = await _context.Transactions.CountAsync(),
                AvailableBooks = await _context.Books.CountAsync(b => b.AvailableCopies > 0),
                TotalWishlists = await _context.Wishlists.CountAsync(),
                OverdueBooks = await _context.Transactions.CountAsync(t => t.ReturnDate == null && t.BorrowedDate < DateTime.Now.AddDays(-14))
            };

            ViewBag.Stats = stats;

            // Recent transactions
            var recentTransactions = await _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Book)
                .OrderByDescending(t => t.BorrowedDate)
                .Take(10)
                .ToListAsync();

            ViewBag.RecentTransactions = recentTransactions;

            return View();
        }
    }
}
