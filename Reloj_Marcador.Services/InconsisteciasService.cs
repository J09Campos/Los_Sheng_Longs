using Microsoft.AspNetCore.Http;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;

namespace Reloj_Marcador.Services
{
    public class InconsistenciasService : IInconsistenciasService
    {
        private readonly InconsistenciasRepository _inconsistenciasRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InconsistenciasService(
            InconsistenciasRepository inconsistenciasRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _inconsistenciasRepository = inconsistenciasRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        // ✅ Obtener inconsistencia por ID
        public async Task<Inconsistencias?> GetByIdAsync(int id)
        {
            return await _inconsistenciasRepository.GetByIdAsync(id);
        }

        // ✅ Listar todas las inconsistencias
        public async Task<IEnumerable<Inconsistencias>> GetAllAsync()
        {
            return await _inconsistenciasRepository.GetAllAsync();
        }

        // ✅ Crear / actualizar / eliminar
        public async Task<(bool Resultado, string Mensaje)> CRUDAsync(Inconsistencias inconsistencias, string accion)
        {
            try
            {
                if (ValidarInconsistencia(inconsistencias, accion))
                {
                    var resultado = await _inconsistenciasRepository.CRUDAsync(inconsistencias, accion);

                    if (resultado.Resultado)
                    {
                        string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                        var datosBitacora = new
                        {
                            inconsistencias.ID_Inconsistencia,
                            inconsistencias.Tipo_Inconsistencia,
                            Accion = accion
                        };

                        await _bitacoraService.RegistrarAsync(
                            usuario,
                            "Actualización en tabla de inconsistencias",
                            datosBitacora
                        );
                    }

                    return resultado;
                }
                else
                {
                    return (false, inconsistencias.Mensaje ?? "Validación fallida.");
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

        // ✅ Validación de datos
        private bool ValidarInconsistencia(Inconsistencias inconsistencia, string accion)
        {
            if (accion.Equals("INSERT", StringComparison.OrdinalIgnoreCase) ||
                accion.Equals("UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(inconsistencia.Tipo_Inconsistencia))
                {
                    inconsistencia.Mensaje = "El tipo de inconsistencia es obligatorio.";
                    return false;
                }

                if (inconsistencia.Tipo_Inconsistencia.Length > 100)
                {
                    inconsistencia.Mensaje = "El tipo de inconsistencia no puede superar los 100 caracteres.";
                    return false;
                }

                if (!Regex.IsMatch(inconsistencia.Tipo_Inconsistencia, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                {
                    inconsistencia.Mensaje = "El tipo de inconsistencia solo puede contener letras y espacios.";
                    return false;
                }
            }

            return true;
        }

 
        public async Task<IEnumerable<Inconsistencias>> ListarInconsistenciasAsync(
            DateTime? inicio, DateTime? fin, string? area, string? funcionario)
        {
            using var connection = _inconsistenciasRepository.CreateConnection();

            string sql = @"
                SELECT *
                FROM inconsistencias
                WHERE (@inicio IS NULL OR Fecha >= @inicio)
                  AND (@fin IS NULL OR Fecha <= @fin)
                  AND (@area IS NULL OR ID_Area = @area)
                  AND (@funcionario IS NULL OR Identificacion = @funcionario)
                ORDER BY Fecha DESC;";

            var result = await connection.QueryAsync<Inconsistencias>(sql, new
            {
                inicio,
                fin,
                area,
                funcionario
            });

            return result;
        }
    }
}
