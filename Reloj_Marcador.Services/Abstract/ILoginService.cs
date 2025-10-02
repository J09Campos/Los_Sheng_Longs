using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface ILoginService
    {

        Task<(string Mensaje, string? NombreCompleto)> LoginAsync(string usuario, string contrasena);


    }
}
