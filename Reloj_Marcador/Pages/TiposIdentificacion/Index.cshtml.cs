using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.TiposIdentificacion
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITiposIdentificacionService _service;

        public IndexModel(ITiposIdentificacionService service)
        {
            _service = service;
            Tipos = new List<TipoIdentificacion>();
        }

        public IEnumerable<TipoIdentificacion> Tipos { get; set; }

        [BindProperty]
        public string IdTipo { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allTipos = await _service.ListarAsync();

            int totalRecords = allTipos.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            Tipos = allTipos
                .OrderBy(t => t.ID_TipoIdentificacion)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string IdTipo)
        {
            try
            {
                await _service.EliminarAsync(IdTipo);
                return RedirectToPage();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Message.Contains("No se puede eliminar un registro con datos relacionados") ||
                    ex.Message.Contains("foreign key constraint fails"))
                {
                    TempData["CreateMessage0"] = "No se puede eliminar un registro con datos relacionados.";
                }
                else
                {
                    TempData["CreateMessage0"] = "Ocurrió un error inesperado al intentar eliminar.";
                }

                TempData["CreateTitle0"] = "Operación Fallida.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["CreateMessage0"] = "Ocurrió un error inesperado: " + ex.Message;
                TempData["CreateTitle0"] = "Operación Fallida.";
                return RedirectToPage();
            }
        }
    }
}
