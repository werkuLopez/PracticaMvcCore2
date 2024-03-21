using PracticaMvcCore2Iniciales;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Helpers;
using PracticaMvcCore2Iniciales.Models;
using PracticaMvcCore2Iniciales.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2Iniciales.Controllers
{
    public class ManagedController : Controller
    {
        private UsuariosRepository repo;
        private HelperUploadFiles uploadFiles;

        public ManagedController(UsuariosRepository repo, HelperUploadFiles uploadFiles)
        {
            this.repo = repo;
            this.uploadFiles= uploadFiles;
        }
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Usuario usuario = await this.repo.LogIn(email, password);

            if(usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim clamiName = new Claim(ClaimTypes.Name, usuario.Email);
                identity.AddClaim(clamiName);

                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["mensaje"] = "Email o contraseña incorrectas";
                return View();
            }
        }

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(string nombre, string apellido, string email, string pass, IFormFile file)
        {
            Usuario usuario;
            if (file != null)
            {
               string ruta = await this.uploadFiles.UploadFileAsync(file, Folder.Imagenes);

               usuario  = await this.repo.SignIn(nombre, apellido, email, pass, file.FileName);
            }
            else
            {
                ViewData["mensaje"] = "Debe asignar una imagen al usuario";
                return View();
            }

            return View(usuario);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
