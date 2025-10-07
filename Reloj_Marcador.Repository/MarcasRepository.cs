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
    public class MarcasRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;
        public MarcasRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<(string Id_Area, string Nombre_Area)>> GetAllAreaByID(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();
                parametros.Add("p_Identificacion", id, DbType.String, ParameterDirection.Input);

                var lista = await connection.QueryAsync<(string, string)>(
                    "SP_Listar_Areas_CBO",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return lista;
            }
        }
        public async Task<(bool Resultado , string Mensaje)> ValidateUser(Marcas marca)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();
                parametros.Add("p_Usuario", marca.Identificacion, DbType.String, ParameterDirection.Input);

                string ContraEncritada = Encrypt(marca.Contrasena);

                parametros.Add("p_Contrasena", ContraEncritada, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Id_Area", marca.Id_Area, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Descripcion", marca.Descripcion, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Tipo_Marca", marca.Tipo_Marca, DbType.String, ParameterDirection.Input);


                parametros.Add("p_Mensaje", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                parametros.Add("p_Resultado", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await connection.QueryAsync<Area>(
                    "SP_Marcar_Entrada_Salida",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                string mensaje = parametros.Get<string>("p_Mensaje");
                byte resultadoByte = parametros.Get<byte>("p_Resultado");
                bool resultado = resultadoByte == 1;
                return (Convert.ToBoolean(resultado), mensaje);

            }
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




    }
}
