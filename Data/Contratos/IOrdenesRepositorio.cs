using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Contratos
{
    public interface IOrdenesRepositorio: IRepositorioGenerico<Orden>
    {
        Task<IEnumerable<Orden>> ObtenerTodosConDetallesAsync();
        Task<Orden> ObtenerConDetallesAsync(int id);
    }
}
