using HRMS.DAO;
using HRMS.Reportings;
using HRMS.Services;
using HRMS.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var config = builder.Configuration;
builder.Services.AddDbContext<HRMSApplicationDbContext>(o => o.UseSqlServer(config.GetConnectionString("HRMSConnectionString")));

builder.Services.AddRazorPages();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HRMSApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IReproting, Reporting>();
builder.Services.AddTransient<IPositionService, PositionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Correct order
app.UseAuthorization(); // Correct order

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Correct order

app.Run();