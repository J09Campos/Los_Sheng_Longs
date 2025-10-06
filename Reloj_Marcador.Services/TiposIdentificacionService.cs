using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Reloj_Marcador.Services
{
    public class TiposIdentificacionService : ITiposIdentificacionService
    {
        private readonly TiposIdentificacionRepository _repo;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TiposIdentificacionService(
            TiposIdentificacionRepository repo,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IEnumerable<TipoIdentificacion>> ListarAsync()
            => _repo.ListarAsync();

        public Task<TipoIdentificacion?> ObtenerPorIdAsync(string id)
            => _repo.ObtenerPorIdAsync(id);

        public async Task<int> CrearAsync(TipoIdentificacion tipo)
        {
            ValidarTipo(tipo);
            var resultado = await _repo.CrearAsync(tipo);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se creó un Tipo de Identificación", new
                {
                    tipo.ID_TipoIdentificacion,
                    tipo.Nombre_TipoIdentificacion
                });
            }

            return resultado;
        }

        public async Task<int> ActualizarAsync(TipoIdentificacion tipo)
        {
            ValidarTipo(tipo);

            var tipoAnterior = await _repo.ObtenerPorIdAsync(tipo.ID_TipoIdentificacion);
            if (tipoAnterior == null)
                throw new ArgumentException("El tipo de identificación no existe.");

            var resultado = await _repo.ActualizarAsync(tipo);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un Tipo de Identificación", new
                {
                    Antes = new { tipoAnterior.ID_TipoIdentificacion, tipoAnterior.Nombre_TipoIdentificacion },
                    Ahora = new { tipo.ID_TipoIdentificacion, tipo.Nombre_TipoIdentificacion }
                });
            }

            return resultado;
        }

        public async Task<int> EliminarAsync(string id)
        {
            var tipoAnterior = await _repo.ObtenerPorIdAsync(id);
            if (tipoAnterior == null)
                throw new ArgumentException("El tipo de identificación no existe.");

            var resultado = await _repo.EliminarAsync(id);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un Tipo de Identificación", new
                {
                    tipoAnterior.ID_TipoIdentificacion,
                    tipoAnterior.Nombre_TipoIdentificacion
                });
            }

            return resultado;
        }

        private void ValidarTipo(TipoIdentificacion tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo.ID_TipoIdentificacion))
                throw new ArgumentException("El ID del tipo de identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(tipo.Nombre_TipoIdentificacion))
                throw new ArgumentException("El nombre del tipo de identificación es obligatorio.");

            if (tipo.Nombre_TipoIdentificacion.Length > 40)
                throw new ArgumentException("El nombre del tipo de identificación no debe ser mayor a 40 caracteres.");

            if (!Regex.IsMatch(tipo.Nombre_TipoIdentificacion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("El nombre del tipo de identificación solo debe tener letras y espacios.");
        }
    }
}


