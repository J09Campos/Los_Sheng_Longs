using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System.Data;

namespace Reloj_Marcador.Pages.Funcionarios
{
    public class EditModel : PageModel
    {
        private readonly IFuncionariosService _funcionariosService;

        public EditModel(IFuncionariosService funcionariosService)
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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Funcionario = await _funcionariosService.ObtenerPorIdAsync(id);

            TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
            Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
            Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Recargar combos si el modelo no es válido
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }

            try
            {
                // Intentar actualizar funcionario
                await _funcionariosService.ActualizarAsync(Funcionario);
                TempData["CreateMessage"] = "Funcionario actualizado con éxito.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // Mostrar error en la interfaz
                ModelState.AddModelError(string.Empty, ex.Message);

                // Recargar combos antes de devolver la página
                TiposIdentificacion = await _funcionariosService.ObtenerTiposIdentificacionAsync() ?? new List<TipoIdentificacion>();
                Roles = await _funcionariosService.ObtenerRolesAsync() ?? new List<Rol>();
                Estados = await _funcionariosService.ObtenerEstadosAsync() ?? new List<Estado>();

                return Page();
            }
        }
    }
}

