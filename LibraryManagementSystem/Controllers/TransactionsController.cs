using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions (Admin view)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Book)
                .OrderByDescending(t => t.BorrowedDate)
                .ToListAsync();

            return View(transactions);
        }

        // GET: Transactions/MyTransactions (User view)
        public async Task<IActionResult> MyTransactions()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(userIdClaim);

            var transactions = await _context.Transactions
                .Include(t => t.Book)
                .Where(t => t.UserID == userId)
                .OrderByDescending(t => t.BorrowedDate)
                .ToListAsync();

            return View(transactions);
        }

        // POST: Transactions/Borrow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "No copies available for borrowing.";
                return RedirectToAction("Details", "Books", new { id });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(userIdClaim);

            // Check if user already has this book borrowed and not returned
            var existingTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.UserID == userId && t.BookID == id && t.ReturnDate == null);

            if (existingTransaction != null)
            {
                TempData["ErrorMessage"] = "You have already borrowed this book.";
                return RedirectToAction("Details", "Books", new { id });
            }

            var transaction = new Transaction
            {
                UserID = userId,
                BookID = id,
                BorrowedDate = DateTime.Now
            };

            // Reduce available copies
            book.AvailableCopies--;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book borrowed successfully!";
            return RedirectToAction("MyTransactions");
        }

        // POST: Transactions/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            // Check if user owns this transaction or is admin
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(userIdClaim);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && transaction.UserID != userId)
            {
                return Forbid();
            }

            transaction.ReturnDate = DateTime.Now;
            transaction.Book.AvailableCopies++;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book returned successfully!";
            
            if (isAdmin)
                return RedirectToAction("Index");
            else
                return RedirectToAction("MyTransactions");
        }
    }
}