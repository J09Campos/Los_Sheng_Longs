using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services;

public class FuncionariosServices : IFuncionariosService
{
    private readonly FuncionariosRepository _funcionarioRepository;

    public FuncionariosServices(FuncionariosRepository funcionarioRepository)
    {
        _funcionarioRepository = funcionarioRepository;
    }

    public async Task<IEnumerable<Funcionarios_Usuarios>> ListarAsync()
    {
        return await _funcionarioRepository.ListarAsync();
    }

    public async Task<Funcionarios_Usuarios?> ObtenerPorIdAsync(string identificacion)
    {
        return await _funcionarioRepository.ObtenerPorIdAsync(identificacion);
    }

    public async Task<int> CrearAsync(Funcionarios_Usuarios funcionario)
    {
        if (string.IsNullOrEmpty(funcionario.Identificacion))
            throw new ArgumentException("La identificación es obligatoria");

        return await _funcionarioRepository.CrearAsync(funcionario);
    }

    public async Task<int> ActualizarAsync(Funcionarios_Usuarios funcionario)
    {
        return await _funcionarioRepository.ActualizarAsync(funcionario);
    }

    public async Task<int> EliminarAsync(string identificacion)
    {
        return await _funcionarioRepository.EliminarAsync(identificacion);
    }

    //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------

    public async Task<string> Crear_ContrasenaAsync()
    {
        return await _funcionarioRepository.Crear_ContrasenaAsync();
    }

    public async Task<(bool Resultado, string Mensaje)> Cambiar_ContrasenaAsync(string identificacion, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(identificacion))
           return (false, "La identificación no debe ser nula o vacía.");

        if (string.IsNullOrWhiteSpace(contrasena))
          return (false, "La contraseña no debe ser nula o vacía.");

        if (contrasena.Length > 50)
           return (false, "La contraseña no debe superar los 50 caracteres.");


        try
        {
            var resultado = await _funcionarioRepository.Cambiar_ContrasenaAsync(identificacion, contrasena);

            if (resultado > 0)
            {
                return (true, "La contraseña se cambió correctamente.");
            }
            else
            {
                return (false, "No se pudo cambiar la contraseña. Verifique los datos.");
            }
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {

            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
    //-------------------------------------------------------------------------Cambio de contraseña para funcionarios/usuarios------------------------------------------------------------




    //-------------------------------------------------------------------------Areas para funcionarios/usuarios------------------------------------------------------------
    public async Task<IEnumerable<Funcionarios_Areas>> ListarFuncionariosAreasAsync(string identificacion)
    {
        return await _funcionarioRepository.ListarFuncionariosAreasAsync(identificacion);
    }

    public async Task<IEnumerable<Funcionarios_Areas>> ListarAreasAsync()
    {
        return await _funcionarioRepository.ListarAreasAsync();
    }

    public async Task<(bool resultado, string mensaje)> InsertarFuncionarioAreaAsync(Funcionarios_Areas funcionarioArea)
    {
        try
        {
            // Validaciones básicas antes de llamar al repositorio
            if (string.IsNullOrWhiteSpace(funcionarioArea.Identificacion))
                return (false, "La identificación del funcionario es obligatoria.");

            if (string.IsNullOrWhiteSpace(funcionarioArea.Identificacion))
                return (false, "La identificación del funcionario es obligatoria.");

            if (funcionarioArea.Identificacion.Length > 22)
                return (false, "La identificación del funcionario no debe ser mayor a 22 caracteres.");

            // Llamar al método del repositorio que ejecuta el SP
            var resultado = await _funcionarioRepository.InsertarFuncionarioAreaAsync(funcionarioArea);

            return resultado;
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {

            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, $"Error inesperado al insertar la relación: {ex.Message}");
        }

    }

    public async Task<(bool Resultado, string Mensaje)> EliminarAsociacionAsync(Funcionarios_Areas funcionarioArea)
    {
        if (funcionarioArea.ID_Funcionario_Area == null)
            return (false, "El ID de la asociación no es válido.");

        try
        {
            var resultado = await _funcionarioRepository.EliminarAsociacionAsync(funcionarioArea);
            return resultado;
        }
        catch (Exception ex)
        {
            return (false, $"Error inesperado: {ex.Message}");
        }
    }

    //-------------------------------------------------------------------------Areas para funcionarios/usuarios------------------------------------------------------------

}


