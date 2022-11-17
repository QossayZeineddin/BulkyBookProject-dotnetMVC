using BulkyBook.Data;
using BulkyBook.Repository;
using BulkyBook.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Uitility;
using Microsoft.CodeAnalysis.Options;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplecationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ApplecationDbContext") ?? throw new InvalidOperationException
        ("Connection string 'WebApplication1Context' not found.")
));


//options => options.SignIn.RequireConfirmedAccount = true
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplecationDbContext>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UntiOfWork>();
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));
// because hot loading we don't need this
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "3322507987967921";
    options.AppSecret = "1ab99e79a8944aeeaa69fde538553abc";
});
builder.Services.ConfigureApplicationCookie(Option =>
{
    Option.LoginPath = $"/Identity/Account/Login";
    Option.LogoutPath = $"/Identity/Account/Logout";
    Option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<String>();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();