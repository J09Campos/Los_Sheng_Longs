using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IDetalleHorarioService
    {
        Task<IEnumerable<DetalleHorario>> ListarDetallesPorHorario(int idHorario);
        Task<DetalleHorario?> GetByIdAsync(int id);
        Task<int> AgregarDetalleHorario(DetalleHorario detalle);
        Task<int> ActualizarDetalleHorario(DetalleHorario detalle);
        Task<int> EliminarDetalleHorario(int id);
    }
}