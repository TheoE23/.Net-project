﻿using Bookshop_Project.Data;
using Bookshop_Project.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddPrice(string bookId, decimal price)
        {
            var bookPrice = new BookPrice
            {
                key = bookId,
                Price = price
            };

            _context.BookPrices.Add(bookPrice);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
