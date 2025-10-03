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

        // 🔹 LISTAR (desencripta la contraseña antes de devolverla)
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
                            f.Contrasena = "********"; // fallback
                        }
                    }
                }

                return funcionarios;
            }
        }

        // 🔹 OBTENER POR ID (desencripta contraseña)
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

        // 🔹 CREAR (encripta antes de guardar)
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

        // 🔹 ACTUALIZAR (solo encripta si la contraseña no es "********")
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
                    funcionario.Contrasena = null; // no actualizar
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

        // 🔹 ELIMINAR
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


        // =====================================================
        // 🔐 Métodos de Encriptación / Desencriptación (AES)
        // =====================================================
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
    }
}

