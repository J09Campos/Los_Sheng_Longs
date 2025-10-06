using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class BitacoraService : IBitacoraService
    {
        private readonly BitacoraRepository _bitacoraRepository;

        public BitacoraService(BitacoraRepository bitacorarepository)
        {
            _bitacoraRepository = bitacorarepository;
        }

        public async Task RegistrarAsync(string usuario, string accion, object descripcion)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = false // opcional: JSON en una sola línea
                };

                string descripcionJson = descripcion is string str ? str: JsonSerializer.Serialize(descripcion, options);

                var bitacora = new Bitacora
                {
                    Usuario = usuario,
                    Accion = accion,
                    Fecha = DateTime.Now,
                    Descripcion = descripcionJson
                };

                await _bitacoraRepository.RegistrarAsync(bitacora);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registrando bitácora: {ex.Message}");
            }
        }

    }
}
