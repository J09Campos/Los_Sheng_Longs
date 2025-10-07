using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
    public class DetalleHorario
    {
        public int Id_Detalle { get; set; }
        public int Id_Horario { get; set; }
        public string? Dia { get; set; }              
        public int Hora_Ingreso { get; set; }
        public int Minuto_Ingreso { get; set; }
        public int Hora_Salida { get; set; }
        public int Minuto_Salida { get; set; }
    }
}
