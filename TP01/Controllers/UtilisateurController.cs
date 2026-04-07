using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using ListModel = SystemeDeLocation.Website.Models.Utilisateur.UtilisateurList;
using CreationModel = SystemeDeLocation.Website.Models.Utilisateur.UtilisateurCreation;

namespace SystemeDeLocation.Website.Controllers
{
    [Authorize(Roles = "ADMINISTRATEUR")]
    public class UtilisateurController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UtilisateurController(GestionVoitureContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context, userManager)
        {
            _roleManager = roleManager;
        }

        // UC01 - Liste des utilisateurs
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var utilisateurs = await _userManager.Users
                    .AsNoTracking()
                    .Select(u => new ListModel
                    {
                        Id = u.Id,
                        NomUtilisateur = u.UserName
                    }).ToListAsync();

                return View(utilisateurs);
            }
            catch (Exception)
            {
                TempData["Erreur"] = "[ERREUR] Impossible d'afficher la liste des utilisateurs.";
                return View(new System.Collections.Generic.List<ListModel>());
            }
        }

        // UC02 - Créer un utilisateur
        [HttpGet]
        public async Task<IActionResult> Creation()
        {
            var model = new CreationModel
            {
                MotDePasse = GenererMotDePasse(),
            };
            await RemplirListes(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Creation(CreationModel model)
        {
            if (!ModelState.IsValid)
            {
                await RemplirListes(model);
                return View(model);
            }

            // Vérifier si le nom d'utilisateur existe déjà
            var existant = await _userManager.FindByNameAsync(model.NomUtilisateur);
            if (existant != null)
            {
                ModelState.AddModelError("NomUtilisateur",
                    "[VALIDATION] Ce nom d'utilisateur existe déjà.");
                await RemplirListes(model);
                return View(model);
            }

            try
            {
                var user = new IdentityUser { UserName = model.NomUtilisateur };
                var result = await _userManager.CreateAsync(user, model.MotDePasse);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    await RemplirListes(model);
                    return View(model);
                }

                await _userManager.AddToRoleAsync(user, model.Role);

                TempData["Succes"] = $"L'utilisateur {user.UserName} (ID: {user.Id}) a été créé avec succès.";
                return RedirectToAction(nameof(List));
            }
            catch (Exception)
            {
                TempData["Erreur"] = "[ERREUR] Impossible de créer l'utilisateur.";
                await RemplirListes(model);
                return View(model);
            }
        }

        // Génère un mot de passe aléatoire
        private string GenererMotDePasse()
        {
            var random = new Random();
            const string lettresMaj = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lettresMin = "abcdefghijklmnopqrstuvwxyz";
            const string chiffres = "0123456789";
            const string speciaux = "!@#$%";

            var mdp = new System.Text.StringBuilder();
            mdp.Append(lettresMaj[random.Next(lettresMaj.Length)]);
            mdp.Append(lettresMin[random.Next(lettresMin.Length)]);
            mdp.Append(chiffres[random.Next(chiffres.Length)]);
            mdp.Append(speciaux[random.Next(speciaux.Length)]);

            const string tousCaracteres = lettresMaj + lettresMin + chiffres + speciaux;
            for (int i = 0; i < 8; i++)
                mdp.Append(tousCaracteres[random.Next(tousCaracteres.Length)]);

            return new string(mdp.ToString().OrderBy(_ => random.Next()).ToArray());
        }

        private async Task RemplirListes(CreationModel model)
        {
            model.Roles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            var succursales = await _context.Succursales
                .AsNoTracking()
                .Where(s => s.Statut == SystemeDeLocation.Core.Domain.StatutSuccursale.ACTIVE)
                .Select(s => new { s.Id, s.Nom })
                .ToListAsync();

            model.Succursales = succursales.Select(s =>
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Nom
                }).ToList();
        }
    }
}