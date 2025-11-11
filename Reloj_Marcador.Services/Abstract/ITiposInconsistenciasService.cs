using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface ITiposInconsistenciasService
    {
        Task<IEnumerable<Entities.Tipos_Inconsistencia>> GetAllAsync();
        Task<Entities.Tipos_Inconsistencia?> GetByIdAsync(string id);
        Task<(bool Resultado, string Mensaje)> CRUDAsync(Entities.Tipos_Inconsistencia inconsistencia, string accion);
    }
}
