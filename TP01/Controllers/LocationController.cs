using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Location;


namespace SystemeDeLocation.Website.Controllers
{
    [Authorize]
    public class LocationController : BaseController
    {
        public LocationController(GestionVoitureContext context,
            UserManager<UtilisateurBD> userManager) : base(context, userManager)
        {

        }

        // UC15 - Liste des locations
        [HttpGet]
        public async Task<IActionResult> List(int? id)
        {
            var query = _context.Locations.AsQueryable();

            if (id == null && !User.IsInRole("ADMINISTRATEUR"))
            {
                var user = await _userManager.GetUserAsync(User);
                id = user.SuccursaleId;
            }

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
            var model = new LocationCreation
            {
                VoitureId = id,
                SuccursaleId = voit.SuccursaleId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Creation(LocationCreation nouvelleLocation)
        {

            if (!ModelState.IsValid)
            {
                return View(nouvelleLocation);
            }

            var adresse = await _context.Adresses.FirstOrDefaultAsync(c =>
                c.NumeroCivique == nouvelleLocation.NumeroCivique &&
                c.CodePostal == nouvelleLocation.CodePostal.Trim() &&
                c.Rue == nouvelleLocation.Rue.Trim() &&
                c.Ville == nouvelleLocation.Ville.Trim());

            if (adresse == null)
            {
                adresse = new AdresseBD(nouvelleLocation.NumeroCivique, nouvelleLocation.Rue.Trim(),
                    nouvelleLocation.Ville.Trim(), nouvelleLocation.CodePostal.Trim());
                await _context.Adresses.AddAsync(adresse);
                await _context.SaveChangesAsync();
            }

            var conducteur = await _context.Conducteurs.FirstOrDefaultAsync(c =>
                c.AdresseCourriel.ToLower() == nouvelleLocation.AdresseCourriel.ToLower().Trim());

            if (conducteur == null)
            {
                conducteur = new ConducteurBD(nouvelleLocation.Prenom.Trim(), nouvelleLocation.Nom.Trim(),
                    nouvelleLocation.AdresseCourriel.Trim(), nouvelleLocation.NumeroDeTelephone.Trim(),
                    nouvelleLocation.NumeroDuPermis.Trim(), adresse.Id);
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
            if(nouvelleLocation.Note != null)
            {
                nouvelleLocation.Note = nouvelleLocation.Note.Trim();
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

        //UC 20
        [HttpGet]
        public async Task<IActionResult> Supprimer(int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            var voit = await _context.Voitures.FindAsync(loc!.VoitureId);

            if (loc == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction("List");
            }
            var model = new LocationSupprimer
            {
                LocationId = id,
                FermeturePlanifiee = loc.FermeturePlanifiee,
                SuccursaleId = voit.SuccursaleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Supprimer(LocationSupprimer nouvelleLocation)
        {

            if (!ModelState.IsValid)
            {
                return View(nouvelleLocation);
            }
           

            var loc = await _context.Locations.FindAsync(nouvelleLocation.LocationId);

            if (loc == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
            }

            if(loc.Statut == StatutLocation.FERME)
            {
                TempData["Erreur"] = "[ERREUR] La location est déjà fermée.";
                return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
            }

            var voit = await _context.Voitures.FindAsync(loc!.VoitureId);


            if (voit == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture de la location est introuvable.";
                return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
            }

            if(voit.SuccursaleId != nouvelleLocation.SuccursaleId)
            {
                TempData["Erreur"] = "[ERREUR] La voiture de la location n'appartient pas à la succursale.";
                return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
            }

            voit.Disponible = true;
            loc.Statut = StatutLocation.FERME;
            loc.FermetureReelle = nouvelleLocation.FermetureReelle;

            if (nouvelleLocation.Note != null)
            {
                loc.Notes.Add(new NoteBD(nouvelleLocation.Note.Trim()));
            }

            await _context.SaveChangesAsync();

            TempData["Succes"] = "[SUCCES] La location à été supprimée.";
            return RedirectToAction(nameof(List), new { id = nouvelleLocation.SuccursaleId });
        }
        // UC18 - Ajouter une note à une location
        [HttpGet]
        public IActionResult AjouterNote(int id)
        {
            var location = _context.Locations
                .Include(l => l.Voiture)
                .FirstOrDefault(l => l.Id == id);

            if (location == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction("List");
            }

            return View(new LocationNote
            {
                LocationId = id,
                SuccursaleId = location.Voiture.SuccursaleId
            });
        }

        [HttpPost]
        public IActionResult AjouterNote(LocationNote model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var location = _context.Locations
                .Include(l => l.Notes)
                .FirstOrDefault(l => l.Id == model.LocationId);

            if (location == null)
            {
                TempData["Erreur"] = "[ERREUR] La location est introuvable.";
                return RedirectToAction("List");
            }

            try
            {
                location.Notes.Add(new NoteBD(model.Texte.Trim()));
                _context.SaveChanges();

                TempData["Succes"] = "La note a été ajoutée avec succès.";
                return RedirectToAction("Details", new { id = model.LocationId });
            }
            catch (Exception)
            {
                TempData["Erreur"] = "[ERREUR] Impossible d'ajouter la note.";
                return RedirectToAction("Details", new { id = model.LocationId });
            }
        }
    }
}
