using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface ITiposIdentificacionService
    {
        Task<IEnumerable<TipoIdentificacion>> ListarAsync();
        Task<TipoIdentificacion?> ObtenerPorIdAsync(string id);
        Task<int> CrearAsync(TipoIdentificacion tipo);
        Task<int> ActualizarAsync(TipoIdentificacion tipo);
        Task<int> EliminarAsync(string id);
    }
}
