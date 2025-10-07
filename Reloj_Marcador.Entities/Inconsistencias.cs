using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
    public class Inconsistencias
    {

        public int? Id_Inconsistencia { get; set; }
        public string Nombre_Inconsistencia { get; set; }
        public string? Mensaje { get; set; }
        public bool? Resultado { get; set; }
    }
}
