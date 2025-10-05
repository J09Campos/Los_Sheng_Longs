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


        //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------
        Task<string> Crear_ContrasenaAsync();

        Task<(bool Resultado, string Mensaje)> Cambiar_ContrasenaAsync(string identificacion, string contrasena);
        //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------

        //-------------------------------------------------------------------------Areas Funcionarios------------------------------------------------------------
        Task<IEnumerable<Funcionarios_Areas>> ListarFuncionariosAreasAsync(string identificacion);
        Task<IEnumerable<Funcionarios_Areas>> ListarAreasAsync();
        Task<(bool resultado, string mensaje)> InsertarFuncionarioAreaAsync(Funcionarios_Areas funcionarioArea);
        Task<(bool Resultado, string Mensaje)> EliminarAsociacionAsync(Funcionarios_Areas funcionarioArea);
        //-------------------------------------------------------------------------Areas Funcionarios------------------------------------------------------------

    }
}
