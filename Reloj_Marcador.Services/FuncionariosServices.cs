using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;
using System.Text.RegularExpressions;

namespace Reloj_Marcador.Services;

public class FuncionariosServices : IFuncionariosService
{
    private readonly FuncionariosRepository _funcionarioRepository;

    public FuncionariosServices(FuncionariosRepository funcionarioRepository)
    {
        _funcionarioRepository = funcionarioRepository;
    }

    public async Task<IEnumerable<Funcionarios_Usuarios>> ListarAsync()
        => await _funcionarioRepository.ListarAsync();

    public async Task<Funcionarios_Usuarios?> ObtenerPorIdAsync(string identificacion)
        => await _funcionarioRepository.ObtenerPorIdAsync(identificacion);

    public async Task<int> CrearAsync(Funcionarios_Usuarios funcionario)
    {
        ValidarFuncionario(funcionario);
        return await _funcionarioRepository.CrearAsync(funcionario);
    }

    public async Task<int> ActualizarAsync(Funcionarios_Usuarios funcionario)
    {
        ValidarFuncionario(funcionario);
        return await _funcionarioRepository.ActualizarAsync(funcionario);
    }

    public async Task<int> EliminarAsync(string identificacion)
        => await _funcionarioRepository.EliminarAsync(identificacion);

    public async Task<IEnumerable<TipoIdentificacion>> ObtenerTiposIdentificacionAsync()
        => await _funcionarioRepository.ObtenerTiposIdentificacionAsync();

    public async Task<IEnumerable<Rol>> ObtenerRolesAsync()
        => await _funcionarioRepository.ObtenerRolesAsync();

    public async Task<IEnumerable<Estado>> ObtenerEstadosAsync()
        => await _funcionarioRepository.ObtenerEstadosAsync();

    // Validaciones antes de CRUD
    private void ValidarFuncionario(Funcionarios_Usuarios f)
    {
        if (string.IsNullOrWhiteSpace(f.Identificacion))
            throw new ArgumentException("La identificación es obligatoria.");

        if (string.IsNullOrWhiteSpace(f.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(f.Apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        if (!string.IsNullOrWhiteSpace(f.Correo) &&
            !Regex.IsMatch(f.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("El correo electrónico no es válido.");

        if (string.IsNullOrWhiteSpace(f.ID_Rol))
            throw new ArgumentException("Debe seleccionar un rol.");

        if (string.IsNullOrWhiteSpace(f.Estado))
            throw new ArgumentException("Debe seleccionar un estado.");
    }
}


