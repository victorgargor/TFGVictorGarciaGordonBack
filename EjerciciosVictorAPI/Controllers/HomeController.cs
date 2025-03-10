using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();  // Si quieres mostrar una página de error personalizada
        }
    }
}
