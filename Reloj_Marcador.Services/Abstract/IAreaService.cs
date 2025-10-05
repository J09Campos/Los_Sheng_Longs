using Reloj_Marcador.Entities;


namespace Reloj_Marcador.Services.Abstract
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(string id_area);
        Task<(bool Resultado, string Mensaje)> InsertAsync(Area area);
        Task<IEnumerable<Jefe>> GetJefesAsync();
        Task<(bool Resultado, string Mensaje)> UpdateAsync(Area area);
        Task<(bool Resultado, string Mensaje)> DeleteAsync(Area area);
    }
}
