using Bookshop_Project.Data;
using Bookshop_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bookshop_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly OpenLibraryService _openLibraryService;
        private readonly ApplicationDbContext _context;

        public HomeController(OpenLibraryService openLibraryService, ApplicationDbContext context)
        {
            _openLibraryService = openLibraryService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Books(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index");
            }

            var bookViewModels = await _openLibraryService.SearchBooks( q);

            return View("Books", bookViewModels);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddPrice(string bookId, decimal price)
        {
            var bookPrice = new BookPrice
            {
                key = bookId,
                Price = price
            };

            _context.BookPrices.Add(bookPrice);

            var existingRequest = await _context.BookPriceRequests.FirstOrDefaultAsync(r => r.key == bookId);
            if (existingRequest != null)
            {
                _context.BookPriceRequests.Remove(existingRequest);
            }

            await _context.SaveChangesAsync();
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> AskPrice(string bookId, string title, string author)
        {
            var existingRequest = _context.BookPriceRequests.FirstOrDefault(r => r.key == bookId);

            if (existingRequest == null)
            {

                var bookPriceRequest = new BookPriceRequest
                {
                    key = bookId,
                    Title = title,
                    Author = author
                };
                _context.BookPriceRequests.Add(bookPriceRequest);
                await _context.SaveChangesAsync();
            }
            return View("Index");
        }

        public async Task<IActionResult> PriceRequests()
        {
            var bookPriceRequests = await _context.BookPriceRequests.ToListAsync();

            return View(bookPriceRequests);
        }
    }
}
