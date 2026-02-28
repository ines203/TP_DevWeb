using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP01.Models;

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

            if (succursaleId.HasValue)
                query = query.Where(l => l.Voiture.SuccursaleId == succursaleId);

            var locations = query.ToList();

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

            return View(location);
        }
    }
}

