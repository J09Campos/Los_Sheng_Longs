using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Areas
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAreaService _areaService;

        public IndexModel(IAreaService areaService)
        {
            _areaService = areaService;
            Areas = new List<Area>();
        }

        public IEnumerable<Area> Areas { get; set; }

        [BindProperty]
        public string? IdArea { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allAreas = await _areaService.GetAllAsync();

            int totalRecords = allAreas.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            Areas = allAreas
                .OrderBy(a => a.ID_Area)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {

            var area = await _areaService.GetByIdAsync(IdArea);
            if (area == null)
            {
                TempData["CreateTitle"] = "Operación Fallida";
                TempData["CreateMessage"] = "Área no encontrada.";
                return RedirectToPage(new { pageNumber = CurrentPage });
            }

            var (resultado, mensaje) = await _areaService.DeleteAsync(area);

            TempData["CreateTitle"] = "Operación Exitosa";
            TempData["CreateMessage"] = mensaje;

            return RedirectToPage(new { pageNumber = CurrentPage });
        }
    }
}
