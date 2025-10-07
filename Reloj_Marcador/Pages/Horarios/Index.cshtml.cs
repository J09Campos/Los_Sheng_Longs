using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
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
        }

        [BindProperty(SupportsGet = true)]
        public string FuncionarioSeleccionado { get; set; }

        public IEnumerable<Horario> Horarios { get; set; } = new List<Horario>();
        public IEnumerable<Area> AreasDisponibles { get; set; } = new List<Area>();
        public IEnumerable<DetalleHorario> Detalles { get; set; } = new List<DetalleHorario>();
        public Horario HorarioSeleccionado { get; set; } = new Horario();

        [BindProperty] public Horario NuevoHorario { get; set; } = new Horario();
        [BindProperty] public DetalleHorario NuevoDetalle { get; set; } = new DetalleHorario();

        public async Task OnGetAsync(int? idHorario = null, string? funcionarioId = null)
        {
            try
            {
                FuncionarioSeleccionado ??= funcionarioId;

                if (string.IsNullOrWhiteSpace(FuncionarioSeleccionado))
                {
                    TempData["Error"] = "Debe seleccionar un funcionario v lido.";
                    return;
                }

                Horarios = await _horarioService.ListarHorariosPorFuncionario(FuncionarioSeleccionado) ?? new List<Horario>();
                AreasDisponibles = await _areaService.GetAllAsync() ?? new List<Area>();

                if (idHorario.HasValue && idHorario > 0)
                {
                    HorarioSeleccionado = await _horarioService.GetByIdAsync(idHorario.Value) ?? new Horario();
                    if (HorarioSeleccionado.ID_Horario > 0)
                        Detalles = await _detalleService.ListarDetallesPorHorario(idHorario.Value) ?? new List<DetalleHorario>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los horarios.");
                TempData["Error"] = "Ocurri  un error al cargar los datos. Int ntelo nuevamente.";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FuncionarioSeleccionado))
                {
                    TempData["Error"] = "Debe seleccionar un funcionario antes de crear un horario.";
                    return RedirectToPage();
                }

                if (string.IsNullOrWhiteSpace(NuevoHorario.Descripcion))
                {
                    TempData["Error"] = "La descripci n del horario es obligatoria.";
                    return RedirectToPage(new { FuncionarioSeleccionado });
                }

                NuevoHorario.ID_Funcionario = FuncionarioSeleccionado;
                NuevoHorario.Fecha_Creacion = DateTime.Now;

                var rows = await _horarioService.InsertAsync(NuevoHorario);
                TempData["Success"] = rows > 0
                    ? "Horario guardado exitosamente."
                    : "No se pudo guardar el horario. Verifique los datos ingresados.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear horario.");
                TempData["Error"] = "Ocurri  un error al crear el horario. Intente nuevamente m s tarde.";
            }

            return RedirectToPage(new { FuncionarioSeleccionado });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de horario no v lido.";
                return RedirectToPage(new { FuncionarioSeleccionado });
            }

            try
            {
                var rows = await _horarioService.DeleteAsync(id);
                TempData["Success"] = rows > 0
                    ? "Horario eliminado correctamente."
                    : "No se elimin  el horario.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar horario ID {Id}", id);

                if (ex.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
                    TempData["Error"] = "No se puede eliminar el horario porque tiene detalles asociados.";
                else
                    TempData["Error"] = "Ocurri  un error al eliminar el horario. Intente nuevamente.";
            }

            return RedirectToPage(new { FuncionarioSeleccionado });
        }

        public async Task<IActionResult> OnPostAgregarDetalleAsync(int? idHorario)
        {
            if (!idHorario.HasValue || idHorario <= 0)
            {
                TempData["Error"] = "Debe seleccionar un horario v lido.";
                return RedirectToPage(new { FuncionarioSeleccionado });
            }

            try
            {
                var horaInicio = new TimeSpan(NuevoDetalle.Hora_Ingreso, NuevoDetalle.Minuto_Ingreso, 0);
                var horaFin = new TimeSpan(NuevoDetalle.Hora_Salida, NuevoDetalle.Minuto_Salida, 0);

                if (horaFin <= horaInicio)
                {
                    TempData["Error"] = "La hora de salida debe ser mayor a la de ingreso.";
                    return RedirectToPage(new { idHorario, FuncionarioSeleccionado });
                }

                NuevoDetalle.Id_Horario = idHorario.Value;

                var rows = await _detalleService.AgregarDetalleHorario(NuevoDetalle);
                TempData["Success"] = rows > 0
                    ? "Detalle agregado correctamente."
                    : "No se pudo agregar el detalle. Verifique los datos ingresados.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar detalle al horario {Id}", idHorario);
                TempData["Error"] = "Ocurri  un error al agregar el detalle. Intente nuevamente m s tarde.";
            }

            return RedirectToPage(new { idHorario, FuncionarioSeleccionado });
        }

        public async Task<IActionResult> OnPostEliminarDetalleAsync(int id, int idHorario)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de detalle no v lido.";
                return RedirectToPage(new { idHorario, FuncionarioSeleccionado });
            }

            try
            {
                var rows = await _detalleService.EliminarDetalleHorario(id);
                TempData["Success"] = rows > 0
                    ? "Detalle eliminado correctamente."
                    : "No se elimin  el detalle.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar detalle con ID {Id}", id);
                TempData["Error"] = "Ocurri  un error al eliminar el detalle. Intente nuevamente.";
            }

            return RedirectToPage(new { idHorario, FuncionarioSeleccionado });
        }
    }
}
