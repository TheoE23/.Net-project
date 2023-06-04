using Bookshop_Project.Models;
using Bookshop_Project.Services.Books;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Bookshop_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookProvider bookProvider;

        public HomeController(IBookProvider bookProvider)
        {
            this.bookProvider = bookProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Books(string q, int page = 1)
        {
            BookSearchViewModel model = await bookProvider.SearchBook(q, page);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}