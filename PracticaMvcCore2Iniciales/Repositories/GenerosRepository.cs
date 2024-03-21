using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Iniciales.Context;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Repositories
{
    public class GenerosRepository
    {
        private LibrosContext context;

        public GenerosRepository(LibrosContext context)
        {
            this.context = context; 
        }

        public async Task<List<Genero>> GetAllGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
    }
}
