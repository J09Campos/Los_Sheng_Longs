using Microsoft.AspNetCore.Http;
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
    public class InconsisteciasService : IInconsistenciasService
    {
        private readonly InconsistenciasRepository _inconsisteciasRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InconsisteciasService(InconsistenciasRepository inconsisteciasRepository, IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _inconsisteciasRepository = inconsisteciasRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
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
            try
            {
                if (ValidarInconsistencias(inconsistencia, accion))
                {
                    var resultado = await _inconsisteciasRepository.CRUDAsync(inconsistencia, accion);
                    if (resultado.Resultado)
                    {
                        string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                        var datosBitacora = new
                        {
                            inconsistencia.Id_Inconsistencia,
                            inconsistencia.Nombre_Inconsistencia,
                            accion,
                        };

                        await _bitacoraService.RegistrarAsync(usuario, "Realizo cambios de tipos de inconsistencias", datosBitacora);
                    }

                    return resultado;
                }
                else
                {
                    return (false, inconsistencia.Mensaje);
                }

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
        //Validaciones de entrada de datos

        private bool ValidarInconsistencias(Entities.Inconsistencias inconsistencia, string accion)
        {
            if (accion == "Crear" || accion == "Actualizar")
            {
                if (string.IsNullOrEmpty(inconsistencia.Nombre_Inconsistencia))
                {
                    inconsistencia.Mensaje = "El nombre de la inconsistencia es obligatorio.";
                    return false;
                }
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
