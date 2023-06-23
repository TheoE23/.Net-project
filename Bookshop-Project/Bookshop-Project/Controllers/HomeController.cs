using Bookshop_Project.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookshop_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly OpenLibraryService _openLibraryService;

        public HomeController(OpenLibraryService openLibraryService)
        {
            _openLibraryService = openLibraryService;
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
                // Handle empty query
                return RedirectToAction("Index");
            }

            var bookViewModels = await _openLibraryService.SearchBooks( q);

            return View("Books", bookViewModels);
        }
    }
}
