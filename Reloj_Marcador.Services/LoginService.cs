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

        public async Task<(string Mensaje, string? NombreCompleto)> LoginAsync(string usuario, string contrasena)
        {

            
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                return ("Usuario y/o contraseña incorrectos.", null);
            }

            if (usuario.Length > 255)
            {
                return ("El usuario no puede tener más de 255 caracteres.", null);
            }


            if (contrasena.Length > 255)
            {
                return ("La contraseña no puede tener más de 255 caracteres.", null);
            }

            var resultado = await _loginRepository.LoginAsync(usuario, contrasena);

            
            return resultado;
        }

    }
}
