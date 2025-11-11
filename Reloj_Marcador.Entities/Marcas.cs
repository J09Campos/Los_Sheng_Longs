using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
    public class Marcas
    {
        public string? Identificacion { get; set; }
        public string? Contrasena { get; set; }
        public string? Id_Area { get; set; }
        public string? Descripcion { get; set; }
        public string? Tipo_Marca { get; set; }
        public TimeOnly? Hora_Servidor { get; set; }
        public DateOnly? Fecha { get; set; }
        public string? IP_Registro { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string? Mensaje { get; set; }
        public bool? Resultado { get; set; }
    }
}

