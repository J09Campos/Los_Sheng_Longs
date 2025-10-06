using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Repository
{
    public class AreaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AreaRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // Listar todas las áreas
        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Area, Nombre_Area, ID_Jefe 
                        FROM areas";
            return await connection.QueryAsync<Area>(sql);
        }

        // Obtener un área por ID
        public async Task<Area?> GetByIdAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Area, Nombre_Area, ID_Jefe 
                        FROM areas 
                        WHERE ID_Area = @Id";
            return await connection.QuerySingleOrDefaultAsync<Area>(sql, new { Id = id });
        }

        // Insertar un área
        public async Task<int> InsertAsync(Area area)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"INSERT INTO areas (ID_Area, Nombre_Area, ID_Jefe)
                        VALUES (@ID_Area, @Nombre_Area, @ID_Jefe)";
            return await connection.ExecuteAsync(sql, area);
        }

        // Actualizar un área existente
        public async Task<int> UpdateAsync(Area area)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"UPDATE areas 
                        SET Nombre_Area = @Nombre_Area,
                            ID_Jefe = @ID_Jefe
                        WHERE ID_Area = @ID_Area";
            return await connection.ExecuteAsync(sql, area);
        }

        // Eliminar un área
        public async Task<int> DeleteAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"DELETE FROM areas WHERE ID_Area = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
