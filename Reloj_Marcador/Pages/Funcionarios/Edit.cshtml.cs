using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class EditModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public EditModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionario = new Funcionarios_Usuarios();
        }

        [BindProperty]
        public Funcionarios_Usuarios Funcionario { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Funcionario = await _funcionariosService.ObtenerPorIdAsync(id);

            if (Funcionario == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _funcionariosService.ActualizarAsync(Funcionario);

            return RedirectToPage("Index");
        }
    }
}

