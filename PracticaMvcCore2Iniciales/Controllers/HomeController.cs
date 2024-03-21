using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Models;
using PracticaMvcCore2Iniciales.Repositories;
using System.Diagnostics;

namespace PracticaMvcCore2Iniciales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LibrosRepository repo;

        public HomeController(ILogger<HomeController> logger, LibrosRepository repo)
        {
            _logger = logger;
            this.repo= repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros =
                await this.repo.GetAllLibrosAsync();

            return View(libros);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
