using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public readonly ITiposInconsistenciasService _inconsistenciasService;

        public IndexModel(ITiposInconsistenciasService inconsistenciasService)
        {
            _inconsistenciasService = inconsistenciasService;
        }

        public IEnumerable<Tipos_Inconsistencia> Inconsistencias { get; set; }

        [BindProperty]
        public Tipos_Inconsistencia Inconsistencia { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private const int PageSize = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allInconsistencias = await _inconsistenciasService.GetAllAsync();

            int totalRecords = allInconsistencias.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            Inconsistencias = allInconsistencias
                .OrderBy(a => a.Id_Inconsistencia)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

        }
        public async Task<IActionResult> OnPostDeleteAsync(int IdInconsistencia)
        {
            Inconsistencia.Id_Inconsistencia = IdInconsistencia;
            await _inconsistenciasService.CRUDAsync(Inconsistencia, "Eliminar");

            return RedirectToPage();
        }
    }
}


