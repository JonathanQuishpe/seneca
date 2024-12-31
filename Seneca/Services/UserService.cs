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
            Usuario usuario = await _context.Usuarios.Where(u => u.CorreoElectronico == correo && u.Contrasenia == clave && u.Estado == 1).FirstOrDefaultAsync();

            return usuario;

        }

        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> FindUser(long id)
        {
            Usuario usuario = await _context.Usuarios.FindAsync(id);

            return usuario;
        }

        public async Task<Usuario> UpdateUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> FindUserByEmail(string correo)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.CorreoElectronico == correo).FirstOrDefaultAsync();

            return usuario;

        }
        public async Task<Log> RegisterLog(Usuario usuario)
        {
            Log log = await _context.Logs.Where(u => u.UsuarioId == usuario.Id).FirstOrDefaultAsync();

            if (log == null)
            {
                log = new Log
                {
                    UsuarioId = usuario.Id,
                    Fecha = DateTime.Now
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
                return log;
            }

            log.Fecha = DateTime.Now;

            _context.Logs.Update(log);
            await _context.SaveChangesAsync();
            return log;
        }
    }
}
