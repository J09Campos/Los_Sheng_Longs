namespace Reloj_Marcador.Entities
{
    public class MarcasReporte
    {
        public int Id_Marca { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Id_Area { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Tipo_Marca { get; set; } = string.Empty;
        public TimeSpan? Hora_Servidor { get; set; }
        public DateTime? Fecha { get; set; }
        public string? IP_Registro { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}
