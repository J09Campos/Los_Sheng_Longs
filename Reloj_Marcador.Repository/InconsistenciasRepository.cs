using Dapper;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Inconsistencias>> GetAllAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection()) {
                var parametros = new DynamicParameters();
                parametros.Add("p_Id_Inconsistencia", dbType: DbType.Int32, direction: ParameterDirection.Input, value: DBNull.Value);

                var lista = await connection.QueryAsync<Inconsistencias>(
                    "SP_Listar_Inconsistencias",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return lista;
            }
        }
        public async Task<Inconsistencias?> GetByIdAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();

                parametros.Add("p_Id_Inconsistencia", id, DbType.Int32, ParameterDirection.Input);

                var lista = await connection.QueryAsync<Inconsistencias>(
                 "SP_Listar_Inconsistencias",
                 parametros,
                 commandType: CommandType.StoredProcedure
                 );

                // Retornar primer registro o null
                return lista.FirstOrDefault();
            }
        }
        public async Task<(bool Resultado,string Mensaje)> CRUDAsync(Inconsistencias inconsistencia , string accion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();

                parametros.Add("p_Id_Inconsistencia", inconsistencia.Id_Inconsistencia, DbType.Int32, ParameterDirection.Input);
                parametros.Add("p_Nombre_Inconsistencia", inconsistencia.Nombre_Inconsistencia, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Accion", accion, DbType.String, ParameterDirection.Input);

                parametros.Add("p_Mensaje", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parametros.Add("p_Resultado", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "SP_Crud_Tipo_Inconsistencia",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                string mensaje = parametros.Get<string>("p_Mensaje");
                byte resultadoByte = parametros.Get<byte>("p_Resultado");
                bool resultado = resultadoByte == 1;


                return (Convert.ToBoolean(resultado), mensaje);
            }
        }
    }
}
