using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services
{
    public class HorarioService : IHorarioService
    {
        private readonly HorarioRepository _horarioRepository;

        public HorarioService(HorarioRepository horarioRepository)
        {
            _horarioRepository = horarioRepository;
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
            return await _horarioRepository.InsertAsync(horario);
        }

        public async Task<int> UpdateAsync(Horario horario)
        {
            return await _horarioRepository.UpdateAsync(horario);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _horarioRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Horario>> ListarHorariosPorFuncionario(string idFuncionario)
        {
            return await _horarioRepository.GetByFuncionarioAsync(idFuncionario);
        }
    }
}
