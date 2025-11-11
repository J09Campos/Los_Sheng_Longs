using Microsoft.AspNetCore.Http;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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

        public async Task<(bool Resultado, string Mensaje)> ValidateUser(Marcas marca)
        {
            if (ValidarUsuario(marca))
            {
                if (string.IsNullOrEmpty(marca.IP_Registro) || marca.IP_Registro == "::1")
                {
                    marca.IP_Registro = ObtenerIpCliente(_httpContextAccessor);
                }

                if (marca.Latitud == null || marca.Longitud == null)
                {
                    var (lat, lon) = await ObtenerGeoDesdeIpAsync(marca.IP_Registro);
                    marca.Latitud = lat;
                    marca.Longitud = lon;
                }


                var resultado = await _marcasRepository.ValidateUser(marca);

                if (resultado.Resultado)
                {
                    string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonimo";
                    var datosBitacora = new
                    {
                        marca.Identificacion,
                        marca.Id_Area,
                        marca.Tipo_Marca,
                        marca.IP_Registro,
                        marca.Latitud,
                        marca.Longitud,
                        marca.Hora_Servidor
                    };

                    await _bitacoraService.RegistrarAsync(usuario, "Registro de marca", datosBitacora);
                }

                return resultado;
            }

            return (false, marca.Mensaje);
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

        public async Task<IEnumerable<MarcasReporte>> ListarMarcasReporteAsync(DateTime? inicio, DateTime? fin, string? usuario)
        {
            var marcas = await _marcasRepository.GetMarcasReporteAsync(inicio, fin, usuario);


            string admin = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Administrador";
            var datosBitacora = new
            {
                FiltroInicio = inicio,
                FiltroFin = fin,
                Usuario = usuario,
                Cantidad = marcas?.Count() ?? 0
            };

            await _bitacoraService.RegistrarAsync(admin, "Consulta de reporte de marcas", datosBitacora);

            return marcas;
        }


        public async Task<IEnumerable<Marcas>> ListarMarcasAsync(DateTime? inicio, DateTime? fin, string? funcionario)
        {
            return await _marcasRepository.GetMarcasAsync(inicio, fin, funcionario);
        }

        private string ObtenerIpCliente(IHttpContextAccessor ctx)
        {
            var http = ctx.HttpContext;
            if (http == null) return "unknown";

            var xff = http.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xff))
                return xff.Split(',')[0].Trim();

            var realIp = http.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
                return realIp;

            return http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private async Task<(double?, double?)> ObtenerGeoDesdeIpAsync(string? ip)
        {
            if (string.IsNullOrEmpty(ip) || ip == "unknown")
                return (null, null);

            try
            {
                using var http = new HttpClient();
                var url = $"http://ip-api.com/json/{ip}";
                var resp = await http.GetFromJsonAsync<IpApiResponse>(url);
                if (resp?.Status == "success")
                    return (resp.Lat, resp.Lon);
            }
            catch { }

            return (null, null);
        }

        private class IpApiResponse
        {
            public string Status { get; set; } = "";
            public double Lat { get; set; }
            public double Lon { get; set; }
        }


    }
}