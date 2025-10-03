using Microsoft.AspNetCore.Authentication.Cookies;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

// Inyecci�n de Dependencias (Utilizaci�n de Instancias de Clase sin Usarlas de Maneras Explicitas)

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// Llega Solicitud Http / Para casa Solicitud Crea un LoginRepository / Cada vez que ocupe uno que vayan a un constructor y de alguna forma le va a llegar la instancia de lo que ocupa

builder.Services.AddScoped<LoginRepository>();

builder.Services.AddScoped<ILoginService, LoginService>();

// Agregar soporte para cache distribuido y sesi�n

builder.Services.AddDistributedMemoryCache();

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

                // Si existe la cookie en el request pero el usuario no est� autenticado va a ser expirada

                if (ctx.Request.Cookies.ContainsKey(".AspNetCore.Cookies") &&
                    (ctx.HttpContext.User?.Identity == null || !ctx.HttpContext.User.Identity.IsAuthenticated))
                {
                    ctx.Response.Redirect("/Login/Login?expired=true");
                }
                else
                {
                    // Nunca estuvo logueado

                    ctx.Response.Redirect("/Login/Login?unauthenticated=true");
                }

                return Task.CompletedTask;

            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline (Basicamente es un Filtro).

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapRazorPages();

// Para iniciar en la p�gina de login de Primero

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/Login");
    return Task.CompletedTask;
});

app.Run();
