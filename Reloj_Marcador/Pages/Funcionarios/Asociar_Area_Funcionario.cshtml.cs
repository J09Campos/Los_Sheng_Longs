using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class Asociar_Area_FuncionarioModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public Asociar_Area_FuncionarioModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            FuncionarioArea = new Funcionarios_Areas();
            ListaAreas = new List<SelectListItem>();
        }

        [BindProperty]
        public Funcionarios_Areas FuncionarioArea { get; set; }

        public List<SelectListItem> ListaAreas { get; set; }

        public async Task OnGetAsync(string identificacion)
        {
            if (string.IsNullOrEmpty(identificacion) && TempData.ContainsKey("Identificacion"))
            {
                identificacion = TempData["Identificacion"]?.ToString();
            }

            FuncionarioArea.Identificacion = identificacion;
            await CargarListaAreas();

            // Guarda en TempData para que se conserve al hacer Post
            TempData["Identificacion"] = identificacion;
        }

        private async Task CargarListaAreas()
        {
            var areas = await _funcionariosService.ListarAreasAsync();
            ListaAreas = areas.Select(a => new SelectListItem
            {
                Value = a.ID_Area,
                Text = a.Nombre_Area
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Recupera identificación del TempData por seguridad
            if (string.IsNullOrEmpty(FuncionarioArea.Identificacion) && TempData.ContainsKey("Identificacion"))
            {
                FuncionarioArea.Identificacion = TempData["Identificacion"]?.ToString();
            }

            if (!ModelState.IsValid)
            {
                TempData["CreateTitle"] = "Operación Fallida";
                TempData["CreateMessage"] = "Existen errores en el formulario.";
                await CargarListaAreas();
                return Page();
            }

            var (resultado, mensaje) = await _funcionariosService.InsertarFuncionarioAreaAsync(FuncionarioArea);

            TempData["CreateTitle"] = resultado ? "Operación Exitosa" : "Operación Fallida";
            TempData["CreateMessage"] = mensaje;

            if (!resultado)
            {
                await CargarListaAreas();
                return Page();
            }

            // ? Redirige al listado de áreas del mismo funcionario
            return RedirectToPage("Funcionario_Areas", new { identificacion = FuncionarioArea.Identificacion });
        }
    }
}
