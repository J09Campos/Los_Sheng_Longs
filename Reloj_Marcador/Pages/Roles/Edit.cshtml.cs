using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Roles
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IRolesService _rolesService;

        public EditModel(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [BindProperty]
        public Rol Rol { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Rol = await _rolesService.ObtenerPorIdAsync(id);

            if (Rol == null)
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _rolesService.ActualizarAsync(Rol);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["CreateTitle6"] = "Operación Fallida";
                TempData["CreateMessage6"] = ex.Message;
                return Page();
            }
        }
    }
}


