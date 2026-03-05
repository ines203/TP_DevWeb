using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;
using TP01.Models.Voiture;

namespace TP01.Controllers
{
    public class VoitureController : BaseController
    {
        private readonly GestionVoitureContext _context;

        public VoitureController(GestionVoitureContext context)
        {
            _context = context;
        }
        // UC08 - Liste des voitures d'une succursale
        [HttpGet]
        public IActionResult List(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            if (HttpContext.Session.GetString("RoleUtilisateur") == "CLIENT")
            {
                return RedirectToAction("List", "Succursale");
            }

            var succursale = _context.Succursales.Include(s => s.Voitures).FirstOrDefault(s => s.Id == id);

            if (succursale == null)
            {
                TempData["Erreur"] = "[ERREUR] La succursale n existe pas. Impossible d afficher les voitures.";
                return RedirectToAction("List", "Succursale");
            }

            var voitures = succursale.Voitures
                .Where(v => v.Statut != StatutVoiture.ARCHIVE)
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
                }); ;

            ViewBag.SuccursaleId = id;
            ViewBag.SuccursaleNom = succursale.Nom;

            return View(voitures);
        }
        // UC10 - Consulter le profil d'une voiture
        [HttpGet]
        public IActionResult Profil(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");


            if (HttpContext.Session.GetString("RoleUtilisateur") == "CLIENT")
            {
                return RedirectToAction("List", "Succursale");
            }

            var voit = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voit == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            var succursale = _context.Succursales.FirstOrDefault(s => s.Id == voit.SuccursaleId);

            var voitureDetail = new Profil
            {
                Id = voit.Id,
                Surnom = voit.Surnom,
                Statut = voit.Statut,
                Disponible = voit.Disponible,
                Etat = voit.Etat,
                NumeroDeSerie = voit.NumeroDeSerie,
                NumeroImmatriculation = voit.NumeroImmatriculation,
                Marque = voit.Marque,
                Modele = voit.Modele,
                Annee = voit.Annee,
                Couleur = voit.Couleur,
                Kilometrage = voit.Kilometrage,
                ValeurEstime = voit.ValeurEstime,
                Notes = voit.Notes,
                Locations = voit.Locations,
                succursaleId = voit.SuccursaleId,
                nomSuccursale = succursale.Nom
            };

            return View(voitureDetail);
        }
        // UC12 - Désactiver une voiture (Commis)
        [HttpPost]
        public IActionResult Desactiver(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

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
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            var role = HttpContext.Session.GetString("RoleUtilisateur");
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
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures.Include(n => n.Notes).Include(l => l.Locations).FirstOrDefault(s => s.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", "Succursale");
            }

            // Vérification du rôle
            var role = HttpContext.Session.GetString("RoleUtilisateur");
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
