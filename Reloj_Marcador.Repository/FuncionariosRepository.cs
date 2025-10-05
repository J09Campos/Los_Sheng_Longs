using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Repository
{
    public class FuncionariosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly LoginRepository _loginRepository;

        public FuncionariosRepository(IDbConnectionFactory dbConnectionFactory, LoginRepository loginRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _loginRepository = loginRepository;
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


        //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------


        public async Task<string> Crear_ContrasenaAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("NuevaContra", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "SP_Crear_Contra_Funcionario",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Recuperar el valor OUT
                return parameters.Get<string>("NuevaContra");
            }
        }

        public async Task<int> Cambiar_ContrasenaAsync(string identificacion, string contrasena)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                string contrasenaCifrada = LoginRepository.Encrypt(contrasena);

                return await connection.ExecuteAsync(
                    "SP_Cambiar_Contra_Funcionario",
                    new
                    {
                        p_ID_Funcionario = identificacion,
                        p_Nueva_Contra = contrasenaCifrada
                    },
                    commandType: CommandType.StoredProcedure
                );
            }

        }


        //private static readonly string Key = "0123456789abcdef";
        //private static readonly string IV = "abcdef0123456789";

        //// Método  para Encriptar

        //public static string Encrypt(string plainText)
        //{
        //    if (string.IsNullOrEmpty(plainText))
        //        return plainText;

        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Encoding.UTF8.GetBytes(Key);
        //        aesAlg.IV = Encoding.UTF8.GetBytes(IV);
        //        aesAlg.Padding = PaddingMode.PKCS7;

        //        using var msEncrypt = new MemoryStream();
        //        using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
        //        using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
        //        {
        //            swEncrypt.Write(plainText);
        //        }

        //        byte[] encryptedBytes = msEncrypt.ToArray();
        //        string base64 = Convert.ToBase64String(encryptedBytes);

        //        return base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
        //    }
        //}

        //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------

        public async Task<IEnumerable<Funcionarios_Areas>> ListarFuncionariosAreasAsync(string identificacion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Funcionarios_Areas>(
                    "SP_Listar_Func_Areas",
                    new { p_ID_Funcionario = identificacion },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Funcionarios_Areas>> ListarAreasAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Funcionarios_Areas>(
                    "SP_Listar_Areas_Funcionarios",
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<(bool resultado, string mensaje)> InsertarFuncionarioAreaAsync(Funcionarios_Areas funcionarioArea)
        {
            try
            {
                // Llama a un SP para insertar la relación (lo puedes crear similar al de listar)
                using (var connection = _dbConnectionFactory.CreateConnection())
                {
                    var parametros = new
                    {
                        p_Identificacion = funcionarioArea.Identificacion,
                        p_ID_Area = funcionarioArea.ID_Area
                    };

                    await connection.ExecuteAsync("SP_Insertar_Funcionario_Area", parametros, commandType: CommandType.StoredProcedure);
                }

                return (true, "Funcionario asociado correctamente.");
            }
            catch (Exception ex)
            {
                return (false, $"{ex.Message}");
            }
        }

        public async Task<(bool Resultado, string Mensaje)> EliminarAsociacionAsync(Funcionarios_Areas funcionarioArea)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection())
                {
                    var parametros = new
                    {
                        p_id_Asocio_Area = funcionarioArea.ID_Funcionario_Area
                    };

                    await connection.ExecuteAsync(
                        "SP_Eliminar_Funcionario_Area",
                        parametros,
                        commandType: CommandType.StoredProcedure
                    );
                }

                return (true, "La asociación se eliminó correctamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar la asociación: {ex.Message}");
            }
        }


    }
}
