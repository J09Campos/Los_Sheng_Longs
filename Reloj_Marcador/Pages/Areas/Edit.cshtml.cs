using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Areas
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IAreaService _areaService;

        public EditModel(IAreaService areaService)
        {
            _areaService = areaService;
            Area = new Area();
        }

        [BindProperty]
        public Area Area { get; set; }

        public List<SelectListItem> ListaJefes { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            var jefes = await _areaService.GetJefesAsync();

            if (string.IsNullOrEmpty(id))
            {
                Area = new Area();
            }
            else
            {
                Area = await _areaService.GetByIdAsync(id);

                if (Area == null)
                {
                    return NotFound();
                }
            }

            ListaJefes = jefes.Select(j => new SelectListItem { Value = j.ID_Jefe, Text = j.Nombre_Jefe, Selected = (Area != null && Area.Jefe == j.ID_Jefe) }).ToList();
            return Page();
        }

        private async Task CargarListaJefes()
        {
            var jefes = await _areaService.GetJefesAsync();
            ListaJefes = jefes.Select(j => new SelectListItem
            {
                Value = j.ID_Jefe,
                Text = j.Nombre_Jefe
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["EditMessage"] = "Existen errores en el formulario.";
                TempData["EditTitle"] = "Operación Fallida";
                await CargarListaJefes();
                return Page();
            }

            var (resultado, mensaje) = await _areaService.UpdateAsync(Area);

            if (!resultado)
            {
                TempData["EditMessage"] = mensaje;
                TempData["EditTitle"] = "Operación Fallida";
                await CargarListaJefes();
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
