using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reloj_Marcador.Repository
{
    public class BitacoraRepository 
    {

        private readonly string _connectionString;

        public BitacoraRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task RegistrarAsync(Bitacora bitacora)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("sp_insertar_bitacora", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@pFecha", bitacora.Fecha);
            cmd.Parameters.AddWithValue("@pUsuario", bitacora.Usuario);
            cmd.Parameters.AddWithValue("@pAccion", bitacora.Accion);
            cmd.Parameters.AddWithValue("@pDescripcion", bitacora.Descripcion);

            await cmd.ExecuteNonQueryAsync();
        }

    }
}
