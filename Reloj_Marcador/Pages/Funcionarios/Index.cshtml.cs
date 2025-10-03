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

        public async Task OnGetAsync()
        {
            Funcionarios = await _funcionariosService.ListarAsync();
        }
    }
}
