using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;
using TP01.Models.Voiture;

namespace TP01.Controllers
{
    public class VoitureController : BaseController
    {
        public VoitureController(GestionVoitureContext context) : base(context)
        {
        }

        // UC08 - Liste des voitures d'une succursale
        [HttpGet]
        public IActionResult List(int id)
        {
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            if (ObtenirRoleUsager() == "CLIENT")
            {
                return RetourPageAccueilUtilisateur();
            }

            var listeVoitureSuccursale = _context.Voitures
                .AsNoTracking()
                .Where(v => v.SuccursaleId == id && v.Statut != StatutVoiture.ARCHIVE)
                .Select(voit => new List
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
                TempData["Erreur"] = "[ERREUR] La succursale n existe pas. Impossible d afficher les voitures.";
                return RedirectToAction("List", "Succursale");
            }

            return View(listeVoitureSuccursale);
        }
        // UC10 - Consulter le profil d'une voiture
        [HttpGet]
        public IActionResult Profil(int id)
        {
            if (!UsagerEstAuthentifier())
                return RetourAuth();


            if (ObtenirRoleUsager() == "CLIENT")
            {
                return RetourPageAccueilUtilisateur();
            }

            var voit = _context.Voitures
                .AsNoTracking()
                .Where(v => v.Id == id)
                .Select(v => new Profil
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
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }


            return View(voit);
        }
        // UC12 - Désactiver une voiture (Commis)
        [HttpPost]
        public IActionResult Desactiver(int id)
        {
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            var voiture = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
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
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            var voiture = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            var role = ObtenirRoleUsager();
            if (role != "GERANT" && role != "ADMINISTRATEUR")
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
            if (!UsagerEstAuthentifier())
                return RetourAuth();

            var voiture = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            var role = ObtenirRoleUsager();
            if (role != "GERANT" && role != "ADMINISTRATEUR")
            {
                TempData["Erreur"] = "[ERREUR] Vous n'avez pas les droits pour effectuer cette action.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            if (voiture.Statut != StatutVoiture.DESACTIVE)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être désactivée pour être archivée.";
                return RedirectToAction("List", new { id = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.ARCHIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été archivée avec succès.";
            return RedirectToAction("List", new { id = voiture.SuccursaleId });
        }
    }
    
}
