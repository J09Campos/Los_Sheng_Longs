using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Reloj_Marcador.Pages.Horarios
{
    public class IndexModel : PageModel
    {
        private readonly IHorarioService _horarioService;
        private readonly IAreaService _areaService;
        private readonly IDetalleHorarioService _detalleService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IHorarioService horarioService,
            IAreaService areaService,
            IDetalleHorarioService detalleService,
            ILogger<IndexModel> logger)
        {
            _horarioService = horarioService;
            _areaService = areaService;
            _detalleService = detalleService;
            _logger = logger;

            Horarios = new List<Horario>();
            AreasDisponibles = new List<Area>();
            Detalles = new List<DetalleHorario>();
        }

        [BindProperty]
        public string FuncionarioSeleccionado { get; set; }

        public IEnumerable<Horario> Horarios { get; set; }
        public IEnumerable<Area> AreasDisponibles { get; set; }

        [BindProperty]
        public Horario NuevoHorario { get; set; } = new Horario();

        [BindProperty]
        public DetalleHorario NuevoDetalle { get; set; } = new DetalleHorario();

        public Horario HorarioSeleccionado { get; set; } = new Horario();
        public IEnumerable<DetalleHorario> Detalles { get; set; }

        public async Task OnGetAsync(int? idHorario = null)
        {
            Horarios = await _horarioService.ListarHorariosPorFuncionario(FuncionarioSeleccionado) ?? new List<Horario>();

            AreasDisponibles = await _areaService.GetAllAsync() ?? new List<Area>();

            if (idHorario.HasValue && idHorario > 0)
            {
                HorarioSeleccionado = await _horarioService.GetByIdAsync(idHorario.Value) ?? new Horario();

                if (HorarioSeleccionado.ID_Horario > 0)
                {
                    Detalles = await _detalleService.ListarDetallesPorHorario(idHorario.Value) ?? new List<DetalleHorario>();
                }
                else
                {
                    
                    HorarioSeleccionado = new Horario();
                    Detalles = new List<DetalleHorario>();
                }
            }
            else
            {

                HorarioSeleccionado = new Horario();
                Detalles = new List<DetalleHorario>();

            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            if (string.IsNullOrEmpty(NuevoHorario.ID_Area) || string.IsNullOrEmpty(NuevoHorario.Descripcion?.Trim()))
            {
                ModelState.AddModelError("", "Área y Descripción son requeridos y no pueden estar vacíos.");
                await OnGetAsync();
                return Page();
            }

            NuevoHorario.ID_Funcionario = FuncionarioSeleccionado;

            try
            {
                var rowsAffected = await _horarioService.InsertAsync(NuevoHorario);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Horario guardado exitosamente.";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo guardar. Posible duplicado (ya existe para esta área) o error en datos.");
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var rows = await _horarioService.DeleteAsync(id);
                if (rows > 0)
                {
                    TempData["Success"] = "Horario eliminado.";
                }
                else
                {
                    TempData["Error"] = "No se eliminó (no encontrado).";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAgregarDetalleAsync(int? idHorario)
        {
            if (!idHorario.HasValue || idHorario <= 0)
            {
                ModelState.AddModelError("", "ID de Horario inválido. Cree un horario primero.");
                await OnGetAsync();
                return Page();
            }

           
            var keysToRemove = ModelState.Keys.Where(k =>
                k.StartsWith("NuevoHorario.") ||
                k == "NuevoHorario" ||
                k == "ID_Area" ||
                k == "ID_Funcionario" ||
                k == "Descripcion"
            ).ToList();
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync(idHorario.Value);
                return Page();
            }

            if (string.IsNullOrEmpty(NuevoDetalle.Dia?.Trim()))
            {
                ModelState.AddModelError("", "Día es requerido.");
                await OnGetAsync(idHorario.Value);
                return Page();
            }

            if (NuevoDetalle.Hora_Ingreso < 0 || NuevoDetalle.Hora_Ingreso > 23 ||
                NuevoDetalle.Minuto_Ingreso < 0 || NuevoDetalle.Minuto_Ingreso > 59 ||
                NuevoDetalle.Hora_Salida < 0 || NuevoDetalle.Hora_Salida > 23 ||
                NuevoDetalle.Minuto_Salida < 0 || NuevoDetalle.Minuto_Salida > 59)
            {
                ModelState.AddModelError("", "Horas: 0-23, Minutos: 0-59.");
                await OnGetAsync(idHorario.Value);
                return Page();
            }

            NuevoDetalle.Id_Horario = idHorario.Value;

            try
            {
                var rowsAffected = await _detalleService.AgregarDetalleHorario(NuevoDetalle);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Detalle agregado exitosamente.";
                    return RedirectToPage(new { idHorario = idHorario.Value });
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo agregar. Verifique duplicados por día o horario válido.");
                    await OnGetAsync(idHorario.Value);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al agregar: {ex.Message}");
                await OnGetAsync(idHorario.Value);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEliminarDetalleAsync(int id, int? idHorario)
        {
            if (id <= 0 || !idHorario.HasValue || idHorario <= 0)
            {
                TempData["Error"] = "IDs inválidos.";
                return RedirectToPage();
            }

            try
            {
                var rows = await _detalleService.EliminarDetalleHorario(id);
                if (rows > 0)
                {
                    TempData["Success"] = "Detalle eliminado.";
                }
                else
                {
                    TempData["Error"] = "No se eliminó.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToPage(new { idHorario = idHorario.Value });
        }
    }
}