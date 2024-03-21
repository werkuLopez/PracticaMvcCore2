using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Models;
using PracticaMvcCore2Iniciales.Extensions;
using PracticaMvcCore2Iniciales.Repositories;
using PracticaMvcCore2Iniciales.Filters;
using System.Security.Claims;

namespace PracticaMvcCore2Iniciales.Controllers
{
    public class LibrosController : Controller
    {
        private LibrosRepository repo;
        private GenerosRepository repoGenero;
        private UsuariosRepository repouser;

        public LibrosController(LibrosRepository repo,
            GenerosRepository generos,
            UsuariosRepository user)
        {
            this.repo = repo;
            this.repoGenero = generos;
            this.repouser = user;
        }

        public async Task<IActionResult> Index(int? idgenero)
        {
            List<Libro> libros;

            if (idgenero != null)
            {
                libros = await this.repo.GetLibrosByGeneroAsync(idgenero.Value);
            }
            else
            {
                libros =
                    await this.repo.GetAllLibrosAsync();
            }

            return View(libros);
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> AddLibroCarrito(int? idlibro)
        {
            if (idlibro != null)
            {

                List<int> carrito;

                if (HttpContext.Session.GetString("carrito") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("carrito");
                }

                carrito.Add(idlibro.Value);
                HttpContext.Session.SetObject("carrito", carrito);
            }

            return RedirectToAction("Index");
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> Carrito()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("carrito");

            if (carrito != null)
            {
                List<Libro> libros = await this.repo.GetLibrosSession(carrito);
                return View(libros);
            }
            return View();
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> EliminarLibroCarrito(int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> carrito =
                    HttpContext.Session.GetObject<List<int>>("carrito");
                carrito.Remove(idlibro.Value);
                if (carrito.Count() == 0)
                {
                    HttpContext.Session.Remove("carrito");
                }
                else
                {
                    HttpContext.Session.SetObject("carrito", carrito);
                }
            }
            return RedirectToAction("Carrito");
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> RealizarCompra()
        {

            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("carrito");

            List<Pedido> pedidos =
                await this.repo.RealizarPedido(carrito, idusuario);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> InsertarLibro()
        {
            List<Genero> generos =
                await this.repoGenero.GetAllGenerosAsync();

            return View(generos);
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> PedidosUsuario()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Pedido> pedidos = await this.repo.PedidosUsuario(idusuario);

            ViewData["usuarios"] = await this.repouser.GetAllUsers();
            ViewData["libros"] = await this.repo.GetAllLibrosAsync();
            return View(pedidos);
        }
    }
}
