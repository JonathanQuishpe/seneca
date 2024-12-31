using Seneca.Models.Entities;

namespace Seneca.Services
{
    public interface IUserService
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario usuario);
        Task<Usuario> FindUser(long id);
        Task<Usuario> UpdateUsuario(Usuario usuario);
        Task<Usuario> FindUserByEmail(string correo);
        Task<Log> RegisterLog(Usuario usuario);
    }
}
