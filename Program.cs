using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Opgavesæt4.CustomMiddleware;
using Opgavesæt4.Data;
using Opgavesæt4.Models;
using Opgavesæt4.Services;
using System.Configuration;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.UseMiddleware<CustomLogger>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    dbContext.ClearDatabase();
    ApplicationDbContext.SeedApplicationUsers(userManager, dbContext);

    DeleteFiles(services);
}

app.Run();

void DeleteFiles(IServiceProvider services)
{
    var _webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
    string rootPath = _webHostEnvironment.WebRootPath.Replace('\\', '/');
    var filePath = Path.Combine(rootPath + "/logFiles/", "albumLogs");
    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }
    filePath = Path.Combine(rootPath + "/logFiles/", "songLogs");
    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }
}



