using Dapper;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class DetalleHorarioRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DetalleHorarioRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // Listar detalles de un horario
        public async Task<IEnumerable<DetalleHorario>> GetByHorarioAsync(int idHorario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Detalle, ID_Horario, Dia, Hora_Ingreso, Minuto_Ingreso, Hora_Salida, Minuto_Salida
                        FROM detalle_horario 
                        WHERE ID_Horario = @IdHorario";
            return await connection.QueryAsync<DetalleHorario>(sql, new { IdHorario = idHorario });
        }

        // Obtener un detalle por ID
        public async Task<DetalleHorario?> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT ID_Detalle, ID_Horario, Dia, Hora_Ingreso, Minuto_Ingreso, Hora_Salida, Minuto_Salida
                        FROM detalle_horario 
                        WHERE ID_Detalle = @Id";
            return await connection.QuerySingleOrDefaultAsync<DetalleHorario>(sql, new { Id = id });
        }

        // Insertar un detalle de horario
        public async Task<int> InsertAsync(DetalleHorario detalle)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"INSERT INTO detalle_horario (ID_Horario, Dia, Hora_Ingreso, Minuto_Ingreso, Hora_Salida, Minuto_Salida)
                        VALUES (@ID_Horario, @Dia, @Hora_Ingreso, @Minuto_Ingreso, @Hora_Salida, @Minuto_Salida)";
            return await connection.ExecuteAsync(sql, detalle);
        }

        // Actualizar un detalle de horario
        public async Task<int> UpdateAsync(DetalleHorario detalle)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"UPDATE detalle_horario 
                        SET Dia = @Dia,
                            Hora_Ingreso = @Hora_Ingreso,
                            Minuto_Ingreso = @Minuto_Ingreso,
                            Hora_Salida = @Hora_Salida,
                            Minuto_Salida = @Minuto_Salida
                        WHERE ID_Detalle = @ID_Detalle";
            return await connection.ExecuteAsync(sql, detalle);
        }

        // Eliminar un detalle de horario
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"DELETE FROM detalle_horario WHERE ID_Detalle = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
