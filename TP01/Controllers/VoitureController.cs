using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Voiture;


namespace SystemeDeLocation.Website.Controllers
{
    [Authorize]
    public class VoitureController : BaseController
    {
        public VoitureController(GestionVoitureContext context,
            UserManager<UtilisateurBD> userManager) : base(context, userManager)
        {
        }

        // UC08 - Liste des voitures d'une succursale
        [HttpGet]
        public async Task<IActionResult> List(int? id)
        {
            bool voitureComplet = true;
            var query = _context.Voitures.AsQueryable();

            if(id == null && !User.IsInRole("ADMINISTRATEUR"))
            {
                var user = await _userManager.GetUserAsync(User);
                id = user.SuccursaleId;
            }

            if (id != null)
            {
                query = query.Where(l => l.SuccursaleId == id);
                voitureComplet = false;
            }

            var listeVoitureSuccursale = query
                .AsNoTracking()
                .Where(v => !v.Archiver)
                .Select(voit => new VoitureList
                {
                    SuccursaleId = voit.SuccursaleId,
                    VoitureId = voit.Id,
                    Surnom = voit.Surnom,
                    NumeroImmatriculation = voit.NumeroImmatriculation,
                    Marque = voit.Marque,
                    Modele = voit.Modele,
                    Couleur = voit.Couleur,
                    Annee = voit.Annee,
                    Statut = voit.Statut,
                    Disponible = voit.Disponible
                }).ToList();

            if (listeVoitureSuccursale == null)
            {
                TempData["Erreur"] = "[ERREUR] La succursale n'existe pas. Impossible d'afficher les voitures.";
                return RedirectToAction("List", "Succursale");
            }

            TempData["TouteVoiture"] = voitureComplet;

            return View(listeVoitureSuccursale);
        }

        // UC10 - Consulter le profil d'une voiture
        [HttpGet]
        public IActionResult Profil(int id)
        {
            var voit = _context.Voitures
                .AsNoTracking()
                .Where(v => v.Id == id)
                .Select(v => new VoitureProfil
                {
                    Id = v.Id,
                    Surnom = v.Surnom,
                    Statut = v.Statut,
                    Disponible = v.Disponible,
                    Etat = v.Etat,
                    NumeroDeSerie = v.NumeroDeSerie,
                    NumeroImmatriculation = v.NumeroImmatriculation,
                    Marque = v.Marque,
                    Modele = v.Modele,
                    Annee = v.Annee,
                    Couleur = v.Couleur,
                    Kilometrage = v.Kilometrage,
                    ValeurEstime = v.ValeurEstime,
                    Notes = v.Notes,
                    Locations = v.Locations,
                    succursaleId = v.SuccursaleId,
                    nomSuccursale = v.Succursale.Nom
                })
                .FirstOrDefault();

            if (voit == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n'existe pas. Impossible d'afficher les détails de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            return View(voit);
        }

        // UC12 - Désactiver une voiture (Commis)
        [HttpPost]
        public IActionResult Desactiver(int id)
        {
            var voiture = _context.Voitures
                .Include(n => n.Notes)
                .Include(l => l.Locations)
                .FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n'existe pas.";
                return RedirectToAction("List", "Succursale");
            }

            if (voiture.Statut != StatutVoiture.ACTIVE || !voiture.Disponible)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être active et disponible pour être désactivée.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.DESACTIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été désactivée avec succès.";
            return RedirectToAction("List", new { id = voiture.SuccursaleId });
        }

        // UC13 - Activer une voiture (Gérant seulement)
        [HttpPost]
        public IActionResult Activer(int id)
        {
            var voiture = _context.Voitures
                .Include(n => n.Notes)
                .Include(l => l.Locations)
                .FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n'existe pas.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            if (!User.IsInRole("GERANT") && !User.IsInRole("ADMINISTRATEUR"))
            {
                TempData["Erreur"] = "[ERREUR] Vous n'avez pas les droits pour effectuer cette action.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            if (voiture.Statut != StatutVoiture.DESACTIVE)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être désactivée pour être activée.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.ACTIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été activée avec succès.";
            return RedirectToAction("List", new { id = voiture.SuccursaleId });
        }

        // UC14 - Archiver une voiture (Gérant seulement)
        [HttpPost]
        public IActionResult Archiver(int id)
        {
            var voiture = _context.Voitures
                .Include(n => n.Notes)
                .Include(l => l.Locations)
                .FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n'existe pas.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            if (!User.IsInRole("GERANT") && !User.IsInRole("ADMINISTRATEUR"))
            {
                TempData["Erreur"] = "[ERREUR] Vous n'avez pas les droits pour effectuer cette action.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            if (voiture.Statut != StatutVoiture.DESACTIVE)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être désactivée pour être archivée.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            voiture.Archiver = true;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été archivée avec succès.";
            return RedirectToAction("List", new { id = voiture.SuccursaleId });
        }

        // UC09 - Ajouter une voiture à une succursale (Gérant seulement)
        [HttpGet]
        [Authorize(Roles = "GERANT,ADMINISTRATEUR")]
        public IActionResult Creation(int succursaleId)
        {
            var succursale = _context.Succursales.Find(succursaleId);

            if (succursale == null)
            {
                TempData["Erreur"] = $"[ERREUR] La succursale \"{succursaleId}\" n'existe pas.";
                return RedirectToAction("List", "Succursale");
            }
            else if (succursale.Statut != StatutSuccursale.ACTIVE)
            {
                TempData["Erreur"] = $"[ERREUR] La succursale \"{succursale.Nom}\" ({succursaleId}) n'est pas active.";
                return RedirectToAction("List", "Succursale");
            }

            return View(new VoitureCreation { SuccursaleId = succursaleId });
        }
        [HttpPost]
        [Authorize(Roles = "GERANT,ADMINISTRATEUR")]
        public IActionResult Creation(VoitureCreation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Voitures.Any(v => v.NumeroDeSerie == model.NumeroDeSerie))
            {
                ModelState.AddModelError(string.Empty, $"[VALIDATION] Le numéro de série de cette voiture existe déjà ({model.NumeroDeSerie}).");
                return View(model);
            }

            if (model.Annee > DateTime.Now.Year)
            {
				ModelState.AddModelError("Annee", $"L'année doit être entre 1900 et {DateTime.Now.Year}.");
				return View(model);
			}

            try
            {
                var voiture = new VoitureBD(
                    model.Surnom,
                    model.NumeroDeSerie,
                    model.NumeroImmatriculation,
                    model.Marque,
                    model.Modele,
                    model.Annee ?? 0,
                    model.Couleur,
                    model.Etat.Value,
                    model.SuccursaleId,
                    model.Kilometrage ?? 0,
                    model.ValeurEstime ?? 0
                );

                if (!string.IsNullOrWhiteSpace(model.NoteInitiale))
                {
                    voiture.Notes.Add(new NoteBD(model.NoteInitiale));
                }

                _context.Voitures.Add(voiture);
                _context.SaveChanges();

                TempData["Succes"] = $"Création de la voiture avec succès.";
                return RedirectToAction("List", new { id = model.SuccursaleId });
            }
            catch (Exception ex)
            {
                TempData["Erreur"] = $"[ERREUR] Impossible de créer la voiture: {ex.Message}";
                return View(model);
            }
        }
        // UC11 - Ajouter une note au profil de la voiture
        [HttpGet]
        public IActionResult AjouterNote(int id)
        {
            var voiture = _context.Voitures
                .AsNoTracking()
                .FirstOrDefault(v => v.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            return View(new VoitureNote
            {
                VoitureId = id,
                Surnom = voiture.Surnom
            });
        }

        [HttpPost]
        public IActionResult AjouterNote(VoitureNote model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var voiture = _context.Voitures
                .Include(v => v.Notes)
                .FirstOrDefault(v => v.Id == model.VoitureId);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            try
            {
                voiture.Notes.Add(new NoteBD(model.Texte.Trim()));
                _context.SaveChanges();

                TempData["Succes"] = "La note a été ajoutée avec succès.";
                return RedirectToAction("Profil", new { id = model.VoitureId });
            }
            catch (Exception)
            {
                TempData["Erreur"] = "[ERREUR] Impossible d'ajouter la note.";
                return RedirectToAction("Profil", new { id = model.VoitureId });
            }
        }
    }
}