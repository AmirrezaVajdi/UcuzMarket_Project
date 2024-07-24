using _0_Framework.Application;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DeliveryManagement.Infrastructure.Configuration;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using InventoryManagement.Presentation.Api;
using LampShade.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost;
using ShopManagement.Infrastructure.Configuration;
using ShopManagement.Presentation.Api;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));

var connectionString = builder.Configuration.GetConnectionString("LampShadeDb");

ShopContextBootstrapper.Configure(builder.Services, connectionString);
DiscountManagementBootstrapper.Configure(builder.Services, connectionString);
InventoryManagementBootstrapper.Configure(builder.Services, connectionString);
BlogManagementBootstrapper.Configure(builder.Services, connectionString);
CommentManagementBootstrapper.Configure(builder.Services, connectionString);
AccountManagementBootstrapper.Configure(builder.Services, connectionString);
DeliveryManagementBootstrapper.Configure(builder.Services, connectionString);

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
builder.Services.AddTransient<ISmsService, SmsService>();
LampShadeSettingsBootstrapper.Configure(builder.Services);


builder.Services.Configure<CookiePolicyOptions>(option =>
{
    option.CheckConsentNeeded = context => true;
    //option.MinimumSameSitePolicy = SameSiteMode.Strict; وقتی از درگاه پرداخت برمیگشتیم به سایت کوکی ما رو نمی خوند و با خطای لاگین مواجه می شدیم بنابراین کار زیر را می کنیم 

    option.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = "/Account";
        o.LogoutPath = "/Account";
        o.AccessDeniedPath = "/AccessDenied";
    });


builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminArea", builder =>
    {
        builder.RequireRole(new List<string> { Roles.Administrator, Roles.ContentUploader });
    });
    option.AddPolicy("Shop", builder =>
    {
        builder.RequireRole(new List<string> { Roles.Administrator });
    });
    option.AddPolicy("Discount", builder =>
    {
        builder.RequireRole(new List<string> { Roles.Administrator });
    });
    option.AddPolicy("Account", builder =>
    {
        builder.RequireRole(new List<string> { Roles.Administrator });
    });
});


builder.Services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
    .AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
    options.Conventions.AuthorizeAreaFolder("Administration", "/Shop", "Shop");
    options.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discount");
    options.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
})
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(InventoryController).Assembly)
    .AddApplicationPart(typeof(LampShadeController).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var res = builder.Services.BuildServiceProvider();
DataBaseCreator.Migrate(res);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
