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
    public class InconsisteciasService : IInconsistenciasService
    {
        private readonly InconsistenciasRepository _inconsisteciasRepository;

        public InconsisteciasService(InconsistenciasRepository inconsisteciasRepository)
        {
            _inconsisteciasRepository = inconsisteciasRepository;
        }
        public async Task<Entities.Inconsistencias?> GetByIdAsync(string id)
        {
            return await _inconsisteciasRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Entities.Inconsistencias>> GetAllAsync()
        {
            return await _inconsisteciasRepository.GetAllAsync();
        }
        public async Task<(bool Resultado, string Mensaje)> CRUDAsync(Entities.Inconsistencias inconsistencia, string accion)
        {
            if (ValidarInconsistencias(inconsistencia, accion))
            {
                return await _inconsisteciasRepository.CRUDAsync(inconsistencia, accion);
            }
            else
            {
                return (false, inconsistencia.Mensaje);
            }
        }
        //Validaciones de entrada de datos

        private bool ValidarInconsistencias(Entities.Inconsistencias inconsistencia,string accion)
        {
            if (accion == "Crear" || accion == "Actualizar")
            {
                if (inconsistencia.Nombre_Inconsistencia.Length > 40)
                {
                    inconsistencia.Mensaje = "El nombre de la inconsistencia no puede ser mayor a 40 caracteres.";
                    return false;

                }
                if (!Regex.IsMatch(inconsistencia.Nombre_Inconsistencia, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                {
                    inconsistencia.Mensaje = "El nombre de la inconsistencia solo puede contener letras y espacios.";
                    return false;
                }
            }
            return true;
        }
    }
}
