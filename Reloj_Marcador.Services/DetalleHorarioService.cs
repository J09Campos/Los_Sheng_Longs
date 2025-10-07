using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Reloj_Marcador.Services
{
    public class DetalleHorarioService : IDetalleHorarioService
    {
        private readonly DetalleHorarioRepository _detalleHorarioRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetalleHorarioService(
            DetalleHorarioRepository detalleHorarioRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _detalleHorarioRepository = detalleHorarioRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
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
            var resultado = await _detalleHorarioRepository.InsertAsync(detalle);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se agregó un nuevo Detalle de Horario", new
                {
                    detalle.Id_Detalle,
                    detalle.Id_Horario,
                    detalle.Dia,
                    detalle.Hora_Ingreso,
                    detalle.Minuto_Ingreso,
                    detalle.Hora_Salida,
                    detalle.Minuto_Salida
                });
            }

            return resultado;
        }

        public async Task<int> ActualizarDetalleHorario(DetalleHorario detalle)
        {
            var detalleAnterior = await _detalleHorarioRepository.GetByIdAsync(detalle.Id_Detalle);
            if (detalleAnterior == null)
                throw new ArgumentException("El detalle de horario no existe.");

            var resultado = await _detalleHorarioRepository.UpdateAsync(detalle);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un Detalle de Horario", new
                {
                    Antes = new
                    {
                        detalleAnterior.Id_Detalle,
                        detalleAnterior.Id_Horario,
                        detalleAnterior.Dia,
                        detalleAnterior.Hora_Ingreso,
                        detalleAnterior.Minuto_Ingreso,
                        detalleAnterior.Hora_Salida,
                        detalleAnterior.Minuto_Salida
                    },
                    Ahora = new
                    {
                        detalle.Id_Detalle,
                        detalle.Id_Horario,
                        detalle.Dia,
                        detalle.Hora_Ingreso,
                        detalle.Minuto_Ingreso,
                        detalle.Hora_Salida,
                        detalle.Minuto_Salida
                    }
                });
            }

            return resultado;
        }

        public async Task<int> EliminarDetalleHorario(int id)
        {
            var detalleAnterior = await _detalleHorarioRepository.GetByIdAsync(id);
            if (detalleAnterior == null)
                throw new ArgumentException("El detalle de horario no existe.");

            var resultado = await _detalleHorarioRepository.DeleteAsync(id);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un Detalle de Horario", new
                {
                    detalleAnterior.Id_Detalle,
                    detalleAnterior.Id_Horario,
                    detalleAnterior.Dia,
                    detalleAnterior.Hora_Ingreso,
                    detalleAnterior.Minuto_Ingreso,
                    detalleAnterior.Hora_Salida,
                    detalleAnterior.Minuto_Salida
                });
            }

            return resultado;
        }
    }
}


