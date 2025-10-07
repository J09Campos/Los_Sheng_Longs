using Dapper;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System.Data;

namespace Reloj_Marcador.Repository
{
    public class RolesRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RolesRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Rol>> ListarAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Rol>(
                "sp_RolesCRUD",
                new { p_accion = "LISTAR", p_ID_Rol = (string?)null, p_Nombre_Rol = (string?)null },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Rol?> ObtenerPorIdAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Rol>(
                "sp_RolesCRUD",
                new { p_accion = "OBTENER_POR_ID", p_ID_Rol = id, p_Nombre_Rol = (string?)null },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CrearAsync(Rol rol)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                "sp_RolesCRUD",
                new { p_accion = "CREAR", p_ID_Rol = rol.ID_Rol, p_Nombre_Rol = rol.Nombre_Rol },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ActualizarAsync(Rol rol)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                "sp_RolesCRUD",
                new { p_accion = "ACTUALIZAR", p_ID_Rol = rol.ID_Rol, p_Nombre_Rol = rol.Nombre_Rol },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarAsync(string id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                "sp_RolesCRUD",
                new { p_accion = "ELIMINAR", p_ID_Rol = id, p_Nombre_Rol = (string?)null },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}




