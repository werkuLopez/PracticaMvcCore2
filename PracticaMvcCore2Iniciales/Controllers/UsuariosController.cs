using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Filters;
using PracticaMvcCore2Iniciales.Repositories;

namespace PracticaMvcCore2Iniciales.Controllers
{
    public class UsuariosController : Controller
    {
        private UsuariosRepository repo;
        public UsuariosController(UsuariosRepository repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsuario]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
