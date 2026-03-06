using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;
using TP01.Models.Conducteur;

namespace TP01.Controllers
{
    public class ConducteurController : BaseController
    {
        private readonly GestionVoitureContext _context;

        public ConducteurController(GestionVoitureContext context)
        {
            _context = context;
        }

        // UC19 - Liste des conducteurs
        [HttpGet]
        public IActionResult List()
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var conducteurs = _context.Conducteurs
                .Include(c => c.Location)
                    .ThenInclude(l => l.Voiture)
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
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var conducteur = _context.Conducteurs
                .Include(c => c.Adresse)
                .Include(c => c.Location)
                    .ThenInclude(l => l.Voiture)
                .FirstOrDefault(c => c.Id == id);

            if (conducteur == null)
            {
                TempData["Erreur"] = "[ERREUR] Le conducteur est introuvable.";
                return RedirectToAction("List");
            }

            var conducteurView = new Profil
            {
                Prenom = conducteur.Prenom,
                Nom = conducteur.Nom,
                AdresseCourriel = conducteur.AdresseCourriel,
                NumeroDeTelephone = conducteur.NumeroDeTelephone,
                NumeroDuPermis = conducteur.NumeroDuPermis,
                NumeroCivic = conducteur.Adresse.NumeroCivic,
                Rue = conducteur.Adresse.Rue,
                Ville = conducteur.Adresse.Ville,
                CodePostal = conducteur.Adresse.CodePostal,
                locationId = conducteur.Location.Id,
                Statut = conducteur.Location.Statut,
                Ouverture = conducteur.Location.Ouverture,
                FermeturePlanifiee = conducteur.Location.FermeturePlanifiee,
                Surnom = conducteur.Location.Voiture.Surnom
            };

            return View(conducteurView);
        }
    }
}

