using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Roles
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IRolesService _rolesService;

        public CreateModel(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [BindProperty]
        public Rol Rol { get; set; } = new Rol();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _rolesService.CrearAsync(Rol);
                TempData["CreateTitle10"] = "Operación Exitosa.";
                TempData["CreateMessage10"] = "Rol creado con éxito.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["CreateTitle5"] = "Operación Fallida.";
                TempData["CreateMessage5"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}

