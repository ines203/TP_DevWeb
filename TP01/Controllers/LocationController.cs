using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;
using TP01.Models.Location;

namespace TP01.Controllers
{
    public class LocationController : BaseController
    {
        private readonly GestionVoitureContext _context;

        public LocationController(GestionVoitureContext context)
        {
            _context = context;
        }

        // UC15 - Liste des locations
        [HttpGet]
        public IActionResult List(int? succursaleId)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

             var query = _context.Locations
                .Include(l => l.Conducteur)
                .Include(l => l.Voiture)
                    .ThenInclude(v => v.Succursale)
                .AsQueryable();

            if (succursaleId != null)
                query = query.Where(l => l.Voiture.SuccursaleId == succursaleId);

            var locations = query
                    .Select(l => new List
                    {
                        LocationId = l.Id,
                        Prenom = l.Conducteur.Prenom,
                        Nom = l.Conducteur.Nom,
                        Surnom = l.Voiture.Surnom,
                        StatutLoc = l.Statut,
                        Ouverture = l.Ouverture,
                        FermeturePlanifiee = l.FermeturePlanifiee,
                        FermetureReelle = l.FermetureReelle
                    }).ToList();

            return View(locations);
        }

        // UC16 - Consulter une location
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var location = _context.Locations
                .Include(l => l.Conducteur)
                    .ThenInclude(c => c.Adresse)
                .Include(l => l.Voiture)
                    .ThenInclude(v => v.Succursale)
                .Include(l => l.Notes)
                .FirstOrDefault(l => l.Id == id);

            if (location == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction("List");
            }

            var locationViewModel = new Details
            {
                LocationId = location.Id,
                Prenom = location.Conducteur.Prenom,
                Nom = location.Conducteur.Nom,
                AdresseCourriel = location.Conducteur.AdresseCourriel,
                NumeroDeTelephone = location.Conducteur.NumeroDeTelephone,
                NumeroDuPermis = location.Conducteur.NumeroDuPermis,
                NomSuccursale = location.Voiture.Succursale.Nom,
                Surnom = location.Voiture.Surnom,
                StatutVoit = location.Voiture.Statut,
                Disponible = location.Voiture.Disponible,
                Etat = location.Voiture.Etat,
                NumeroDeSerie = location.Voiture.NumeroDeSerie,
                NumeroImmatriculation = location.Voiture.NumeroImmatriculation,
                Marque = location.Voiture.Marque,
                Modele = location.Voiture.Modele,
                Annee = location.Voiture.Annee,
                Couleur = location.Voiture.Couleur,
                Kilometrage = location.Voiture.Kilometrage,
                ValeurEstime = location.Voiture.ValeurEstime,
                StatutLoc = location.Statut,
                Ouverture = location.Ouverture,
                FermeturePlanifiee = location.FermeturePlanifiee,
                FermetureReelle = location.FermetureReelle,
                Notes = location.Notes
            };

            return View(locationViewModel);
        }
    }
}

