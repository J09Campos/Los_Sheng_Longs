namespace Reloj_Marcador.Entities
{
    public class Horario
    {
        public int ID_Horario { get; set; }
        public string ID_Funcionario { get; set; } = null!;
        public string ID_Area { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }

}
