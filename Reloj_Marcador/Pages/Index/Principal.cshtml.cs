using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reloj_Marcador.Pages.Index
{
    [Authorize]
    public class PrincipalModel : PageModel
    {

        public void OnGet()
        {
        }
    }
}
