using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;

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
        public IActionResult List(int succursaleId)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var succursale = _context.Succursales.Find(succursaleId);
            if (succursale == null)
            {
                TempData["Erreur"] = "[ERREUR] La succursale est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            var voitures = _context.Voitures
                .Where(v => v.SuccursaleId == succursaleId
                         && v.Statut != StatutVoiture.ARCHIVE)
                .ToList();

            ViewBag.SuccursaleId = succursaleId;
            ViewBag.SuccursaleNom = succursale.Nom;

            return View(voitures);
        }
        // UC10 - Consulter le profil d'une voiture
        [HttpGet]
        public IActionResult Profil(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures
                .Include(v => v.Notes)
                .Include(v => v.Locations)
                    .ThenInclude(l => l.Conducteur)
                .Include(v => v.Succursale)
                .FirstOrDefault(v => v.Id == id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            return View(voiture);
        }
        // UC12 - Désactiver une voiture (Commis)
        [HttpPost]
        public IActionResult Desactiver(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures.Find(id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            if (voiture.Statut != StatutVoiture.ACTIVE || !voiture.Disponible)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être active et disponible pour être désactivée.";
                return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.DESACTIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été désactivée avec succès.";
            return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
        }
        // UC13 - Activer une voiture (Gérant)
        [HttpPost]
        public IActionResult Activer(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures.Find(id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            if (voiture.Statut != StatutVoiture.DESACTIVE)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être désactivée pour être activée.";
                return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.ACTIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été activée avec succès.";
            return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
        }
        // UC14 - Archiver une voiture (Gérant)
        [HttpPost]
        public IActionResult Archiver(int id)
        {
            if (!usagerEstAuthentifier())
                return RedirectToAction("Login", "Auth");

            var voiture = _context.Voitures.Find(id);

            if (voiture == null)
            {
                TempData["Erreur"] = "[ERREUR] La voiture est introuvable.";
                return RedirectToAction("List", "Succursale");
            }

            if (voiture.Statut != StatutVoiture.DESACTIVE)
            {
                TempData["Erreur"] = "[ERREUR] La voiture doit être désactivée pour être archivée.";
                return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
            }

            voiture.Statut = StatutVoiture.ARCHIVE;
            _context.SaveChanges();

            TempData["Succes"] = $"La voiture {voiture.Surnom} a été archivée avec succès.";
            return RedirectToAction("List", new { succursaleId = voiture.SuccursaleId });
        }
    }
        //[HttpGet]
        //public IActionResult List()
        //{
        //    var voitures = _context.Voitures.ToList();
        //    return View(voitures);
        //}
    
}
