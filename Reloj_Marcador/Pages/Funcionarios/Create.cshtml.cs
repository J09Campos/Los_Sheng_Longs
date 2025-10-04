using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class CreateModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public CreateModel(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
            Funcionario = new Funcionarios_Usuarios();

            // Inicializamos listas para evitar nulls
            TiposIdentificacion = new List<TipoIdentificacion>();
            Roles = new List<Rol>();
            Estados = new List<Estado>();
        }

        [BindProperty]
        public Funcionarios_Usuarios Funcionario { get; set; }

        // Listas para combos
        public IEnumerable<TipoIdentificacion> TiposIdentificacion { get; set; }
        public IEnumerable<Rol> Roles { get; set; }
        public IEnumerable<Estado> Estados { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Cargar combos desde el servicio
            TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
            Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
            Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Si hay error de validación, recargamos combos
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }

            try
            {
                // Intentar guardar
                await _funcionariosService.CrearAsync(Funcionario);
                TempData["CreateMessage1"] = "Funcionario creado con éxito.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // Mostrar error en la interfaz
                //ModelState.AddModelError(string.Empty, ex.Message);
                TempData["CreateTitle3"] = "Operación Fallida";
                TempData["CreateMessage3"] = ex.Message;

                // Recargar combos antes de volver a la vista
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }
        }
    }
}
