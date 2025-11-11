using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public interface IProc1Service
    {
        /// <summary>
        /// Ejecuta el proceso PROC1 manualmente.
        /// </summary>
        Task EjecutarAsync(DateTime inicio, DateTime fin, string? area = null, string? funcionario = null);

        /// <summary>
        /// Devuelve la bitácora de ejecuciones de PROC1.
        /// </summary>
        Task<IEnumerable<BitacoraProc1>> ListarBitacoraAsync();

        /// <summary>
        /// Devuelve las inconsistencias generadas (filtradas por rango, área o funcionario).
        /// </summary>
        Task<IEnumerable<Inconsistencias>> ListarInconsistenciasAsync(DateTime? inicio = null, DateTime? fin = null, string? area = null, string? funcionario = null);
    }
}
