using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wishlist
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var wishlistItems = await _context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.UserID == userId)
                .OrderByDescending(w => w.AddedDate)
                .ToListAsync();

            return View(wishlistItems);
        }

        // POST: Wishlist/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Check if already in wishlist
            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserID == userId && w.BookID == id);

            if (existingItem != null)
            {
                TempData["ErrorMessage"] = "Book is already in your wishlist!";
                return RedirectToAction("Details", "Books", new { id });
            }

            var wishlistItem = new Wishlist
            {
                UserID = userId,
                BookID = id,
                AddedDate = DateTime.Now
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book added to wishlist successfully!";
            return RedirectToAction("Details", "Books", new { id });
        }

        // POST: Wishlist/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.WishlistID == id && w.UserID == userId);

            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book removed from wishlist!";
            return RedirectToAction("Index");
        }

        // POST: Wishlist/MoveToBorrow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToBorrow(int id)
        {
            var wishlistItem = await _context.Wishlists
                .Include(w => w.Book)
                .FirstOrDefaultAsync(w => w.WishlistID == id);

            if (wishlistItem == null)
            {
                return NotFound();
            }

            var book = wishlistItem.Book;
            if (book.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "No copies available for borrowing.";
                return RedirectToAction("Index");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Check if already borrowed
            var existingTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.UserID == userId && t.BookID == book.BookID && t.ReturnDate == null);

            if (existingTransaction != null)
            {
                TempData["ErrorMessage"] = "You have already borrowed this book.";
                return RedirectToAction("Index");
            }

            // Create transaction
            var transaction = new Transaction
            {
                UserID = userId,
                BookID = book.BookID,
                BorrowedDate = DateTime.Now
            };

            // Reduce available copies
            book.AvailableCopies--;

            // Remove from wishlist
            _context.Wishlists.Remove(wishlistItem);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book borrowed from wishlist successfully!";
            return RedirectToAction("Index", "Transactions");
        }

        // GET: Wishlist/GetCount
        [HttpGet]
        public async Task<JsonResult> GetCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(0);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var count = await _context.Wishlists
                .Where(w => w.UserID == userId)
                .CountAsync();
            return Json(count);
        }
    }
}