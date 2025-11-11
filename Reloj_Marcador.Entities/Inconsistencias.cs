using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
        public class Inconsistencias
        {
            public int ID_Inconsistencia { get; set; }
            public DateTime Fecha { get; set; }
            public string? Tipo_Inconsistencia { get; set; }
            public string? Detalle { get; set; }
            public string? Estado { get; set; }
            public string? Identificacion { get; set; }
            public string? ID_Area { get; set; }
            public int? ID_Marca { get; set; }
            public int? ID_Horario { get; set; }

            public string Mensaje { get; set; }
            public bool? Resultado { get; set; }

            public int? Nombre_Inconsistencia { get; set; }
        }
    
}
