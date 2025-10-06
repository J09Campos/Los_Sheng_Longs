using Reloj_Marcador.Entities;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        // Necesario para mantener el valor entre GET y POST
        [BindProperty(SupportsGet = true)]
        public string FuncionarioSeleccionado { get; set; } = string.Empty;

        public IEnumerable<Horario> Horarios { get; set; }
        public IEnumerable<Area> AreasDisponibles { get; set; }

        [BindProperty]
        public Horario NuevoHorario { get; set; } = new Horario();

        [BindProperty]
        public DetalleHorario NuevoDetalle { get; set; } = new DetalleHorario();

        public Horario HorarioSeleccionado { get; set; } = new Horario();
        public IEnumerable<DetalleHorario> Detalles { get; set; }

        public async Task OnGetAsync(string funcionarioSeleccionado, int? idHorario = null)
        {
            if (!string.IsNullOrEmpty(funcionarioSeleccionado))
                FuncionarioSeleccionado = funcionarioSeleccionado;

            await CargarDatosAsync(idHorario);
        }

        private async Task CargarDatosAsync(int? idHorario)
        {
            Horarios = await _horarioService.ListarHorariosPorFuncionario(FuncionarioSeleccionado) ?? new List<Horario>();
            AreasDisponibles = await _areaService.GetAllAsync() ?? new List<Area>();

            if (idHorario.HasValue && idHorario > 0)
            {
                HorarioSeleccionado = await _horarioService.GetByIdAsync(idHorario.Value) ?? new Horario();
                Detalles = await _detalleService.ListarDetallesPorHorario(idHorario.Value) ?? new List<DetalleHorario>();
            }
            else
            {
                HorarioSeleccionado = new Horario();
                Detalles = new List<DetalleHorario>();
            }
        }

        // --- Crear Horario ---
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarDatosAsync(null);
                TempData["Error"] = "Formulario inválido. Revise los campos.";
                return Page();
            }

            if (string.IsNullOrEmpty(NuevoHorario.ID_Area) || string.IsNullOrWhiteSpace(NuevoHorario.Descripcion))
            {
                TempData["Error"] = "Área y Descripción son obligatorios.";
                return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado });
            }

            NuevoHorario.ID_Funcionario = FuncionarioSeleccionado;

            try
            {
                var rowsAffected = await _horarioService.InsertAsync(NuevoHorario);
                TempData[rowsAffected > 0 ? "Success" : "Error"] =
                    rowsAffected > 0 ? "Horario creado correctamente." : "No se pudo guardar el horario.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado });
        }

        // --- Eliminar Horario ---
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var rows = await _horarioService.DeleteAsync(id);
                TempData[rows > 0 ? "Success" : "Error"] =
                    rows > 0 ? "Horario eliminado correctamente." : "No se pudo eliminar el horario (tiene detalles asociados).";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Message.Contains("foreign key constraint fails"))
                    TempData["Error"] = "No se puede eliminar un horario con detalles asociados.";
                else
                    TempData["Error"] = $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error inesperado: {ex.Message}";
            }

            return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado });
        }

        // --- Agregar Detalle ---
        public async Task<IActionResult> OnPostAgregarDetalleAsync(int idHorario)
        {
            if (idHorario <= 0)
            {
                TempData["Error"] = "Debe seleccionar un horario válido.";
                return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado });
            }

            if (string.IsNullOrWhiteSpace(NuevoDetalle.Dia))
            {
                TempData["Error"] = "El campo Día es obligatorio.";
                return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado, idHorario });
            }

            NuevoDetalle.Id_Horario = idHorario;

            try
            {
                var rowsAffected = await _detalleService.AgregarDetalleHorario(NuevoDetalle);
                TempData[rowsAffected > 0 ? "Success" : "Error"] =
                    rowsAffected > 0 ? "Detalle agregado correctamente." : "No se pudo agregar el detalle (posible duplicado).";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado, idHorario });
        }

        // --- Eliminar Detalle ---
        public async Task<IActionResult> OnPostEliminarDetalleAsync(int id, int idHorario)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID de detalle inválido.";
                return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado, idHorario });
            }

            try
            {
                var rows = await _detalleService.EliminarDetalleHorario(id);
                TempData[rows > 0 ? "Success" : "Error"] =
                    rows > 0 ? "Detalle eliminado correctamente." : "No se pudo eliminar el detalle.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToPage(new { funcionarioSeleccionado = FuncionarioSeleccionado, idHorario });
        }
    }
}



