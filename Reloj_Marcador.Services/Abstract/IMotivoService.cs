using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IMotivoService
    {
        Task<IEnumerable<Motivo>> GetAllAsync();
        Task<Motivo?> GetByIdAsync(int id);
        Task<int> InsertAsync(Motivo motivo);
        Task<int> UpdateAsync(Motivo motivo);
        Task<int> DeleteAsync(int id);
    }
}
