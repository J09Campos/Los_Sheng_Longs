using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Reloj_Marcador.Entities;


namespace Reloj_Marcador.Repository
{
    public class HorarioRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public HorarioRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Horario, ID_Funcionario, ID_Area, Descripcion, Fecha_Creacion 
                    FROM horario";
            return await connection.QueryAsync<Horario>(sql);
        }

        public async Task<Horario?> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Horario, ID_Funcionario, ID_Area, Descripcion, Fecha_Creacion 
                    FROM horario 
                    WHERE ID_Horario = @Id";
            return await connection.QuerySingleOrDefaultAsync<Horario>(sql, new { Id = id });
        }

        public async Task<int> InsertAsync(Horario horario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"INSERT INTO horario (ID_Funcionario, ID_Area, Descripcion)
                    VALUES (@ID_Funcionario, @ID_Area, @Descripcion)";
            return await connection.ExecuteAsync(sql, horario);
        }

        public async Task<int> UpdateAsync(Horario horario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"UPDATE horario 
                    SET ID_Funcionario = @ID_Funcionario,
                        ID_Area = @ID_Area,
                        Descripcion = @Descripcion
                    WHERE ID_Horario = @ID_Horario";
            return await connection.ExecuteAsync(sql, horario);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"DELETE FROM horario WHERE ID_Horario = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Horario>> GetByFuncionarioAsync(string idFuncionario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Horario, ID_Funcionario, ID_Area, Descripcion, Fecha_Creacion 
                    FROM horario 
                    WHERE ID_Funcionario = @IdFuncionario";
            return await connection.QueryAsync<Horario>(sql, new { IdFuncionario = idFuncionario });
        }


    }
}