using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services
{
    public class AreaServices : IAreaService
    {
        private readonly AreaRepository _areaRepository;

        public AreaServices(AreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
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
                return await _areaRepository.CRUDAsync(area, 1);
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
                return await _areaRepository.CRUDAsync(area, 2);
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
            return await _areaRepository.CRUDAsync(area, 3);
        }
    }
}
