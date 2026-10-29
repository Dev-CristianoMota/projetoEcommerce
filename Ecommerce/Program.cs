using Ecommerce.Data;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration.GetSection("Cloudinary");
var cloudinary = new Cloudinary(new Account(cfg["CloudName"], cfg["ApiKey"], cfg["ApiSecret"]))
{
    Api = { Secure = true }
};

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

builder.Services.AddSingleton(cloudinary);
builder.Services.AddScoped<ImagemService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<EnderecoService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ProdutoFeedService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
