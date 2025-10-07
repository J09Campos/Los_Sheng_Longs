using Dapper;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class FuncionariosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        private static readonly string Key = "0123456789abcdef";
        private static readonly string IV = "abcdef0123456789";

        public FuncionariosRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Funcionarios_Usuarios>> ListarAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var funcionarios = await connection.QueryAsync<Funcionarios_Usuarios>(
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

                foreach (var f in funcionarios)
                {
                    if (!string.IsNullOrEmpty(f.Contrasena))
                    {
                        try
                        {
                            f.Contrasena = Decrypt(f.Contrasena);
                        }
                        catch
                        {
                            f.Contrasena = "********"; 
                        }
                    }
                }

                return funcionarios;
            }
        }

        public async Task<Funcionarios_Usuarios?> ObtenerPorIdAsync(string identificacion)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var funcionario = await connection.QueryFirstOrDefaultAsync<Funcionarios_Usuarios>(
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

                if (funcionario != null && !string.IsNullOrEmpty(funcionario.Contrasena))
                {
                    try
                    {
                        funcionario.Contrasena = Decrypt(funcionario.Contrasena);
                    }
                    catch
                    {
                        funcionario.Contrasena = "********";
                    }
                }

                return funcionario;
            }
        }
        public async Task<int> CrearAsync(Funcionarios_Usuarios funcionario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                if (!string.IsNullOrEmpty(funcionario.Contrasena))
                {
                    funcionario.Contrasena = Encrypt(funcionario.Contrasena);
                }

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
                if (!string.IsNullOrEmpty(funcionario.Contrasena) && funcionario.Contrasena != "********")
                {
                    funcionario.Contrasena = Encrypt(funcionario.Contrasena);
                }
                else
                {
                    funcionario.Contrasena = null;
                }

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
                        p_Contrasena = funcionario.Contrasena,
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

        public async Task<IEnumerable<TipoIdentificacion>> ObtenerTiposIdentificacionAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_accion", "OBTENER_TIPOS_IDENTIFICACION");
                parameters.Add("p_TipoIdentificacion", null);
                parameters.Add("p_Identificacion", null);
                parameters.Add("p_Nombre", null);
                parameters.Add("p_Apellido", null);
                parameters.Add("p_Correo", null);
                parameters.Add("p_Contrasena", null);
                parameters.Add("p_ID_Rol", null);
                parameters.Add("p_Estado", null);

                return await connection.QueryAsync<TipoIdentificacion>(
                    "sp_FuncionariosCRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_accion", "OBTENER_ROLES");
                parameters.Add("p_TipoIdentificacion", null);
                parameters.Add("p_Identificacion", null);
                parameters.Add("p_Nombre", null);
                parameters.Add("p_Apellido", null);
                parameters.Add("p_Correo", null);
                parameters.Add("p_Contrasena", null);
                parameters.Add("p_ID_Rol", null);
                parameters.Add("p_Estado", null);

                return await connection.QueryAsync<Rol>(
                    "sp_FuncionariosCRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Estado>> ObtenerEstadosAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_accion", "OBTENER_ESTADOS");
                parameters.Add("p_TipoIdentificacion", null);
                parameters.Add("p_Identificacion", null);
                parameters.Add("p_Nombre", null);
                parameters.Add("p_Apellido", null);
                parameters.Add("p_Correo", null);
                parameters.Add("p_Contrasena", null);
                parameters.Add("p_ID_Rol", null);
                parameters.Add("p_Estado", null);

                return await connection.QueryAsync<Estado>(
                    "sp_FuncionariosCRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                    {
                        swEncrypt.Write(plainText);
                    }

                    byte[] encryptedBytes = msEncrypt.ToArray();
                    string base64 = Convert.ToBase64String(encryptedBytes);
                    return base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            string base64 = cipherText.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            byte[] cipherBytes = Convert.FromBase64String(base64);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

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
                return (false, $"{ex.Message}");
            }
        }


    }
}

