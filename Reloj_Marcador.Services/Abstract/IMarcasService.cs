using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IMarcasService
    {
        Task<IEnumerable<(string Id_Area, string Nombre_Area)>> GetAllAreaByID(string id);
        Task<(bool Resultado, string Mensaje)> ValidateUser(Marcas marca);

    }
}
