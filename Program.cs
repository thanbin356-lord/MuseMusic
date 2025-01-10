using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using MuseMusic.Models.Services;
using MuseMusic.Models.Tables;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Authentication and Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Đường dẫn trang đăng nhập
        options.AccessDeniedPath = "/Login/AccessDenied"; // Trang từ chối truy cập
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    });

builder.Services.AddDbContext<shopmanagementContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                      ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddAuthorization();

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization Middleware
app.UseAuthentication(); // Xác thực trước khi phân quyền
app.UseAuthorization();

// Endpoint Mapping
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
