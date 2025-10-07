using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reloj_Marcador.Pages.Motivos
{
    public class IndexModel : PageModel
    {
        private readonly IMotivoService _motivoService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IMotivoService motivoService, ILogger<IndexModel> logger)
        {
            _motivoService = motivoService;
            _logger = logger;
        }

        public IEnumerable<Motivo> Motivos { get; set; } = new List<Motivo>();

        [BindProperty] public Motivo NuevoMotivo { get; set; } = new Motivo();
        [BindProperty] public Motivo MotivoEnEdicion { get; set; } = new Motivo();

        public async Task OnGetAsync()
        {
            try
            {
                Motivos = await _motivoService.GetAllAsync() ?? new List<Motivo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los motivos.");
                TempData["Error"] = "No se pudieron cargar los motivos. Intente nuevamente m s tarde.";
            }
        }

        public async Task<IActionResult> OnPostCrearMotivoAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NuevoMotivo.Nombre_Motivo))
                {
                    TempData["Error"] = "El nombre del motivo es obligatorio.";
                    return RedirectToPage();
                }

                var result = await _motivoService.InsertAsync(NuevoMotivo);

                if (result > 0)
                    TempData["Success"] = "Motivo creado correctamente.";
                else
                    TempData["Error"] = "No se pudo crear el motivo. Verifique los datos.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear motivo.");

                if (ex.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                    TempData["Error"] = "Ya existe un motivo con ese nombre.";
                else
                    TempData["Error"] = "Ocurri  un error al crear el motivo. Intente nuevamente.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarMotivoAsync(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de motivo no v lido.";
                return RedirectToPage();
            }

            try
            {
                var motivo = await _motivoService.GetByIdAsync(id);
                if (motivo == null)
                {
                    TempData["Error"] = "No se encontr  el motivo a editar.";
                    return RedirectToPage();
                }

                if (string.IsNullOrWhiteSpace(MotivoEnEdicion.Nombre_Motivo))
                {
                    TempData["Error"] = "El nombre del motivo no puede estar vac o.";
                    return RedirectToPage();
                }

                motivo.Nombre_Motivo = MotivoEnEdicion.Nombre_Motivo;
                var rows = await _motivoService.UpdateAsync(motivo);

                TempData["Success"] = rows > 0
                    ? "Motivo actualizado correctamente."
                    : "No se pudo actualizar el motivo.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar motivo ID {Id}", id);
                TempData["Error"] = "Ocurri  un error al actualizar el motivo. Intente nuevamente.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarMotivoAsync(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de motivo no v lido.";
                return RedirectToPage();
            }

            try
            {
                var rows = await _motivoService.DeleteAsync(id);

                TempData["Success"] = rows > 0
                    ? "Motivo eliminado correctamente."
                    : "No se pudo eliminar el motivo.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar motivo ID {Id}", id);

                if (ex.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
                    TempData["Error"] = "No se puede eliminar el motivo porque est  asociado a otros registros.";
                else
                    TempData["Error"] = "Ocurri  un error al eliminar el motivo. Intente nuevamente.";
            }

            return RedirectToPage();
        }
    }
}

