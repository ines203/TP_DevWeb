using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Succursale;


namespace SystemeDeLocation.Website.Controllers
{
    public class SuccursaleController : BaseController
    {
        public SuccursaleController(GestionVoitureContext context,
            UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var succursales = _context.Succursales
                .AsNoTracking()
                .Select(suc => new SuccursaleList
                {
                    Id = suc.Id,
                    Nom = suc.Nom,
                    Statut = suc.Statut,
                    NbVoitureDispo = suc.Voitures.Count(v => v.Disponible),
                    NbVoitureIndispo = suc.Voitures.Count(v => !v.Disponible),
                    NbVoitureArchiver = suc.Voitures.Count(v => v.Archiver),
                }).ToList();

            return View(succursales);
        }

        [HttpPost]
        public IActionResult ChangementStatue(int id)
        {
            var succursale = _context.Succursales.Find(id);

            if (succursale == null)
            {
                TempData["Erreur"] = "[ERREUR] La succursale n'existe pas. Impossible d'activer/désactiver.";
                return RedirectToAction("List");
            }

            succursale.Statut = succursale.Statut == StatutSuccursale.ACTIVE
                ? StatutSuccursale.DESACTIVE
                : StatutSuccursale.ACTIVE;

            _context.SaveChanges();

            TempData["Succes"] = $"La succursale {succursale.Nom} est maintenant {succursale.Statut}.";
            return RedirectToAction("List");
        }

        // UC06 - Créer une nouvelle succursale
        [HttpGet]
        [Authorize(Roles="ADMINISTRATEUR")]
        public IActionResult Creation()
        {
            return View(new SuccursaleCreation());
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATEUR")]
        public IActionResult Creation(SuccursaleCreation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            // Vérification si l'utilisateur a rentré l'adresse, et si tout est présent
            var champsAdresse = new[] { model.Rue, model.Ville, model.CodePostal };
            bool adressePartielle = model.NumeroCivique != null || champsAdresse.Any(c => !string.IsNullOrWhiteSpace(c));
            bool adresseComplete = model.NumeroCivique != null && champsAdresse.All(c => !string.IsNullOrWhiteSpace(c));
            if (adressePartielle && !adresseComplete)
            {
                ModelState.AddModelError(string.Empty, "[VALIDATION] Veuillez remplir tous les champs de l'adresse ou les laisser tous vides.");
                return View(model);
            }

            try
            {
                var succursale = new SuccursaleBD(model.Nom, StatutSuccursale.ACTIVE);

                if (adresseComplete) {
                    succursale.Adresse = new AdresseBD(model.NumeroCivique.Value, model.Rue, model.Ville, model.CodePostal);
                }

                _context.Succursales.Add(succursale);
                _context.SaveChanges();

                TempData["Succes"] = $"Création de la succursale \"{model.Nom}\" avec succès.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Erreur"] = $"[ERREUR] Impossible de créer la succursale: {ex.Message}";
                return View(model);
            }
        }
    }
}