using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    public class CreateModel : PageModel
    {
        private readonly IInconsistenciasService _inconsistenciasService;

        public CreateModel(IInconsistenciasService inconsistenciaService)
        {
            _inconsistenciasService = inconsistenciaService;
            Inconsistencia = new Inconsistencias();
        }

        [BindProperty]
        public Inconsistencias Inconsistencia { get; set; }

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

            await _inconsistenciasService.CRUDAsync(Inconsistencia, "Crear");

            return RedirectToPage("Index");
        }
    }
}