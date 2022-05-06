using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Optik_A.Data;
using Optik_A.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Optik_AContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Optik_AContext") ?? throw new InvalidOperationException("Connection string 'Optik_AContext' not found.")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Optik_AContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>                                                               //CookieAuthenticationOptions
        {
            options.LoginPath = new PathString("/Account/Login");
            options.Cookie.Expiration = TimeSpan.FromDays(3650);
        });

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
