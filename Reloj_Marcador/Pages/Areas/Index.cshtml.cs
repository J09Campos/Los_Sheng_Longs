using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Services.Abstract;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Areas
{
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
            // Traemos todas las áreas (si tu servicio soporta paginación, aquí sería mejor)
            var allAreas = await _areaService.GetAllAsync();

            // Calculamos la paginación
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
            if (string.IsNullOrEmpty(IdArea))
            {
                ModelState.AddModelError(string.Empty, "Id no válido.");
                return Page();
            }

            try
            {
                var area = await _areaService.GetByIdAsync(IdArea);
                if (area == null)
                {
                    ModelState.AddModelError(string.Empty, "Área no encontrada.");
                    return Page();
                }

                var (resultado, mensaje) = await _areaService.DeleteAsync(area);

                if (!resultado)
                {
                    ModelState.AddModelError(string.Empty, mensaje);
                    return Page();
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
