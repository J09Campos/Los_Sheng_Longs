using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    public class EditModel : PageModel
    {
        private readonly IInconsistenciasService _inconsistenciasService;

        public EditModel(IInconsistenciasService personaService)
        {
            _inconsistenciasService = personaService;
            Inconsistencia = new Inconsistencias();
        }

        [BindProperty]
        public Inconsistencias Inconsistencia { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Inconsistencia = await _inconsistenciasService.GetByIdAsync(id);

            if (Inconsistencia == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _inconsistenciasService.CRUDAsync(Inconsistencia, "Actualizar");

            if (!Inconsistencia.Resultado.HasValue)
            {
                TempData["ModalTitle"] = "Operación Fallida";
                TempData["ModalMessage"] = Inconsistencia.Mensaje;

                return Page();

            }
            else
            {
                TempData["ModalTitle"] = "Operación Exitosa";
                TempData["ModalMessage"] = Inconsistencia.Mensaje;

                return RedirectToPage("Index");

            }
        }
    }
}
