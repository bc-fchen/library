using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
