using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services
{
    public class DetalleHorarioService : IDetalleHorarioService
    {
        private readonly DetalleHorarioRepository _detalleHorarioRepository;

        public DetalleHorarioService(DetalleHorarioRepository detalleHorarioRepository)
        {
            _detalleHorarioRepository = detalleHorarioRepository;
        }

        public async Task<IEnumerable<DetalleHorario>> ListarDetallesPorHorario(int idHorario)
        {
            return await _detalleHorarioRepository.GetByHorarioAsync(idHorario);
        }

        public async Task<DetalleHorario?> GetByIdAsync(int id)
        {
            return await _detalleHorarioRepository.GetByIdAsync(id);
        }

        public async Task<int> AgregarDetalleHorario(DetalleHorario detalle)
        {
            return await _detalleHorarioRepository.InsertAsync(detalle);
        }

        public async Task<int> ActualizarDetalleHorario(DetalleHorario detalle)
        {
            return await _detalleHorarioRepository.UpdateAsync(detalle);
        }

        public async Task<int> EliminarDetalleHorario(int id)
        {
            return await _detalleHorarioRepository.DeleteAsync(id);
        }
    }
}
