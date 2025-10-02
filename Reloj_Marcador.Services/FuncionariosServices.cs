using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}


