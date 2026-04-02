using SystemeDeLocation.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Website.Models.Conducteur;

namespace SystemeDeLocation.Website.Controllers
{
    public class ConducteurController : BaseController
    {
        public ConducteurController(GestionVoitureContext context) : base(context)
        {
        }

        // UC19 - Liste des conducteurs
        [HttpGet]
        public IActionResult List()
        {
            if (!UsagerEstAuthentifier())
                return RetourAuth();

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
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            var conducteur = _context.Conducteurs
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new Profil
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

