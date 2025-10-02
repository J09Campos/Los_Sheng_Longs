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
    public class MarcasRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;
        public MarcasRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task<IEnumerable<string>> GetAllAreaByID(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();
                parametros.Add("p_Identificacion", id , DbType.String,ParameterDirection.Input);
                var lista = connection.QueryAsync<string>(
                    "SP_Listar_Areas_CBO",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return lista;
            }
        }


    }
}
