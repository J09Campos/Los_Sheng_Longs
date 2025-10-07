using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IRolesService
    {
        Task<IEnumerable<Rol>> ListarAsync();
        Task<Rol?> ObtenerPorIdAsync(string id);
        Task<int> CrearAsync(Rol rol);
        Task<int> ActualizarAsync(Rol rol);
        Task<int> EliminarAsync(string id);
    }
}
