using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Pages.Marca_Entrada_Salida;

public class IndexModel : PageModel
{
    public readonly Services.Abstract.IMarcasService _marcasService;

    public IndexModel(Services.Abstract.IMarcasService marcasService)
    {
        _marcasService = marcasService;
        Marca = new Marcas();

    }
    // Propiedad para cargar opciones en el combo
    public SelectList Areas { get; set; }

    // Propiedad para el valor seleccionado
    [BindProperty]
    public Marcas Marca { get; set; }

    public async Task OnGetAsync(string id = "305690396")
    {
        var lista = await _marcasService.GetAllAreaByID(id);
        Areas = new SelectList(lista);
    }
}
