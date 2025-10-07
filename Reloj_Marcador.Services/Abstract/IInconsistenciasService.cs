using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IInconsistenciasService
    {

        Task<IEnumerable<Entities.Inconsistencias>> GetAllAsync();
        Task<Entities.Inconsistencias?> GetByIdAsync(string id);
        Task<(bool Resultado, string Mensaje)> CRUDAsync(Entities.Inconsistencias inconsistencia, string accion);
    }
}
