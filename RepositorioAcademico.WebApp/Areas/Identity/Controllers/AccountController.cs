using Microsoft.AspNetCore.Mvc;

namespace RepositorioAcademico.WebApp.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
