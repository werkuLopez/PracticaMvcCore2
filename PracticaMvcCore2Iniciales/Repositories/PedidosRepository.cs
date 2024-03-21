using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Iniciales.Context;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Repositories
{
    public class PedidosRepository
    {
        private LibrosContext context;

        public PedidosRepository(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Pedido>> GetAllpedidosByUsuario(int idusuario)
        {
            List<Pedido> pedidos = await this.context.Pedidos.Where(x => x.IdUsuario == idusuario).ToListAsync();
            return pedidos;
        }
    }
}
