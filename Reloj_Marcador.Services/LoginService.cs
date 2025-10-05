using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class LoginService : ILoginService
    {

        private readonly LoginRepository _loginRepository;

        public LoginService(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<Login> LoginAsync(string usuario, string contrasena)
        {

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                return new Login
                {
                    Identificacion = usuario,
                    Contrasena = contrasena,
                    Nombre_Completo = string.Empty,
                    Mensaje = "Usuario y/o contraseña incorrectos."
                };
            }

            if (usuario.Length > 255)
            {
                return new Login
                {
                    Identificacion = usuario,
                    Contrasena = contrasena,
                    Nombre_Completo = string.Empty,
                    Mensaje = "El usuario no puede tener más de 255 caracteres."
                };
            }

            if (contrasena.Length > 255)
            {
                return new Login
                {
                    Identificacion = usuario,
                    Contrasena = contrasena,
                    Nombre_Completo = string.Empty,
                    Mensaje = "La contraseña no puede tener más de 255 caracteres."
                };
            }


            var resultado = await _loginRepository.LoginAsync(usuario, contrasena);

            return resultado;
        }


    }
}
