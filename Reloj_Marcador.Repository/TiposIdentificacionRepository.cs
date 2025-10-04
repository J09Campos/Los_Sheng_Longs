using Dapper;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class TiposIdentificacionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public TiposIdentificacionRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<TipoIdentificacion>> ListarAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<TipoIdentificacion>(
                    "sp_TiposIdentificacionCRUD",
                    new { p_accion = "LISTAR", p_ID_TipoIdentificacion = (string?)null, p_Nombre_TipoIdentificacion = (string?)null },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<TipoIdentificacion?> ObtenerPorIdAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<TipoIdentificacion>(
                    "sp_TiposIdentificacionCRUD",
                    new { p_accion = "OBTENER_POR_ID", p_ID_TipoIdentificacion = id, p_Nombre_TipoIdentificacion = (string?)null },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> CrearAsync(TipoIdentificacion tipo)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_TiposIdentificacionCRUD",
                    new { p_accion = "CREAR", p_ID_TipoIdentificacion = tipo.ID_TipoIdentificacion, p_Nombre_TipoIdentificacion = tipo.Nombre_TipoIdentificacion },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> ActualizarAsync(TipoIdentificacion tipo)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_TiposIdentificacionCRUD",
                    new { p_accion = "ACTUALIZAR", p_ID_TipoIdentificacion = tipo.ID_TipoIdentificacion, p_Nombre_TipoIdentificacion = tipo.Nombre_TipoIdentificacion },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> EliminarAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_TiposIdentificacionCRUD",
                    new { p_accion = "ELIMINAR", p_ID_TipoIdentificacion = id, p_Nombre_TipoIdentificacion = (string?)null },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}

