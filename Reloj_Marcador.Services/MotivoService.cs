using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Reloj_Marcador.Services
{
    public class MotivoService : IMotivoService
    {
        private readonly MotivoRepository _motivoRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MotivoService(
            MotivoRepository motivoRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _motivoRepository = motivoRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
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

            var resultado = await _motivoRepository.InsertAsync(motivo);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se creó un nuevo Motivo", new
                {
                    motivo.ID_Motivo,
                    motivo.Nombre_Motivo
                });
            }

            return resultado;
        }

        public async Task<int> UpdateAsync(Motivo motivo)
        {
            var motivoAnterior = await _motivoRepository.GetByIdAsync(motivo.ID_Motivo);
            if (motivoAnterior == null)
                throw new ArgumentException("El motivo no existe.");

            var resultado = await _motivoRepository.UpdateAsync(motivo);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un Motivo", new
                {
                    Antes = new
                    {
                        motivoAnterior.ID_Motivo,
                        motivoAnterior.Nombre_Motivo
                    },
                    Ahora = new
                    {
                        motivo.ID_Motivo,
                        motivo.Nombre_Motivo
                    }
                });
            }

            return resultado;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var motivoAnterior = await _motivoRepository.GetByIdAsync(id);
            if (motivoAnterior == null)
                throw new ArgumentException("El motivo no existe.");

            var enUso = await _motivoRepository.EstaEnUsoAsync(id);
            if (enUso)
                throw new InvalidOperationException("No se puede eliminar el motivo porque está en uso.");

            var resultado = await _motivoRepository.DeleteAsync(id);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un Motivo", new
                {
                    motivoAnterior.ID_Motivo,
                    motivoAnterior.Nombre_Motivo
                });
            }

            return resultado;
        }
    }
}

