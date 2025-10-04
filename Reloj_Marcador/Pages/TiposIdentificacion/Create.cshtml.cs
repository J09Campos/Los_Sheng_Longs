using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.TiposIdentificacion
{
    public class CreateModel : PageModel
    {
        private readonly ITiposIdentificacionService _service;

        public CreateModel(ITiposIdentificacionService service)
        {
            _service = service;
        }

        [BindProperty]
        public TipoIdentificacion Tipo { get; set; } = new TipoIdentificacion();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _service.CrearAsync(Tipo);
                TempData["CreateMessage"] = "Tipo Identificacion creado con éxito.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
