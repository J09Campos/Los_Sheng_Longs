using Reloj_Marcador.Entities;
using Reloj_Marcador.Services;

namespace Reloj_Marcador
{
    public static class Proc1Endpoints
    {
        public static void MapProc1Endpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/proc1");

            group.MapPost("/ejecutar", async (Proc1Request request, Proc1Service service) =>
            {
                await service.EjecutarAsync(request.Inicio, request.Fin, request.Area, request.Funcionario);
                return Results.Ok(new
                {
                    mensaje = "PROC1 ejecutado correctamente.",
                    rango = $"{request.Inicio:yyyy-MM-dd} a {request.Fin:yyyy-MM-dd}"
                });
            })
            .WithTags("PROC1 - Inconsistencias automáticas");

            group.MapGet("/bitacora", async (Proc1Service service) =>
            {
                var data = await service.ListarBitacoraAsync();
                return Results.Ok(data);
            })
            .WithTags("PROC1 - Inconsistencias automáticas");

            group.MapGet("/inconsistencias", async (DateTime? inicio, DateTime? fin, string? area, string? funcionario, Proc1Service service) =>
            {
                var data = await service.ListarInconsistenciasAsync(inicio, fin, area, funcionario);
                return Results.Ok(data);
            });
        }
    }
}
