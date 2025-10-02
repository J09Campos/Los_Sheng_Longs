using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloj_Marcador.Entities;
using Dapper;

namespace Reloj_Marcador.Repository
{
    public class FuncionariosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public FuncionariosRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Funcionarios_Usuarios>> ListarAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Funcionarios_Usuarios>(
                    "sp_FuncionariosCRUD",
                    new
                    {
                        p_accion = "LISTAR",
                        p_TipoIdentificacion = (string?)null,
                        p_Identificacion = (string?)null,
                        p_Nombre = (string?)null,
                        p_Apellido = (string?)null,
                        p_Correo = (string?)null,
                        p_Contrasena = (string?)null,
                        p_ID_Rol = (string?)null,
                        p_Estado = (string?)null
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<Funcionarios_Usuarios?> ObtenerPorIdAsync(string identificacion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Funcionarios_Usuarios>(
                    "sp_FuncionariosCRUD",
                    new
                    {
                        p_accion = "OBTENER_POR_ID",
                        p_TipoIdentificacion = (string?)null,
                        p_Identificacion = identificacion,
                        p_Nombre = (string?)null,
                        p_Apellido = (string?)null,
                        p_Correo = (string?)null,
                        p_Contrasena = (string?)null,
                        p_ID_Rol = (string?)null,
                        p_Estado = (string?)null
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> CrearAsync(Funcionarios_Usuarios funcionario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_FuncionariosCRUD",
                    new
                    {
                        p_accion = "CREAR",
                        p_TipoIdentificacion = funcionario.TipoIdentificacion,
                        p_Identificacion = funcionario.Identificacion,
                        p_Nombre = funcionario.Nombre,
                        p_Apellido = funcionario.Apellido,
                        p_Correo = funcionario.Correo,
                        p_Contrasena = funcionario.Contrasena,
                        p_ID_Rol = funcionario.ID_Rol,
                        p_Estado = funcionario.Estado
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> ActualizarAsync(Funcionarios_Usuarios funcionario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_FuncionariosCRUD",
                    new
                    {
                        p_accion = "ACTUALIZAR",
                        p_TipoIdentificacion = funcionario.TipoIdentificacion,
                        p_Identificacion = funcionario.Identificacion,
                        p_Nombre = funcionario.Nombre,
                        p_Apellido = funcionario.Apellido,
                        p_Correo = funcionario.Correo,
                        p_Contrasena = funcionario.Contrasena, // si es null, SP conserva la anterior
                        p_ID_Rol = funcionario.ID_Rol,
                        p_Estado = funcionario.Estado
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> EliminarAsync(string identificacion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteAsync(
                    "sp_FuncionariosCRUD",
                    new
                    {
                        p_accion = "ELIMINAR",
                        p_TipoIdentificacion = (string?)null,
                        p_Identificacion = identificacion,
                        p_Nombre = (string?)null,
                        p_Apellido = (string?)null,
                        p_Correo = (string?)null,
                        p_Contrasena = (string?)null,
                        p_ID_Rol = (string?)null,
                        p_Estado = (string?)null
                    },
                    commandType: CommandType.StoredProcedure
                );
            }

        }
    }
}
