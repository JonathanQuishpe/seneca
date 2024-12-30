using Microsoft.EntityFrameworkCore;
using Seneca.Models.Entities;

namespace Seneca.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context) { 
            _context = context;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.CorreoElectronico == correo && u.Contrasenia == clave).FirstOrDefaultAsync();

            return usuario;

        }

        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
