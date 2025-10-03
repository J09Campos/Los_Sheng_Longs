using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class DeleteModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public DeleteModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
        }

        [BindProperty]
        public Funcionarios_Usuarios? Funcionario { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Funcionario = await _funcionariosService.ObtenerPorIdAsync(id);

            if (Funcionario == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _funcionariosService.EliminarAsync(id);

            return RedirectToPage("Index");
        }
    }
}
