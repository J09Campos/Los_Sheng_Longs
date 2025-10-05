using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class CreateModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public CreateModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionario = new Funcionarios_Usuarios();
        }

        [BindProperty]
        public Funcionarios_Usuarios Funcionario { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _funcionariosService.CrearAsync(Funcionario);

            return RedirectToPage("Index");
        }
    }
}
