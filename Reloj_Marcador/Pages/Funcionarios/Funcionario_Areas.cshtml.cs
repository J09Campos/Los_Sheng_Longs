using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class Funcionario_AreasModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public Funcionario_AreasModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            ListaAreas = new List<Funcionarios_Areas>();
        }

        [BindProperty]
        public string? Identificacion { get; set; }

        [BindProperty]
        public int IdArea { get; set; } // ID_Funcionario_Area para eliminar

        public List<Funcionarios_Areas> ListaAreas { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        // GET: cargar lista
        public async Task OnGetAsync(string identificacion, int pageNumber = 1)
        {
            Identificacion = identificacion;
            await CargarAreasAsync(pageNumber);
        }

        private async Task CargarAreasAsync(int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(Identificacion))
            {
                ListaAreas = new List<Funcionarios_Areas>();
                TotalPages = 0;
                CurrentPage = 1;
                return;
            }

            var allAreas = (await _funcionariosService.ListarFuncionariosAreasAsync(Identificacion)).ToList();
            int totalRecords = allAreas.Count;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            ListaAreas = allAreas
                .OrderBy(f => f.ID_Funcionario_Area)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        // POST: eliminar asociación
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (IdArea <= 0)
            {
                TempData["CreateTitle"] = "Operación Fallida";
                TempData["CreateMessage"] = "ID de asociación inválido.";
                return RedirectToPage(new { identificacion = Identificacion });
            }

            var asociacion = new Funcionarios_Areas
            {
                ID_Funcionario_Area = IdArea
            };

            var (resultado, mensaje) = await _funcionariosService.EliminarAsociacionAsync(asociacion);


            // Refresca la lista
            return RedirectToPage(new { identificacion = Identificacion, pageNumber = CurrentPage });
        }
    }
}
