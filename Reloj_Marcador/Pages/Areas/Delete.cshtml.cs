using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Areas
{
    public class DeleteModel : PageModel
    {
        private readonly IAreaService _areaService;

        public DeleteModel(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [BindProperty]
        public Area? Area { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Area = await _areaService.GetByIdAsync(id);

            if (Area == null)
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

            try
            {
                var (resultado, mensaje) = await _areaService.DeleteAsync(Area);

                if (!resultado)
                {
                    ModelState.AddModelError(string.Empty, mensaje);
                    return Page();
                }
                return RedirectToPage("Index");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
