using Microsoft.AspNetCore.Mvc;

namespace Bookshop_Project.Controllers
{
    public class FeedBackController : Controller
    {
        public IActionResult FeedBack()
        {
            return View();
        }
    }
}
