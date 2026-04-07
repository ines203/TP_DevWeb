using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GestionVoitureContext>(options =>
    options.UseInMemoryDatabase("GestionVoitureDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GestionVoitureContext>();

builder.Services.AddFluentValidationAutoValidation();  //active la validation automatique côté serveur
builder.Services.AddFluentValidationClientsideAdapters(); //active la validation automatique côté client (javascript) en utilisant les règles définies dans les classes de validation FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>(); //enregistre toutes les classes de validation qui se trouvent dans l'assembly de l'application (le projet ContactManager.WebSite) et qui implémentent l'interface IValidator<T> de FluentValidation. Cela permet à FluentValidation de découvrir automatiquement les règles de validation définies dans ces classes et de les appliquer lors de la validation des modèles dans les contrôleurs.

var app = builder.Build();

//initialiser la base de données
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<GestionVoitureContext>();
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();
    await initaliserBD.Seed(context, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();