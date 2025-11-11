using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reloj_Marcador.Entities;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IInconsistenciasService
    {
        Task<IEnumerable<Inconsistencias>> GetAllAsync();

        Task<Inconsistencias?> GetByIdAsync(int id);

        Task<(bool Resultado, string Mensaje)> CRUDAsync(Inconsistencias inconsistencia, string accion);

        Task<IEnumerable<Inconsistencias>> ListarInconsistenciasAsync(
            DateTime? inicio,
            DateTime? fin,
            string? area,
            string? funcionario
        );
    }
}
