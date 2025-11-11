using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ITiposInconsistenciasService _inconsistenciasService;

        public CreateModel(ITiposInconsistenciasService inconsistenciaService)
        {
            _inconsistenciasService = inconsistenciaService;
            Inconsistencia = new Tipos_Inconsistencia();
        }

        [BindProperty]
        public Tipos_Inconsistencia Inconsistencia { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _inconsistenciasService.CRUDAsync(Inconsistencia, "Crear");

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
