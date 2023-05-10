using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using static ExaminationProject.Authorization.IsNotDeletedRequirement;
using ExaminationProject.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login/";
    options.AccessDeniedPath = "/auth/login";

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsNotDeletedPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new IsNotDeletedRequirement());
    });

});
builder.Services.AddScoped<IAuthorizationHandler, IsNotDeletedRequirementHandler>();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{seourl?}");

app.Run();
