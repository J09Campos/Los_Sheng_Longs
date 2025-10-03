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
                // Si hay error, recargamos combos
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }

            // Guardar nuevo funcionario
            await _funcionariosService.CrearAsync(Funcionario);

            return RedirectToPage("Index");
        }
    }
}
