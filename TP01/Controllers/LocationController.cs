using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Location;
using TP01.Controllers;

namespace SystemeDeLocation.Website.Controllers
{
    public class LocationController : BaseController
    {
        public LocationController(GestionVoitureContext context,
            UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        // UC15 - Liste des locations
        [HttpGet]
        public IActionResult List(int? id)
        {
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

        // UC17 - Créer une location (Check-Out)
        [HttpGet]
        public async Task<IActionResult> Creation(int id)
        {
            var voit = await _context.Voitures.FindAsync(id);
            if (voit == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List");
            }
            var model = new Creation
            {
                VoitureId = id,
                SuccursaleId = voit.SuccursaleId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Creation(Creation nouvelleLocation)
        {
            if (!ModelState.IsValid)
                return View(nouvelleLocation);

            var adresse = await _context.Adresses.FirstOrDefaultAsync(c =>
                c.NumeroCivique == nouvelleLocation.NumeroCivique &&
                c.CodePostal == nouvelleLocation.CodePostal &&
                c.Rue == nouvelleLocation.Rue &&
                c.Ville == nouvelleLocation.Ville);

            if (adresse == null)
            {
                adresse = new AdresseBD(nouvelleLocation.NumeroCivique, nouvelleLocation.Rue,
                    nouvelleLocation.Ville, nouvelleLocation.CodePostal);
                await _context.Adresses.AddAsync(adresse);
                await _context.SaveChangesAsync();
            }

            var conducteur = await _context.Conducteurs.FirstOrDefaultAsync(c =>
                c.AdresseCourriel.ToLower() == nouvelleLocation.AdresseCourriel.ToLower());

            if (conducteur == null)
            {
                conducteur = new ConducteurBD(nouvelleLocation.Prenom, nouvelleLocation.Nom,
                    nouvelleLocation.AdresseCourriel, nouvelleLocation.NumeroDeTelephone,
                    nouvelleLocation.NumeroDuPermis, adresse.Id);
                await _context.Conducteurs.AddAsync(conducteur);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (conducteur.AdresseId != adresse.Id)
                {
                    TempData["Erreur"] = "[ERREUR] L'adresse civique n'est pas la même.";
                    return View(nouvelleLocation);
                }
            }

            await _context.AddAsync(new LocationBD(nouvelleLocation.VoitureId, conducteur.Id,
                nouvelleLocation.FermeturePlanifiee, nouvelleLocation.Note));
            await _context.SaveChangesAsync();

            var voit = await _context.Voitures.FindAsync(nouvelleLocation.VoitureId);
            voit!.Disponible = false;
            await _context.SaveChangesAsync();

            TempData["Succes"] = $"La voiture {voit.Surnom} a été louée avec succès.";
            return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
        }
    }
}
