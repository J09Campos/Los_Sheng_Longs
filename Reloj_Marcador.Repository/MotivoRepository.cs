using Dapper;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Repository
{
    public class MotivoRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MotivoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

   
        public async Task<IEnumerable<Motivo>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT ID_Motivo, Nombre_Motivo FROM motivo_ausencia";
            var result = await connection.QueryAsync<Motivo>(sql);
            return result ?? new List<Motivo>();
        }

    
        public async Task<Motivo?> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT ID_Motivo, Nombre_Motivo FROM motivo_ausencia WHERE ID_Motivo = @Id";
            return await connection.QuerySingleOrDefaultAsync<Motivo>(sql, new { Id = id });
        }

     
        public async Task<int> InsertAsync(Motivo motivo)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"INSERT INTO motivo_ausencia (Nombre_Motivo) VALUES (@Nombre_Motivo);SELECT LAST_INSERT_ID();"; 
            return await connection.ExecuteScalarAsync<int>(sql, motivo);
        }

        public async Task<int> UpdateAsync(Motivo motivo)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "UPDATE motivo_ausencia SET Nombre_Motivo = @Nombre_Motivo WHERE ID_Motivo = @ID_Motivo";
            return await connection.ExecuteAsync(sql, motivo);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "DELETE FROM motivo_ausencia WHERE ID_Motivo = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> EstaEnUsoAsync(int idMotivo)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = "SELECT COUNT(*) FROM ausencia WHERE ID_Motivo = @IdMotivo";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { IdMotivo = idMotivo });
            return count > 0;
        }
    }
}
