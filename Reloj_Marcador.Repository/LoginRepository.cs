using Dapper;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class LoginRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public LoginRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Login> LoginAsync(string usuario, string contrasena)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string contrasenaCifrada = Encrypt(contrasena);

            var parameters = new DynamicParameters();
            parameters.Add("p_Usuario", usuario, DbType.String, ParameterDirection.Input);
            parameters.Add("p_Contrasena", contrasenaCifrada, DbType.String, ParameterDirection.Input);
            parameters.Add("p_Mensaje", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
            parameters.Add("p_Nombre_Completo", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("SP_Login_Usuario", parameters, commandType: CommandType.StoredProcedure);

            string mensaje = parameters.Get<string>("p_Mensaje");
            string? nombreCompleto = parameters.Get<string>("p_Nombre_Completo");

            return new Login
            {
                Identificacion = usuario,
                Nombre_Completo = nombreCompleto ?? string.Empty,
                Contrasena = contrasenaCifrada,
                Mensaje = mensaje
            };
        }


        private static readonly string Key = "0123456789abcdef";
        private static readonly string IV = "abcdef0123456789";


        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(IV);
                aesAlg.Padding = PaddingMode.PKCS7;

                using var msEncrypt = new MemoryStream();
                using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                {
                    swEncrypt.Write(plainText);
                }

                byte[] encryptedBytes = msEncrypt.ToArray();
                string base64 = Convert.ToBase64String(encryptedBytes);

                return base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
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

                using var msDecrypt = new MemoryStream(cipherBytes);
                using var csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8);

                return srDecrypt.ReadToEnd();
            }
        }




    }
}
