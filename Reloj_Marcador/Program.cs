using Microsoft.AspNetCore.Authentication.Cookies;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Funcionarios
builder.Services.AddScoped<FuncionariosRepository>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosServices>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

//Áreas
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddScoped<IAreaService, AreaServices>();

builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

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

                // Si existe la cookie en el request pero el usuario no está autenticado va a ser expirada

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/Login");
    return Task.CompletedTask;
});

app.Run();
