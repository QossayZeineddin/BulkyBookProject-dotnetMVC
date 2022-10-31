using BulkyBook.Areas.Admin.Data;
using BulkyBook.Repository;
using BulkyBook.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplecationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ApplecationDbContext") ?? throw new InvalidOperationException
        ("Connection string 'WebApplication1Context' not found.")
));

builder.Services.AddScoped<IUnitOfWork, UntiOfWork>();
// because hot loading we don't need this
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();