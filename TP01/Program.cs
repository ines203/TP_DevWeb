///Auteur : Ritchy Lemonde
///Auteur : Rafaël Laserra
///Auteur : Inès Lidaou Tchoou

using SystemeDeLocation.Core.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GestionVoitureContext>(options =>
    options.UseInMemoryDatabase("GestionVoitureDb"));

//permet de creer une session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée d'inactivité avant l'expiration de la session
    options.Cookie.HttpOnly = true; // Empêche l'accès au cookie de session via JavaScript
    options.Cookie.IsEssential = true; // Indique que le cookie de session est essentiel pour le fonctionnement de l'application
});

var app = builder.Build();

//initialiser la session et avant app.UseRouting();
app.UseSession();

//initialiser la base de données

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<GestionVoitureContext>();

    initaliserBD.Seed(context);
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
