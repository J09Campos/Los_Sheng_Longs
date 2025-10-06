using Reloj_Marcador.Repository;
using Reloj_Marcador.Repository.Reloj_Marcador.Repository;
using Reloj_Marcador.Services;
using Reloj_Marcador.Services.Abstract;
using Reloj_Marcador.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<HorarioRepository>();
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddScoped<DetalleHorarioRepository>();
builder.Services.AddScoped<MotivoRepository>(); 
builder.Services.AddScoped<IHorarioService, HorarioService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IDetalleHorarioService, DetalleHorarioService>();
builder.Services.AddScoped<IMotivoService, MotivoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
