using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public CreateModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionario = new Funcionarios_Usuarios();

            TiposIdentificacion = new List<TipoIdentificacion>();
            Roles = new List<Rol>();
            Estados = new List<Estado>();
        }

        [BindProperty]
        public Funcionarios_Usuarios Funcionario { get; set; }

        public IEnumerable<TipoIdentificacion> TiposIdentificacion { get; set; }
        public IEnumerable<Rol> Roles { get; set; }
        public IEnumerable<Estado> Estados { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
            Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
            Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }

            try
            {
                await _funcionariosService.CrearAsync(Funcionario);
                TempData["CreateTitle1"] = "Operación Exitosa.";
                TempData["CreateMessage1"] = "Funcionario creado con éxito.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["CreateTitle3"] = "Operación Fallida.";
                TempData["CreateMessage3"] = ex.Message;

                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }
        }
    }
}
