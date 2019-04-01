using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error(string error)
        {
            return View(new ErrorViewModel { ErrorMessage = error });
        }
    }
}
