using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Repositories;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Controllers
{
    public class GenerosController : Controller
    {
        private GenerosRepository repo;

        public GenerosController(GenerosRepository repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> _MenuGeneros()
        {
            List<Genero> generos =
                await this.repo.GetAllGenerosAsync();

            return PartialView("_MenuGeneros", generos);
        }
    }
}
