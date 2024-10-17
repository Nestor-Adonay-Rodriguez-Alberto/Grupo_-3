using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configura y agrega un cliente HTTP con nombre "API_RESTful"  * AGREGADA *
builder.Services.AddHttpClient("API_RESTful", x =>
{
    // Configura la dirección base del cliente HTTP desde la configuración
    x.BaseAddress = new Uri(builder.Configuration["UrlsAPI:Puerto"]);
});

// PARA AUTENTICACION BASADA EN COOKIES:
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie((o) =>
{
    o.LoginPath = new PathString("/Autenticacion/Login"); // No Autenticado
    o.AccessDeniedPath = new PathString("/Autenticacion/Login"); // No Autorizado
    o.ExpireTimeSpan = TimeSpan.FromHours(8);
    o.SlidingExpiration = true;
    o.Cookie.HttpOnly = true;
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// PARA AUTENTICADOS:
app.UseAuthentication();
app.UseAuthorization();

// ACCION DE INICIO:
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
