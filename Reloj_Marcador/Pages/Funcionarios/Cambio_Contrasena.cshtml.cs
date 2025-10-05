using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class Cambio_ContrasenaModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public Cambio_ContrasenaModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionario = new Funcionarios_Usuarios();
        }

        [BindProperty]
        public Funcionarios_Usuarios Funcionario { get; set; }

        [BindProperty]
        public string identificacion { get; set; } = string.Empty;

        [BindProperty]
        public string contrasena { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Funcionario = await _funcionariosService.ObtenerPorIdAsync(id);


            if (Funcionario == null)
            {
                return NotFound();
            }

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    await _funcionariosService.Cambiar_ContrasenaAsync(identificacion,contrasena);

        //    return RedirectToPage("Index");
        //}

        public async Task<IActionResult> OnPostAceptarAsync()
        {
            var (resultado, mensaje) = await _funcionariosService.Cambiar_ContrasenaAsync(identificacion, contrasena);

            if (!ModelState.IsValid)
            {
                TempData["ChangeTitle"] = "Operación Fallida";
                TempData["ChangeMessage"] = mensaje;
                return Page();
            }



            if (!resultado)
            {
                TempData["ChangeTitle"] = "Operación Fallida";
                TempData["ChangeMessage"] = mensaje;
                return Page();
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAutogenerarAsync()
        {
            var nuevaContra = await _funcionariosService.Crear_ContrasenaAsync();

            contrasena = nuevaContra;

            return Page();
        }
    }
}
