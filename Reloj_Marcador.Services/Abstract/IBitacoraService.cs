using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IBitacoraService
    {
        Task RegistrarAsync(string usuario, string accion ,object descripcion);
    }
}
