using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;

namespace Reloj_Marcador.Services
{
    public class Proc1Service : IProc1Service
    {
        private readonly Proc1Repository _repo;

        public Proc1Service(Proc1Repository repo)
        {
            _repo = repo;
        }

        public async Task EjecutarAsync(DateTime inicio, DateTime fin, string? area = null, string? funcionario = null)
        {
            await _repo.EjecutarProcesoAsync(inicio, fin, area, funcionario);
        }

        public async Task<IEnumerable<BitacoraProc1>> ListarBitacoraAsync()
        {
            return await _repo.ListarBitacoraAsync();
        }

        public async Task<IEnumerable<Inconsistencias>> ListarInconsistenciasAsync(DateTime? inicio = null, DateTime? fin = null, string? area = null, string? funcionario = null)
        {
            return await _repo.ListarInconsistenciasAsync(inicio, fin, area, funcionario);
        }





    }
}
