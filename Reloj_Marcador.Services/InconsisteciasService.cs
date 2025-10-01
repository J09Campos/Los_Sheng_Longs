using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class InconsisteciasService : IInconsistenciasService
    {
        private readonly InconsistenciasRepository _inconsisteciasRepository;

        public InconsisteciasService(InconsistenciasRepository inconsisteciasRepository)
        {
            _inconsisteciasRepository = inconsisteciasRepository;
        }
        public async Task<Entities.Inconsistencias?> GetByIdAsync(string id)
        {
            return await _inconsisteciasRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Entities.Inconsistencias>> GetAllAsync()
        {
            return await _inconsisteciasRepository.GetAllAsync();
        }
        public async Task<(bool Resultado, string Mensaje)> CRUDAsync(Entities.Inconsistencias inconsistencia, string accion)
        {
            return await _inconsisteciasRepository.CRUDAsync(inconsistencia, accion);
        }
    }
}
