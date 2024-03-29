using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Data.Config.SeedData;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Services.Mapping;
using OnlineShop.Web.Infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwaggerDocumentation();

app.UseStaticFiles();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<StoreContext>();
var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
    await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);
    await ImageSeeder.SeedImagesAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "Error during migration");
}

app.Run(); 