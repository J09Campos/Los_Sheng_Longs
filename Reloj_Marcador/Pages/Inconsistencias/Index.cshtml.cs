using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reloj_Marcador.Pages.Inconsistencias
{
    public class IndexModel : PageModel
    {
        private readonly IInconsistenciasService _inconsistenciasService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IInconsistenciasService inconsistenciasService, ILogger<IndexModel> logger)
        {
            _inconsistenciasService = inconsistenciasService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)] public DateTime? Inicio { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? Fin { get; set; }
        [BindProperty(SupportsGet = true)] public string? Area { get; set; }
        [BindProperty(SupportsGet = true)] public string? Funcionario { get; set; }

        public IEnumerable<Entities.Inconsistencias> Inconsistencias { get; set; } = new List<Entities.Inconsistencias>();

        public async Task OnGetAsync()
        {
            try
            {
                Inicio ??= DateTime.Today.AddDays(-30);
                Fin ??= DateTime.Today;

                Inconsistencias = await _inconsistenciasService.ListarInconsistenciasAsync(Inicio, Fin, Area, Funcionario)
                                  ?? new List<Entities.Inconsistencias>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar inconsistencias.");
                TempData["Error"] = "Ocurrió un error al cargar las inconsistencias.";
                Inconsistencias = new List<Entities.Inconsistencias>();
            }
        }
    }
}
