using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Pages.Marca_Entrada_Salida
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Services.Abstract.IMarcasService _marcasService;

        public IndexModel(Services.Abstract.IMarcasService marcasService)
        {
            _marcasService = marcasService;
            Marca = new Marcas();
            AreasLista = new SelectList(Enumerable.Empty<Area>(), "Id_Area", "Nombre_Area");
        }

        [BindProperty]
        public Marcas Marca { get; set; }

        public SelectList AreasLista { get; set; }

        public async Task<JsonResult> OnGetObtenerIdAsync(string identificacion)
        {
            List<SelectListItem> selectList;

            if (string.IsNullOrEmpty(identificacion))
            {
                selectList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "--Seleccione--" }
                };
            }
            else
            {
                var lista = await _marcasService.GetAllAreaByID(identificacion);

                selectList = lista.Select(a => new SelectListItem
                {
                    Value = a.Item1,
                    Text = a.Item2
                }).ToList();

                selectList.Insert(0, new SelectListItem { Value = "", Text = "Seleccione..." });
            }

            return new JsonResult(selectList);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var (resultado, mensaje) = await _marcasService.ValidateUser(Marca);

            if (!resultado)
            {
                TempData["ModalTitle"] = "Operación Fallida";
                TempData["ModalMessage"] = mensaje;
                return Page();
            }

            var horaServidor = DateTime.Now.ToString("HH:mm:ss"); 

            TempData["ModalTitle"] = "Operación Exitosa";
            TempData["ModalMessage"] = $"Hora Servidor: {horaServidor}";

            return RedirectToPage("Index");
        }
    }
}

