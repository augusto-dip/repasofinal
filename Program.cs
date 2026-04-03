using tl2_recupercionparcial2_2025_augusto_dip.Repositories;
using tl2_recupercionparcial2_2025_augusto_dip.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// ¡ESTA ES LA LÍNEA MÁGICA QUE FALTABA PARA QUE NO TIRE ERROR!
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// ¡ESTO TAMBIÉN FALTABA PARA QUE LA SESIÓN FUNCIONE REALMENTE! 
// Siempre debe ir entre UseRouting y UseAuthorization
app.UseSession(); 

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();