using Seneca.Models.Entities;

namespace Seneca.Services
{
    public interface IUserService
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario usuario);
    }
}
