using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;

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
                .ToList();

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

            return View(conducteur);
        }
    }
}

