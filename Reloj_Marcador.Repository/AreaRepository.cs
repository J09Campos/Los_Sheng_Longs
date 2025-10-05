using Dapper;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class AreaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AreaRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Area>("SP_Listar_Areas", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Area?> GetByIdAsync(string id_area)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Area>("SP_Listar_Areas_ID", new { p_id_area = id_area }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Jefe>> GetJefesAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Jefe>("SP_Listar_Funcionarios_Areas", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<(bool Resultado, string Mensaje)> CRUDAsync(Area area, int accion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();

                parametros.Add("p_ID_Area", area.ID_Area, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Nombre_Area", area.Nombre_Area, DbType.String, ParameterDirection.Input);
                parametros.Add("p_ID_Jefe", area.Jefe, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Accion", accion, DbType.Int32, ParameterDirection.Input);
                parametros.Add("p_Mensaje", area.Mensaje, dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                try
                {
                    await connection.ExecuteAsync(
                        "SP_Crud_Areas",
                        parametros,
                        commandType: CommandType.StoredProcedure
                    );

                    string mensaje = parametros.Get<string>("p_Mensaje");

                    return (true, mensaje);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

    }
}
