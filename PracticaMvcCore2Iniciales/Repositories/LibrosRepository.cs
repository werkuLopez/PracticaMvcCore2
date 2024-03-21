using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Iniciales.Context;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Repositories
{
    public class LibrosRepository
    {
        private LibrosContext context;

        public LibrosRepository(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetAllLibrosAsync()
        {
            List<Libro> consulta = await this.context.Libros.ToListAsync();
            return consulta;
        }

        public async Task<List<Libro>> GetLibrosSession(List<int> libros)
        {
            return await this.context.Libros
            .Where(l => libros.Contains(l.IdLibro))
            .ToListAsync();
        }

        public async Task<List<Pedido>> RealizarPedido(List<int> libros, int idusuario)
        {
            var maxId = await context.Pedidos.MaxAsync(r => (int?)r.IdPedido) ?? 0;
            int nextId = maxId + 1;
           // string fecha = DateTime.Now.ToShortDateString();
            List<Libro> listaLibros = await GetLibrosSession(libros);
            int idCompra = nextId;
            List<Pedido> compras = new List<Pedido>();
            foreach (Libro libro in listaLibros)
            {
                Pedido compra = new Pedido
                {
                    IdPedido = idCompra,
                    IdLibro = libro.IdLibro,
                    Cantidad = 1,
                    Fecha = DateTime.Now,
                    IdFactura= 1,
                    IdUsuario= idusuario

                };
                compras.Add(compra);
                this.context.Pedidos.AddAsync(compra);
            }
            await this.context.SaveChangesAsync();
            return compras;
        }
    }
}
