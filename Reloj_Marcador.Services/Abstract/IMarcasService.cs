using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IMarcasService
    {
        Task<IEnumerable<string>> GetAllAreaByID(string id);


    }
}
