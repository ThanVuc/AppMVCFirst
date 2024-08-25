using AppMVC.Areas.ProductManage.Services;
using AppMVC.ExtensionMethod;
using AppMVC.Models;
using AppMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static AppMVC.Services.SendMailServices;

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

//Session Config
services.AddDistributedMemoryCache();
services.AddSession(cfg =>
{
    cfg.Cookie.Name = "MySession";
    cfg.IdleTimeout = new TimeSpan(0,30,0);
});


services.AddControllersWithViews();
services.AddRazorPages();
services.AddSingleton<PlanetServices, PlanetServices>();

//Route config
services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = false;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
});

services.AddOptions();

services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();




services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;
});

services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/";
    options.LogoutPath = "/Logout/";
    options.AccessDeniedPath = "/AccessDenied/";
});

services.AddAuthentication()
    .AddGoogle(options =>
    {
        var googleConfig = configuration.GetSection("Authentication:Google");
        options.ClientId = googleConfig["ClientID"];
        options.ClientSecret = googleConfig["ClientSecret"];
        //http://localhost:5214/google-login
        options.CallbackPath = "/google-login";
    })
    .AddFacebook(options =>
    {
        var facebookConfig = configuration.GetSection("Authentication:Facebook");
        options.ClientId = facebookConfig["ClientID"];
        options.ClientSecret = facebookConfig["ClientSecret"];
    });

services.AddAuthorization(options =>
{
    options.AddPolicy("HighLevelManage", policy =>
    {
        policy.RequireRole("Adminstrator");
    });
});

// Register Mail Services
var mailSettings = configuration.GetSection("MailSettings");
services.Configure<MailSettings>(mailSettings);
services.AddTransient<IEmailSmsSender, SendMailServices>();

//Register Cart Services
services.AddTransient<CartServices>();

//Access to Url Helper 
services.AddTransient<ActionContextAccessor>();

services.AddTransient<SideBarRenderServices>();


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
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "image")),
    RequestPath = "/image"
});

app.AddStatusCodePage();

app.UseRouting();

app.UseSession();

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
