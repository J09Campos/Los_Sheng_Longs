using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class Proc1Repository
    {
        private readonly string _connectionString;

        public Proc1Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");
        }

        /// <summary>
        /// Ejecuta el procedimiento almacenado PROC1 (genera inconsistencias).
        /// </summary>
        public async Task EjecutarProcesoAsync(DateTime fechaInicio, DateTime fechaFin, string? idArea = null, string? identificacion = null)
        {
            using var connection = new MySqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("p_fecha_inicio", fechaInicio);
            parameters.Add("p_fecha_fin", fechaFin);
            parameters.Add("p_id_area", idArea);
            parameters.Add("p_identificacion", identificacion);

            await connection.ExecuteAsync("sp_generar_inconsistencias", parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene la bitácora de ejecuciones del proceso PROC1.
        /// </summary>
        public async Task<IEnumerable<BitacoraProc1>> ListarBitacoraAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            const string sql = "SELECT * FROM bitacora_proc1 ORDER BY Fecha_Ejecucion DESC";
            return await connection.QueryAsync<BitacoraProc1>(sql);
        }

        /// <summary>
        /// Obtiene las inconsistencias generadas (opcionalmente filtradas por rango, área o funcionario).
        /// </summary>
        public async Task<IEnumerable<Inconsistencias>> ListarInconsistenciasAsync(DateTime? inicio = null, DateTime? fin = null, string? area = null, string? funcionario = null)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = new StringBuilder();
            sql.AppendLine("SELECT ID_Inconsistencia, Fecha, Tipo_Inconsistencia, Detalle, Estado, Identificacion, ID_Area, ID_Marca, ID_Horario");
            sql.AppendLine("FROM inconsistencias WHERE 1=1");

            var parameters = new DynamicParameters();

            if (inicio.HasValue)
            {
                sql.AppendLine("AND Fecha >= @inicio");
                parameters.Add("@inicio", inicio.Value);
            }

            if (fin.HasValue)
            {
                sql.AppendLine("AND Fecha <= @fin");
                parameters.Add("@fin", fin.Value);
            }

            if (!string.IsNullOrEmpty(area))
            {
                sql.AppendLine("AND ID_Area = @area");
                parameters.Add("@area", area);
            }

            if (!string.IsNullOrEmpty(funcionario))
            {
                sql.AppendLine("AND Identificacion = @funcionario");
                parameters.Add("@funcionario", funcionario);
            }

            sql.AppendLine("ORDER BY Fecha DESC");

            return await connection.QueryAsync<Inconsistencias>(sql.ToString(), parameters);
        }
    }
}
