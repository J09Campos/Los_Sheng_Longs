using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Reloj_Marcador.Services
{
    public class AreaServices : IAreaService
    {
        private readonly AreaRepository _areaRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AreaServices(
            AreaRepository areaRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _areaRepository = areaRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IEnumerable<Area>> GetAllAsync()
        {
            return _areaRepository.GetAllAsync();
        }

        public Task<Area?> GetByIdAsync(string id_area)
        {
            return _areaRepository.GetByIdAsync(id_area);
        }

        public async Task<(bool Resultado, string Mensaje)> InsertAsync(Area area)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(area.ID_Area))
                return (false, "El identificador del área no debe ser nulo.");

            if (string.IsNullOrWhiteSpace(area.Jefe))
                return (false, "El Jefe del área no debe ser nulo.");

            if (string.IsNullOrWhiteSpace(area.Nombre_Area))
                return (false, "El nombre del área no debe ser nulo.");

            if (area.Nombre_Area.Length > 40)
                return (false, "El nombre del área no debe ser mayor a 40 caracteres.");

            var regex = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
            if (!regex.IsMatch(area.Nombre_Area))
                return (false, "El nombre del área solo debe tener letras y espacios.");

            try
            {
                var resultado = await _areaRepository.CRUDAsync(area, 1);

                if (resultado.Resultado)
                {
                    string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                    var datosBitacora = new
                    {
                        area.ID_Area,
                        area.Nombre_Area,
                        area.Jefe
                    };

                    await _bitacoraService.RegistrarAsync(usuario, "Se creó un área", datosBitacora);
                }

                return resultado;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<Jefe>> GetJefesAsync()
        {
            return await _areaRepository.GetJefesAsync();
        }

        public async Task<(bool Resultado, string Mensaje)> UpdateAsync(Area area)
        {
            if (string.IsNullOrWhiteSpace(area.ID_Area))
                return (false, "El identificador del área no debe ser nulo.");

            if (string.IsNullOrWhiteSpace(area.Jefe))
                return (false, "El Jefe del área no debe ser nulo.");

            if (string.IsNullOrWhiteSpace(area.Nombre_Area))
                return (false, "El nombre del área no debe ser nulo.");

            if (area.Nombre_Area.Length > 40)
                return (false, "El nombre del área no debe ser mayor a 40 caracteres.");

            var regex = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
            if (!regex.IsMatch(area.Nombre_Area))
                return (false, "El nombre del área solo debe tener letras y espacios.");

            try
            {
                // Obtener estado anterior
                var areaAnterior = await _areaRepository.GetByIdAsync(area.ID_Area);

                var resultado = await _areaRepository.CRUDAsync(area, 2);

                if (resultado.Resultado && areaAnterior != null)
                {
                    var datosBitacora = new
                    {
                        Antes = new { areaAnterior.ID_Area, areaAnterior.Nombre_Area, areaAnterior.Jefe },
                        Ahora = new { area.ID_Area, area.Nombre_Area, area.Jefe }
                    };

                    string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                    await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un área", datosBitacora);
                }

                return resultado;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Resultado, string Mensaje)> DeleteAsync(Area area)
        {
            try
            {
                // Obtener antes de eliminar
                var areaEliminada = await _areaRepository.GetByIdAsync(area.ID_Area);

                var resultado = await _areaRepository.CRUDAsync(area, 3);

                if (resultado.Resultado && areaEliminada != null)
                {
                    var datosBitacora = new
                    {
                        areaEliminada.ID_Area,
                        areaEliminada.Nombre_Area,
                        areaEliminada.Jefe
                    };
                    string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                    await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un área", datosBitacora);
                }

                return resultado;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

