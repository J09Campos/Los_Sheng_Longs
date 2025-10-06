using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services
{
    public class MotivoService : IMotivoService
    {
        private readonly MotivoRepository _motivoRepository;

        public MotivoService(MotivoRepository motivoRepository)
        {
            _motivoRepository = motivoRepository;
        }

        public async Task<IEnumerable<Motivo>> GetAllAsync()
        {
            return await _motivoRepository.GetAllAsync();
        }

        public async Task<Motivo?> GetByIdAsync(int id)
        {
            return await _motivoRepository.GetByIdAsync(id);
        }

        public async Task<int> InsertAsync(Motivo motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo.Nombre_Motivo))
                throw new ArgumentException("El nombre del motivo es requerido.");

            return await _motivoRepository.InsertAsync(motivo);
        }

        public async Task<int> UpdateAsync(Motivo motivo)
        {
            

            return await _motivoRepository.UpdateAsync(motivo);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var enUso = await _motivoRepository.EstaEnUsoAsync(id);
            

            return await _motivoRepository.DeleteAsync(id);
        }
    }
}
