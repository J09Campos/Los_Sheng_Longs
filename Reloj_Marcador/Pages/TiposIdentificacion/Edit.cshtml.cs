using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.TiposIdentificacion
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ITiposIdentificacionService _service;

        public EditModel(ITiposIdentificacionService service)
        {
            _service = service;
        }

        [BindProperty]
        public TipoIdentificacion Tipo { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Tipo = await _service.ObtenerPorIdAsync(id);

            if (Tipo == null)
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.ActualizarAsync(Tipo);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["CreateTitle8"] = "Operación Fallida";
                TempData["CreateMessage8"] = ex.Message;
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}

