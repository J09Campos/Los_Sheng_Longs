using Dapper;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class InconsistenciasRepository
    {
             private readonly IDbConnectionFactory _dbConnectionFactory;

            public InconsistenciasRepository(IDbConnectionFactory dbConnectionFactory)
            {
                _dbConnectionFactory = dbConnectionFactory;
            }

            public IDbConnection CreateConnection()
            {
                return _dbConnectionFactory.CreateConnection();
            }

            /// <summary>
            /// Lista todas las inconsistencias, o una en particular si se pasa el parámetro opcional.
            /// </summary>
            public async Task<IEnumerable<Inconsistencias>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parametros = new DynamicParameters();
            parametros.Add("p_Id_Inconsistencia", dbType: DbType.Int32, direction: ParameterDirection.Input, value: DBNull.Value);

            var lista = await connection.QueryAsync<Inconsistencias>(
                "SP_Listar_Inconsistencias",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return lista;
        }

        /// <summary>
        /// Obtiene una inconsistencia por su ID.
        /// </summary>
        public async Task<Inconsistencias?> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parametros = new DynamicParameters();
            parametros.Add("p_Id_Inconsistencia", id, DbType.Int32, ParameterDirection.Input);

            var lista = await connection.QueryAsync<Inconsistencias>(
                "SP_Listar_Inconsistencias",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return lista.FirstOrDefault();
        }

        /// <summary>
        /// Realiza una operación CRUD sobre las inconsistencias (Crear, Actualizar, Eliminar).
        /// </summary>
        /// <param name="inconsistencia">Objeto inconsistencia.</param>
        /// <param name="accion">Acción a ejecutar: "INSERT", "UPDATE" o "DELETE".</param>
        public async Task<(bool Resultado, string Mensaje)> CRUDAsync(Inconsistencias inconsistencia, string accion)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parametros = new DynamicParameters();
            parametros.Add("p_Id_Inconsistencia", inconsistencia.ID_Inconsistencia, DbType.Int32, ParameterDirection.Input);
            parametros.Add("p_Tipo_Inconsistencia", inconsistencia.Tipo_Inconsistencia, DbType.String, ParameterDirection.Input);
            parametros.Add("p_Detalle", inconsistencia.Detalle, DbType.String, ParameterDirection.Input);
            parametros.Add("p_Estado", inconsistencia.Estado, DbType.String, ParameterDirection.Input);
            parametros.Add("p_Accion", accion, DbType.String, ParameterDirection.Input);

            // Parámetros de salida
            parametros.Add("p_Mensaje", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
            parametros.Add("p_Resultado", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "SP_Crud_Inconsistencia",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            string mensaje = parametros.Get<string>("p_Mensaje");
            bool resultado = parametros.Get<bool>("p_Resultado");

            return (resultado, mensaje);
        }
    }
}
