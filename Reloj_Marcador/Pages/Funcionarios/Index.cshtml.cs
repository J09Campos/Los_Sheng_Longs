using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class IndexModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public IndexModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionarios = new List<Funcionarios_Usuarios>();
        }

        public IEnumerable<Funcionarios_Usuarios> Funcionarios { get; set; }

        [BindProperty]
        public string Identificacion { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allFuncionarios = await _funcionariosService.ListarAsync();

            int totalRecords = allFuncionarios.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            Funcionarios = allFuncionarios
                .OrderBy(f => f.Identificacion)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        // Handler para la eliminación desde el modal
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (string.IsNullOrEmpty(Identificacion))
            {
                TempData["CreateMessage"] = "No se recibió una identificación válida.";
                TempData["CreateTitle"] = "Error";
                return RedirectToPage();
            }

            var rowsAffected = await _funcionariosService.EliminarAsync(Identificacion);

            if (rowsAffected == 0)
            {
                TempData["CreateMessage"] = "No se pudo eliminar el funcionario.";
                TempData["CreateTitle"] = "Error";
            }
            else
            {
                TempData["CreateMessage"] = "Funcionario eliminado correctamente.";
                TempData["CreateTitle"] = "Éxito";
            }

            return RedirectToPage();
        }
    }
}

