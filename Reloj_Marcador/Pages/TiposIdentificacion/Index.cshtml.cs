using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.TiposIdentificacion
{
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage();
            }
        }
    }
}
