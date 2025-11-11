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
        public async Task<(bool Resultado, string Mensaje)> ValidateUser(Marcas marca)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();
                parametros.Add("p_Usuario", marca.Identificacion, DbType.String);
                parametros.Add("p_Contrasena", Encrypt(marca.Contrasena), DbType.String);
                parametros.Add("p_Id_Area", marca.Id_Area, DbType.String);
                parametros.Add("p_Descripcion", marca.Descripcion, DbType.String);
                parametros.Add("p_Tipo_Marca", marca.Tipo_Marca, DbType.String);
                parametros.Add("p_Fecha", marca.Fecha, DbType.Date);
                parametros.Add("p_Hora_Servidor", marca.Hora_Servidor, DbType.Time);

                // NUEVOS CAMPOS
                parametros.Add("p_IP_Registro", marca.IP_Registro, DbType.String, ParameterDirection.Input);
                parametros.Add("p_Latitud", marca.Latitud, DbType.Double, ParameterDirection.Input);
                parametros.Add("p_Longitud", marca.Longitud, DbType.Double, ParameterDirection.Input);


                parametros.Add("p_Mensaje", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                parametros.Add("p_Resultado", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SP_Marcar_Entrada_Salida", parametros, commandType: CommandType.StoredProcedure);

                string mensaje = parametros.Get<string>("p_Mensaje");
                bool resultado = parametros.Get<byte>("p_Resultado") == 1;
                return (resultado, mensaje);
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



        public async Task<IEnumerable<MarcasReporte>> GetMarcasReporteAsync(DateTime? fechaInicio, DateTime? fechaFin, string? identificacion)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"
        SELECT ID_Marca, Identificacion, ID_Area AS Id_Area, Tipo_Marca, Fecha,
            Hora_Servidor,IP_Registro, Latitud,Longitud FROM marcas
        WHERE (@fechaInicio IS NULL OR Fecha >= @fechaInicio)
          AND (@fechaFin IS NULL OR Fecha <= @fechaFin)
          AND (@identificacion IS NULL OR Identificacion = @identificacion)
        ORDER BY Fecha DESC, Hora_Servidor DESC;";

            return await connection.QueryAsync<MarcasReporte>(sql, new
            {
                fechaInicio,
                fechaFin,
                identificacion
            });
        }

        public async Task<IEnumerable<Marcas>> GetMarcasAsync(DateTime? inicio, DateTime? fin, string? funcionario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"
        SELECT Id_Marca,Identificacion,Id_Area,Descripcion,Tipo_Marca,Hora_Servidor, Fecha, IP_Registro, Latitud, Longitud
        FROM marcas WHERE (@inicio IS NULL OR Fecha >= @inicio) AND (@fin IS NULL OR Fecha <= @fin) AND (@funcionario IS NULL OR Identificacion = @funcionario)
        ORDER BY Fecha DESC;";

            var result = await connection.QueryAsync<Marcas>(sql, new
            {
                inicio,
                fin,
                funcionario
            });

            return result;
        }
    }
}

