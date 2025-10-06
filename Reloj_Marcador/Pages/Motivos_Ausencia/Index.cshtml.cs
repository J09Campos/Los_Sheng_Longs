using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reloj_Marcador.Pages.Motivos
{
    public class IndexModel : PageModel
    {
        private readonly IMotivoService _motivoService;

        public IndexModel(IMotivoService motivoService)
        {
            _motivoService = motivoService;
        }

        public IEnumerable<Motivo> Motivos { get; set; } = new List<Motivo>();

        [BindProperty]
        public Motivo NuevoMotivo { get; set; } = new Motivo();

        [BindProperty]
        public Motivo MotivoEnEdicion { get; set; } = new Motivo();

    
        public async Task OnGetAsync()
        {
            Motivos = await _motivoService.GetAllAsync();
        }


        public async Task<IActionResult> OnPostCrearMotivoAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    Motivos = await _motivoService.GetAllAsync();
            //    return Page();
            //}

            await _motivoService.InsertAsync(NuevoMotivo);
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostEditarMotivoAsync(int id)
        {
            var motivo = await _motivoService.GetByIdAsync(id);
            if (motivo == null) return NotFound();

            motivo.Nombre_Motivo = MotivoEnEdicion.Nombre_Motivo;
            await _motivoService.UpdateAsync(motivo);

            return RedirectToPage();
        }

    
        public async Task<IActionResult> OnPostEliminarMotivoAsync(int id)
        {
            await _motivoService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
