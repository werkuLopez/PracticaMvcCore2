using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2Iniciales.Context;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Repositories
{
    public class UsuariosRepository
    {
        private LibrosContext context;

        public UsuariosRepository(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<Usuario> LogIn(string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (usuario == null)
            {
                return null;
            }
            else
            {
                return usuario;
            }
        }

        public async Task<Usuario> SignIn(string nombre, string apellido, string email, string password, string imagen)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (usuario == null)
            {
                var maxId = await context.Usuarios.MaxAsync(r => (int?)r.IdUsuario) ?? 0;
                int nextId = maxId + 1;

                usuario = new Usuario
                {
                    IdUsuario = nextId,
                    Nombre = nombre,
                    Apellidos = apellido,
                    Foto = imagen,
                    Email = email,
                    Password = password
                };

                await this.context.AddAsync(usuario);
                await this.context.SaveChangesAsync();
            }
            return usuario;
        }

        public async Task<Pedido> RealizarPedido(int idusuario, int idLibro, int idFactura, int cantidad, DateTime fecha)
        {
            var maxId = await context.Pedidos.MaxAsync(r => (int?)r.IdPedido) ?? 0;
            int nextId = maxId + 1;

            Pedido pedido = new Pedido
            {
                IdPedido=nextId,
                IdFactura= idFactura,
                IdLibro= idLibro,
                IdUsuario= idusuario,
                Cantidad= cantidad,
                Fecha= fecha
            };

            await this.context.Pedidos.AddAsync(pedido);
            await this.context.SaveChangesAsync();

            return pedido;
        }        

        public async Task<List<Usuario>> GetAllUsers()
        {
            return await this.context.Usuarios.ToListAsync();
        }
    }
}
