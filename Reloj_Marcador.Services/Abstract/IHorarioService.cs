using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IHorarioService
    {
        Task<IEnumerable<Horario>> GetAllAsync();
        Task<Horario?> GetByIdAsync(int id);
        Task<int> InsertAsync(Horario horario);
        Task<int> UpdateAsync(Horario horario);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Horario>> ListarHorariosPorFuncionario(string idFuncionario);
    }
}
