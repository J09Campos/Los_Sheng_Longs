using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
    public class BitacoraProc1
    {
        public int ID_Bitacora { get; set; }
        public DateTime Fecha_Ejecucion { get; set; }
        public string Rango_Fechas { get; set; } = string.Empty;
        public int Total_Generadas { get; set; }
        public string Usuario_Ejecuto { get; set; } = string.Empty;
    }
}
