using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ITiposInconsistenciasService _inconsistenciasService;

        public EditModel(ITiposInconsistenciasService personaService)
        {
            _inconsistenciasService = personaService;
            Inconsistencia = new Tipos_Inconsistencia();
        }

        [BindProperty]
        public Tipos_Inconsistencia Inconsistencia { get; set; }

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
                TempData["ModalTitle"] = "Operaci n Fallida";
                TempData["ModalMessage"] = Inconsistencia.Mensaje;

                return Page();

            }
            else
            {
                TempData["ModalTitle"] = "Operaci n Exitosa";
                TempData["ModalMessage"] = Inconsistencia.Mensaje;

                return RedirectToPage("Index");

            }
        }
    }
}


