using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Conducteur;


namespace SystemeDeLocation.Website.Controllers
{
    [Authorize]
    public class ConducteurController : BaseController
    {
        public ConducteurController(GestionVoitureContext context,
            UserManager<UtilisateurBD> userManager) : base(context, userManager)
        {
        }

        // UC19 - Liste des conducteurs
        [HttpGet]
        public IActionResult List()
        {
            var conducteurs = _context.Conducteurs
                .AsNoTracking()
                .Select(c => new List
                {
                    ConducteurId = c.Id,
                    Prenom = c.Prenom,
                    Nom = c.Nom,
                    Ouverture = c.Location.Ouverture,
                    FermeturePlanifiee = c.Location.FermeturePlanifiee,
                    Surnom = c.Location.Voiture.Surnom
                }).ToList();

            return View(conducteurs);
        }

        // UC21 - Profil d'un conducteur
        [HttpGet]
        public IActionResult Profil(int id)
        {
            var conducteur = _context.Conducteurs
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ConducteurProfil
                {
                    Prenom = c.Prenom,
                    Nom = c.Nom,
                    AdresseCourriel = c.AdresseCourriel,
                    NumeroDeTelephone = c.NumeroDeTelephone,
                    NumeroDuPermis = c.NumeroDuPermis,
                    NumeroCivic = c.Adresse.NumeroCivique,
                    Rue = c.Adresse.Rue,
                    Ville = c.Adresse.Ville,
                    CodePostal = c.Adresse.CodePostal,
                    LocationId = c.Location.Id,
                    Statut = c.Location.Statut,
                    Ouverture = c.Location.Ouverture,
                    FermeturePlanifiee = c.Location.FermeturePlanifiee,
                    Surnom = c.Location.Voiture.Surnom
                })
                .FirstOrDefault();

            if (conducteur == null)
            {
                TempData["Erreur"] = "[ERREUR] Le conducteur est introuvable.";
                return RedirectToAction("List");
            }

            return View(conducteur);
        }
    }
}