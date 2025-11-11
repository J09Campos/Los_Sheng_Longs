using Microsoft.AspNetCore.Authentication.Cookies;
using Reloj_Marcador;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<FuncionariosRepository>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosServices>();

builder.Services.AddScoped<AreaRepository>();
builder.Services.AddScoped<IAreaService, AreaServices>();

builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddScoped<MarcasRepository>();
builder.Services.AddScoped<IMarcasService, MarcasService>();

builder.Services.AddScoped<InconsistenciasRepository>();
builder.Services.AddScoped<IInconsistenciasService, InconsistenciasService>();

builder.Services.AddScoped<RolesRepository>();
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddScoped<TiposIdentificacionRepository>();
builder.Services.AddScoped<ITiposIdentificacionService, TiposIdentificacionService>();

builder.Services.AddScoped<DetalleHorarioRepository>();
builder.Services.AddScoped<IDetalleHorarioService, DetalleHorarioService>();

builder.Services.AddScoped<HorarioRepository>();
builder.Services.AddScoped<IHorarioService, HorarioService>();

builder.Services.AddScoped<MotivoRepository>();
builder.Services.AddScoped<IMotivoService, MotivoService>();

builder.Services.AddScoped<Proc1Repository>();
builder.Services.AddScoped<Proc1Service>();
builder.Services.AddHostedService<Proc1HostedService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                var returnUrl = ctx.Request.Path;

                if (ctx.Request.Cookies.ContainsKey(".AspNetCore.Cookies") &&
                    (ctx.HttpContext.User?.Identity == null || !ctx.HttpContext.User.Identity.IsAuthenticated))
                {
                    ctx.Response.Redirect("/Login/Login?expired=true");
                }
                else
                {
                    ctx.Response.Redirect("/Login/Login?unauthenticated=true");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapProc1Endpoints();
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/Login");
    return Task.CompletedTask;
});
app.Run();
