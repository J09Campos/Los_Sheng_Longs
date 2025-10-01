using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    public class DeleteModel : PageModel
    {
        private readonly IInconsistenciasService _inconsistenciaService;

        public DeleteModel(IInconsistenciasService personaService)
        {
            _inconsistenciaService = personaService;
            //Persona = new Persona();
        }

        [BindProperty]
        public Inconsistencias Inconsistencia { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Inconsistencia = await _inconsistenciaService.GetByIdAsync(id);

            if (Inconsistencia == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Inconsistencia.Id_Inconsistencia = int.Parse(id);
            await _inconsistenciaService.CRUDAsync(Inconsistencia, "Eliminar");

            return RedirectToPage("Index");
        }
    }
}