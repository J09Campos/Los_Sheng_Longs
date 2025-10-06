using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Reloj_Marcador.Services
{
    public class RolesService : IRolesService
    {
        private readonly RolesRepository _rolesRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolesService(
            RolesRepository rolesRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _rolesRepository = rolesRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IEnumerable<Rol>> ListarAsync() => _rolesRepository.ListarAsync();

        public Task<Rol?> ObtenerPorIdAsync(string id) => _rolesRepository.ObtenerPorIdAsync(id);

        public async Task<int> CrearAsync(Rol rol)
        {
            ValidarRol(rol);
            var resultado = await _rolesRepository.CrearAsync(rol);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se creó un Rol", new
                {
                    rol.ID_Rol,
                    rol.Nombre_Rol
                });
            }

            return resultado;
        }

        public async Task<int> ActualizarAsync(Rol rol)
        {
            ValidarRol(rol);

            var rolAnterior = await _rolesRepository.ObtenerPorIdAsync(rol.ID_Rol);
            if (rolAnterior == null)
                throw new ArgumentException("El rol no existe.");

            var resultado = await _rolesRepository.ActualizarAsync(rol);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un Rol", new
                {
                    Antes = new { rolAnterior.ID_Rol, rolAnterior.Nombre_Rol },
                    Ahora = new { rol.ID_Rol, rol.Nombre_Rol }
                });
            }

            return resultado;
        }

        public async Task<int> EliminarAsync(string id)
        {
            var rolAnterior = await _rolesRepository.ObtenerPorIdAsync(id);
            if (rolAnterior == null)
                throw new ArgumentException("El rol no existe.");

            var resultado = await _rolesRepository.EliminarAsync(id);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un Rol", new
                {
                    rolAnterior.ID_Rol,
                    rolAnterior.Nombre_Rol
                });
            }

            return resultado;
        }

        private void ValidarRol(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.ID_Rol))
                throw new ArgumentException("El ID del rol es obligatorio.");

            if (string.IsNullOrWhiteSpace(rol.Nombre_Rol))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            if (rol.Nombre_Rol.Length > 40)
                throw new ArgumentException("El nombre del rol no debe ser mayor a 40 caracteres.");

            if (!Regex.IsMatch(rol.Nombre_Rol, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("El nombre del rol solo debe tener letras y espacios.");
        }
    }
}

