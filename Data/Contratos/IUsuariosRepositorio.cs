using Models;
using System.Threading.Tasks;

namespace Data.Contratos
{
    public interface IUsuariosRepositorio : IRepositorioGenerico<Usuario>
    {
        Task<bool> CambiarContrasena(Usuario usuario);
        Task<bool> CambiarPerfil(Usuario usuario);
        Task<bool> ValidarContrasena(Usuario usuario);
    }
}
