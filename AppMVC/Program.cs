using AppMVC.ExtensionMethod;
using AppMVC.Models;
using AppMVC.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
var configuration = builder.Configuration;

//Inject SqlServer
services.AddDbContext<AppDBContext>(options =>
{
    string connectString = configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectString);
});


services.AddControllersWithViews();
services.AddRazorPages();
services.AddSingleton<ProductServices,ProductServices>();
services.AddSingleton<PlanetServices, PlanetServices>();

//Route config
services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = false;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
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
app.AddStatusCodePage();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "product",
    pattern: "/{controller=Home}/{action=Index}/{id?}",
    areaName: "ProductManage");

app.MapAreaControllerRoute(
    name: "database",
    pattern: "/{controller=Home}/{action=Index}/{id?}",
    areaName: "DatabaseManage");

app.MapAreaControllerRoute(
    name: "contact",
    pattern: "/{controller=Home}/{action=Index}/{id?}",
    areaName: "ContactManage");

// Controller Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Route
app.MapRazorPages();

// Undefined or Error Route

app.Run();
