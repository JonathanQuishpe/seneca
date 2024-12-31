using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Seneca.Models.Entities;
using Seneca.Services;
using Seneca.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(a =>
{
    a.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Home/Login";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<MailjetSettings>(builder.Configuration.GetSection("Mailjet"));
builder.Services.AddScoped<MailJetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
