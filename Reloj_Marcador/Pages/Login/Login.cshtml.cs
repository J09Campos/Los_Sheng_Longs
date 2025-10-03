using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System.Security.Claims;

namespace Reloj_Marcador.Pages.Login
{
    public class LoginModel : PageModel
    {
        
        private readonly ILoginService _loginService;

        public LoginModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // Inputs del Usuario de los Txt

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasena { get; set; } = string.Empty;

        
        public string Mensaje { get; set; } = string.Empty;

        public void OnGet(bool expired = false, bool unauthenticated = false)
        {
            if (expired)
            {
                Mensaje = "Su sesión ha expirado por inactividad.";
            }
            else if (unauthenticated)
            {
                Mensaje = "Por favor inicie sesión para utilizar el sistema.";
            }
        }



        public async Task<IActionResult> OnPostAsync()
        {

            var resultado = await _loginService.LoginAsync(Usuario, Contrasena);

            if (resultado.Mensaje == "Login Exitoso")
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, resultado.Nombre_Completo ?? Usuario),
                    new Claim("Usuario", resultado.Identificacion)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index/Principal");
            }
            else
            {
                Mensaje = resultado.Mensaje;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Cerrar sesión y limpiar cookies

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirigir al Login

            return RedirectToPage("/Login/Login");
        }

    }
}
