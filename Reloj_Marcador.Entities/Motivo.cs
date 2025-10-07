using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Entities
{
    public class Motivo
    {
        public int ID_Motivo { get; set; }

        [Required(ErrorMessage = "El Nombre del Motivo es obligatorio.")]
        [StringLength(40)]
        public string Nombre_Motivo { get; set; } = string.Empty;
    }


}
