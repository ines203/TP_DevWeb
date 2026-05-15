using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Location;
using SystemeDeLocation.Website.Models.Utilisateur;
using CreationModel = SystemeDeLocation.Website.Models.Utilisateur.UtilisateurCreation;
using ListModel = SystemeDeLocation.Website.Models.Utilisateur.UtilisateurList;

namespace SystemeDeLocation.Website.Controllers
{
    [Authorize(Roles = "ADMINISTRATEUR")]
    public class UtilisateurController : BaseController
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<UtilisateurBD> _userManager;

        public UtilisateurController(GestionVoitureContext context,
            UserManager<UtilisateurBD> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) : base(context, userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
            string mdp = GenererMotDePasse();
            var model = new CreationModel
            {
                MotDePasse = mdp,
                ConfirmationMotDePasse = mdp
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

            //verifier si pas admin doit entrer une succursale associer
            if (model.Role != "ADMINISTRATEUR" && model.SuccursaleId == null)
            {
                ModelState.AddModelError("SuccursaleId",
                    "[VALIDATION] Une succursale doit être associée pour ce rôle.");
                await RemplirListes(model);
                return View(model);
            }

            try
            {
                var suc = await _context.Succursales.FindAsync(model.SuccursaleId);
                var user = UtilisateurBD.Create(model.NomUtilisateur, suc);
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

        [HttpPost]
        public async Task<IActionResult> Supprimer(string id)
        {
            var usager = await _userManager.FindByIdAsync(id);

            if (usager == null)
            {
                TempData["Erreur"] = "[ERREUR] L'utilisateur est introuvable.";
                return RedirectToAction(nameof(List));
            }

            var result = await _userManager.DeleteAsync(usager);

            if (result.Succeeded)
            {
                TempData["Succes"] = "L'utilisateur a été supprimé avec succès.";
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet]

        public async Task<IActionResult> ModificationMdp(string id)
        {
            var usager = await _userManager.FindByIdAsync(id);
            if (usager == null)
            {
                TempData["Erreur"] = "[ERREUR] L'utilisateur est introuvable.";
                return RedirectToAction(nameof(List));
            }
            string mdp = GenererMotDePasse();

            var model = new UtilisateurModificationMdp
            {
                Id = usager.Id,
                NomUtilisateur = usager.UserName,
                MotDePasse = mdp,
                ConfirmationMotDePasse = mdp
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ModificationMdp(UtilisateurModificationMdp model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usager = await _userManager.FindByIdAsync(model.Id.ToString());

            if (usager == null)
            {
                TempData["Erreur"] = "[ERREUR] L'utilisateur est introuvable.";
                return RedirectToAction(nameof(List));
            }

            var result = await _userManager.RemovePasswordAsync(usager);

            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(usager, model.MotDePasse);
                TempData["Succes"] = "Le mot de passe a été modifié avec succès.";
                return RedirectToAction(nameof(List));
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }
        }
    }
}