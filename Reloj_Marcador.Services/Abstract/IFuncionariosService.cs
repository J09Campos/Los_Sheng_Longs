using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IFuncionariosService
    {

        Task<IEnumerable<Funcionarios_Usuarios>> ListarAsync();
        Task<Funcionarios_Usuarios?> ObtenerPorIdAsync(string identificacion);
        Task<int> CrearAsync(Funcionarios_Usuarios funcionario);
        Task<int> ActualizarAsync(Funcionarios_Usuarios funcionario);
        Task<int> EliminarAsync(string identificacion);
        Task<IEnumerable<TipoIdentificacion>> ObtenerTiposIdentificacionAsync();
        Task<IEnumerable<Rol>> ObtenerRolesAsync();
        Task<IEnumerable<Estado>> ObtenerEstadosAsync();

    }
}
