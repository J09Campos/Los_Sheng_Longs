using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Reloj_Marcador.Services
{
    public class HorarioService : IHorarioService
    {
        private readonly HorarioRepository _horarioRepository;
        private readonly IBitacoraService _bitacoraService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HorarioService(
            HorarioRepository horarioRepository,
            IBitacoraService bitacoraService,
            IHttpContextAccessor httpContextAccessor)
        {
            _horarioRepository = horarioRepository;
            _bitacoraService = bitacoraService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            return await _horarioRepository.GetAllAsync();
        }

        public async Task<Horario?> GetByIdAsync(int id)
        {
            return await _horarioRepository.GetByIdAsync(id);
        }

        public async Task<int> InsertAsync(Horario horario)
        {
            var resultado = await _horarioRepository.InsertAsync(horario);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se creó un nuevo Horario", new
                {
                    horario.ID_Horario,
                    horario.ID_Funcionario,
                    horario.ID_Area,
                    horario.Descripcion,
                    horario.Fecha_Creacion
                });
            }

            return resultado;
        }

        public async Task<int> UpdateAsync(Horario horario)
        {
            var horarioAnterior = await _horarioRepository.GetByIdAsync(horario.ID_Horario);
            if (horarioAnterior == null)
                throw new ArgumentException("El horario no existe.");

            var resultado = await _horarioRepository.UpdateAsync(horario);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se actualizó un Horario", new
                {
                    Antes = new
                    {
                        horarioAnterior.ID_Horario,
                        horarioAnterior.ID_Funcionario,
                        horarioAnterior.ID_Area,
                        horarioAnterior.Descripcion,
                        horarioAnterior.Fecha_Creacion
                    },
                    Ahora = new
                    {
                        horario.ID_Horario,
                        horario.ID_Funcionario,
                        horario.ID_Area,
                        horario.Descripcion,
                        horario.Fecha_Creacion
                    }
                });
            }

            return resultado;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var horarioAnterior = await _horarioRepository.GetByIdAsync(id);
            if (horarioAnterior == null)
                throw new ArgumentException("El horario no existe.");

            var resultado = await _horarioRepository.DeleteAsync(id);

            if (resultado > 0)
            {
                string usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anónimo";

                await _bitacoraService.RegistrarAsync(usuario, "Se eliminó un Horario", new
                {
                    horarioAnterior.ID_Horario,
                    horarioAnterior.ID_Funcionario,
                    horarioAnterior.ID_Area,
                    horarioAnterior.Descripcion,
                    horarioAnterior.Fecha_Creacion
                });
            }

            return resultado;
        }

        public async Task<IEnumerable<Horario>> ListarHorariosPorFuncionario(string idFuncionario)
        {
            return await _horarioRepository.GetByFuncionarioAsync(idFuncionario);
        }
    }
}


