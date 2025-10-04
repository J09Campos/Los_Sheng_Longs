using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System.Text.RegularExpressions;

namespace Reloj_Marcador.Services
{
    public class TiposIdentificacionService : ITiposIdentificacionService
    {
        private readonly TiposIdentificacionRepository _repo;

        public TiposIdentificacionService(TiposIdentificacionRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<TipoIdentificacion>> ListarAsync()
            => _repo.ListarAsync();

        public Task<TipoIdentificacion?> ObtenerPorIdAsync(string id)
            => _repo.ObtenerPorIdAsync(id);

        public async Task<int> CrearAsync(TipoIdentificacion tipo)
        {
            ValidarTipo(tipo);
            return await _repo.CrearAsync(tipo);
        }

        public async Task<int> ActualizarAsync(TipoIdentificacion tipo)
        {
            ValidarTipo(tipo);
            return await _repo.ActualizarAsync(tipo);
        }

        public Task<int> EliminarAsync(string id)
            => _repo.EliminarAsync(id);

        private void ValidarTipo(TipoIdentificacion tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo.ID_TipoIdentificacion))
                throw new ArgumentException("El ID del tipo de identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(tipo.Nombre_TipoIdentificacion))
                throw new ArgumentException("El nombre del tipo de identificación es obligatorio.");

            if (tipo.Nombre_TipoIdentificacion.Length > 40)
                throw new ArgumentException("El nombre del tipo de identificación no puede tener más de 40 caracteres.");

            if (!Regex.IsMatch(tipo.Nombre_TipoIdentificacion, @"^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"))
                throw new ArgumentException("El nombre del tipo de identificación solo puede contener letras y espacios.");
        }
    }
}

