using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Pages.Roles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRolesService _rolesService;

        public IndexModel(IRolesService rolesService)
        {
            _rolesService = rolesService;
            Roles = new List<Rol>();
        }

        public IEnumerable<Rol> Roles { get; set; }

        [BindProperty]
        public string ID_Rol { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allRoles = await _rolesService.ListarAsync();

            int totalRecords = allRoles.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            CurrentPage = pageNumber;

            Roles = allRoles
                .OrderBy(r => r.ID_Rol)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string IdRol)
        {
            try
            {
                await _rolesService.EliminarAsync(IdRol);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["CreateMessage10"] = ex.Message;
                return RedirectToPage();
            }
        }
    }
}
