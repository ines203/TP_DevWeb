using SystemeDeLocation.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Website.Models.Location;

namespace SystemeDeLocation.Website.Controllers
{
    public class LocationController : BaseController
    {
        public LocationController(GestionVoitureContext context) : base(context)
        {
        }

        // UC15 - Liste des locations
        [HttpGet]
        public IActionResult List(int? id)
        {
            if (!UsagerEstAuthentifier())
                return RetourAuth();

             var query = _context.Locations.AsQueryable();

            if (id != null)
                query = query.Where(l => l.Voiture.SuccursaleId == id);

            var locations = query
                .AsNoTracking()
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
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            var location = _context.Locations
                .AsNoTracking()
                .Where(loc => loc.Id == id)
                .Select(loc => new Details
                {
                    LocationId = loc.Id,
                    Prenom = loc.Conducteur.Prenom,
                    Nom = loc.Conducteur.Nom,
                    AdresseCourriel = loc.Conducteur.AdresseCourriel,
                    NumeroDeTelephone = loc.Conducteur.NumeroDeTelephone,
                    NumeroDuPermis = loc.Conducteur.NumeroDuPermis,
                    NomSuccursale = loc.Voiture.Succursale.Nom,
                    Surnom = loc.Voiture.Surnom,
                    StatutVoit = loc.Voiture.Statut,
                    Disponible = loc.Voiture.Disponible,
                    Etat = loc.Voiture.Etat,
                    NumeroDeSerie = loc.Voiture.NumeroDeSerie,
                    NumeroImmatriculation = loc.Voiture.NumeroImmatriculation,
                    Marque = loc.Voiture.Marque,
                    Modele = loc.Voiture.Modele,
                    Annee = loc.Voiture.Annee,
                    Couleur = loc.Voiture.Couleur,
                    Kilometrage = loc.Voiture.Kilometrage,
                    ValeurEstime = loc.Voiture.ValeurEstime,
                    StatutLoc = loc.Statut,
                    Ouverture = loc.Ouverture,
                    FermeturePlanifiee = loc.FermeturePlanifiee,
                    FermetureReelle = loc.FermetureReelle,
                    Notes = loc.Notes
                })
                .FirstOrDefault();

            if (location == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction("List");
            }

            return View(location);
        }
    }
}

