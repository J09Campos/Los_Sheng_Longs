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

        public MarcasService(Repository.MarcasRepository marcasRepository)
        {
            _marcasRepository = marcasRepository;
        }

        public Task<IEnumerable<(string Id_Area, string Nombre_Area)>> GetAllAreaByID(string id)
        {
            return _marcasRepository.GetAllAreaByID(id);
        }

        public Task<(bool Resultado, string Mensaje)> ValidateUser(Entities.Marcas marca)
        {
            if (!ValidarUsuario(marca))
            {
                return Task.FromResult((false, marca.Mensaje));
            }
            else
            {
                return _marcasRepository.ValidateUser(marca);
            }
        }
        //Validaciones de entrada de datos

        private bool ValidarUsuario(Entities.Marcas marca)
        {

            // Validar identificación
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

            // Validar contraseña
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

            // Validar descripción
            if (string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                marca.Descripcion = "";
            } else 
            {
                if (marca.Descripcion.Length > 100)
                {
                    marca.Mensaje = "La descripción no debe exceder 100 caracteres.";
                    return false;
                }
            }

            // Validar tipo de marca
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

            // Si todo es correcto
            return true;
        }
    }
}
