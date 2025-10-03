using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Areas
{
    public class CreateModel : PageModel
    {
        private readonly IAreaService _areaService;

        public CreateModel(IAreaService areaService)
        {
            _areaService = areaService;
            Area = new Area();
        }

        [BindProperty]
        public Area Area { get; set; }



        public List<SelectListItem> ListaJefes { get; set; }

        public async Task OnGetAsync()
        {
            var jefes = await _areaService.GetJefesAsync();

            ListaJefes = jefes.Select(j => new SelectListItem
            {
                Value = j.ID_Jefe,
                Text = j.Nombre_Jefe
            }).ToList();

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

                TempData["CreateTitle"] = "Validación Fallida";
                TempData["CreateMessage"] = "Existen errores en el formulario. Por favor, revise los campos.";
                await CargarListaJefes();
                return Page();
            }

            var (resultado, mensaje) = await _areaService.InsertAsync(Area);

            if (!resultado)
            {

                TempData["CreateTitle"] = "Operación Fallida";
                TempData["CreateMessage"] = mensaje;
                await CargarListaJefes();
                return Page();
            }

            TempData["CreateTitle"] = "Operación Exitosa";
            TempData["CreateMessage"] = "El área se registró correctamente.";

            return RedirectToPage("Index");
        }

    }
}
