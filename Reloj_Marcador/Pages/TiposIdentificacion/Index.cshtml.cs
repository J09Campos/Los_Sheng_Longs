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

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (string.IsNullOrEmpty(IdTipo))
            {
                TempData["CreateMessage0"] = "No se recibió un tipo de identificación válido.";
                TempData["CreateTitle0"] = "Operación Fallida";
                return RedirectToPage();
            }

            try
            {
                var rowsAffected = await _service.EliminarAsync(IdTipo);

                if (rowsAffected == 0)
                {
                    TempData["CreateMessage0"] = "No se pudo eliminar el tipo de identificación.";
                    TempData["CreateTitle0"] = "Operación Fallida";
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Message.Contains("porque está siendo usado por funcionarios") ||
                    ex.Message.Contains("foreign key constraint fails"))
                {
                    TempData["CreateMessage0"] = "No se puede eliminar este tipo de identificación porque está asociado a uno o más funcionarios.";
                    TempData["CreateTitle0"] = "Operación Fallida";
                }
                else
                {
                    TempData["CreateMessage0"] = "Ocurrió un error inesperado al intentar eliminar.";
                    TempData["CreateTitle0"] = "Operación Fallida";
                }
            }

            return RedirectToPage();
        }
    }
}
