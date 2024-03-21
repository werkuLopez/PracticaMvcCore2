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

        public LibrosController(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros =
                await this.repo.GetAllLibrosAsync();

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

            if(carrito != null)
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
        public async Task<IActionResult> RealizarCompra(int idusuario)
        {
            //ClaimsPrincipal idusuario = HttpContext.User;
            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("carrito");

            List<Pedido> pedidos =
                await this.repo.RealizarPedido(carrito, idusuario);

            return RedirectToAction("Index");
        }
    }
}
