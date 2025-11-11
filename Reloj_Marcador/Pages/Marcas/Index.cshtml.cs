using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reloj_Marcador.Pages.Marcas
{
    public class IndexModel : PageModel
    {
        private readonly IMarcasService _marcasService;

        public IndexModel(IMarcasService marcasService)
        {
            _marcasService = marcasService;
        }
        [BindProperty(SupportsGet = true)] public DateTime? Inicio { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? Fin { get; set; }
        [BindProperty(SupportsGet = true)] public string? Funcionario { get; set; }

        public IEnumerable<MarcasReporte> Marcas { get; set; } = new List<MarcasReporte>();

        public async Task OnGetAsync()
        {
            try
            {
                Inicio ??= DateTime.Today.AddDays(-7);
                Fin ??= DateTime.Today;

                Marcas = await _marcasService.ListarMarcasReporteAsync(Inicio, Fin, Funcionario);

                if (Marcas == null || !Marcas.Any())
                    TempData["Info"] = "No se encontraron marcas para los filtros seleccionados.";
            }
            catch (Exception ex)
            {
        
                TempData["Error"] = $"Ocurrió un error al cargar las marcas: {ex.Message}";
            }
        }
    }
}
