using Microsoft.AspNetCore.Http;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class MarcasService : IMarcasService
    {
        private readonly Repository.MarcasRepository _marcasRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MarcasService(Repository.MarcasRepository marcasRepository, IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _marcasRepository = marcasRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IEnumerable<(string Id_Area, string Nombre_Area)>> GetAllAreaByID(string id)
        {
            return _marcasRepository.GetAllAreaByID(id);
        }

        public async Task<(bool Resultado, string Mensaje)> ValidateUser(Entities.Marcas marca)
        {
            if (ValidarUsuario(marca))
            {
                var resultado = await _marcasRepository.ValidateUser(marca);
                if (resultado.Resultado)
                {
                    string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                    var datosBitacora = new
                    {
                        marca.Identificacion,
                        marca.Descripcion,
                        marca.Id_Area,
                        marca.Tipo_Marca
                    };

                    await _bitacoraService.RegistrarAsync(usuario, "Marco", datosBitacora);
                }

                return resultado;

            }
            else
            {
                return (false, marca.Mensaje);
            }


        }

        private bool ValidarUsuario(Entities.Marcas marca)
        {

            if (string.IsNullOrWhiteSpace(marca.Identificacion))
            {
                marca.Mensaje = "Usuario y/o contraseña incorrectos.";
                return false;
            }
            if (marca.Identificacion.Length > 22)
            {
                marca.Mensaje = "La identificación no debe exceder 22 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(marca.Contrasena))
            {
                marca.Mensaje = "Usuario y/o contraseña incorrectos.";
                return false;
            }
            if (marca.Contrasena.Length > 255)
            {
                marca.Mensaje = "La contraseña no debe exceder 255 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(marca.Id_Area))
            {
                marca.Mensaje = "El área es obligatoria.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                marca.Descripcion = "";
            }
            else
            {
                if (marca.Descripcion.Length > 100)
                {
                    marca.Mensaje = "La descripción no debe exceder 100 caracteres.";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(marca.Tipo_Marca))
            {
                marca.Mensaje = "El tipo de marca es obligatorio.";
                return false;
            }
            if (!(marca.Tipo_Marca.Equals("Entrada", StringComparison.OrdinalIgnoreCase)
               || marca.Tipo_Marca.Equals("Salida", StringComparison.OrdinalIgnoreCase)))
            {
                marca.Mensaje = "El tipo de marca debe ser 'Entrada' o 'Salida'.";
                return false;
            }

            return true;
        }
    }
}
