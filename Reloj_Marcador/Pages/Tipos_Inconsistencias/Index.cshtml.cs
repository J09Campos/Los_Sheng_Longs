using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Tipos_Inconsistencias
{
    public class IndexModel : PageModel
    {
        public readonly IInconsistenciasService _inconsistenciasService;

        public IndexModel(IInconsistenciasService inconsistenciasService)
        {
            _inconsistenciasService = inconsistenciasService;
        }

        public IEnumerable<Inconsistencias> Inconsistencias { get; set; }

        public async Task OnGetAsync()
        {
            Inconsistencias = await _inconsistenciasService.GetAllAsync();
        }
    }
}
