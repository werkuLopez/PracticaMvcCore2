using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2Iniciales.Models;
using PracticaMvcCore2Iniciales.Repositories;

namespace PracticaMvcCore2Iniciales.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private GenerosRepository repo;

        public MenuGenerosViewComponent(GenerosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos =
                await repo.GetAllGenerosAsync();

            return View(generos);
        }
    }
}
