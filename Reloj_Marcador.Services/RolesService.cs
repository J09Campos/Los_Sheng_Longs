using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class RolesService : IRolesService
    {
        private readonly RolesRepository _rolesRepository;

        public RolesService(RolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public Task<IEnumerable<Rol>> ListarAsync() => _rolesRepository.ListarAsync();
        public Task<Rol?> ObtenerPorIdAsync(string id) => _rolesRepository.ObtenerPorIdAsync(id);

        public async Task<int> CrearAsync(Rol rol)
        {
            ValidarRol(rol); 
            return await _rolesRepository.CrearAsync(rol);
        }

        public async Task<int> ActualizarAsync(Rol rol)
        {
            ValidarRol(rol);
            return await _rolesRepository.ActualizarAsync(rol);
        }

        public Task<int> EliminarAsync(string id) => _rolesRepository.EliminarAsync(id);

        private void ValidarRol(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.ID_Rol))
                throw new ArgumentException("El ID del rol es obligatorio.");

            if (string.IsNullOrWhiteSpace(rol.Nombre_Rol))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            if (rol.Nombre_Rol.Length > 40)
                throw new ArgumentException("El nombre del rol no puede tener más de 40 caracteres.");

            if (!Regex.IsMatch(rol.Nombre_Rol, @"^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"))
                throw new ArgumentException("El nombre del rol solo puede contener letras y espacios.");
        }
    }
}
