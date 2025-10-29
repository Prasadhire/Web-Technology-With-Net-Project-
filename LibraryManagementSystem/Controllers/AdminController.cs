using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalLibrarians = await _context.Users.CountAsync(u => u.Role == "Librarian"),
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

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Name)
                .ToListAsync();

            return View(users);
        }

        // POST: Admin/PromoteToLibrarian/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToLibrarian(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = "Librarian";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{user.Name} has been promoted to Librarian!";
            return RedirectToAction("Users");
        }

        // POST: Admin/DemoteToUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemoteToUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = "User";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{user.Name} has been demoted to User!";
            return RedirectToAction("Users");
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent deleting own account
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            if (user.UserID == currentUserId)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account!";
                return RedirectToAction("Users");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{user.Name}'s account has been deleted!";
            return RedirectToAction("Users");
        }

        // GET: Admin/SystemStats
        public async Task<IActionResult> SystemStats()
        {
            // Monthly stats
            var monthlyStats = await _context.Transactions
                .Where(t => t.BorrowedDate >= DateTime.Now.AddMonths(-6))
                .GroupBy(t => new { t.BorrowedDate.Year, t.BorrowedDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    BorrowCount = g.Count(),
                    ReturnCount = g.Count(t => t.ReturnDate != null)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            ViewBag.MonthlyStats = monthlyStats;

            // Popular books
            var popularBooks = await _context.Transactions
                .GroupBy(t => t.Book)
                .Select(g => new
                {
                    Book = g.Key,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(g => g.BorrowCount)
                .Take(10)
                .ToListAsync();

            ViewBag.PopularBooks = popularBooks;

            // Active users
            var activeUsers = await _context.Transactions
                .Where(t => t.BorrowedDate >= DateTime.Now.AddMonths(-1))
                .GroupBy(t => t.User)
                .Select(g => new
                {
                    User = g.Key,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(g => g.BorrowCount)
                .Take(10)
                .ToListAsync();

            ViewBag.ActiveUsers = activeUsers;

            return View();
        }
    }
}