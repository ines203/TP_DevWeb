using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Data;
using SystemeDeLocation.Core.Domain;
using SystemeDeLocation.Website.Models.Succursale;
using TP01.Controllers;

namespace SystemeDeLocation.Website.Controllers
{
    public class SuccursaleController : BaseController
    {
        public SuccursaleController(GestionVoitureContext context,
            UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var succursales = _context.Succursales
                .AsNoTracking()
                .Select(suc => new List
                {
                    Id = suc.Id,
                    Nom = suc.Nom,
                    Statut = suc.Statut,
                    NbVoitureDispo = suc.Voitures.Count(v => v.Disponible),
                    NbVoitureIndispo = suc.Voitures.Count(v => !v.Disponible),
                    NbVoitureArchiver = suc.Voitures.Count(v => v.Archiver),
                }).ToList();

            return View(succursales);
        }

        [HttpPost]
        public IActionResult ChangementStatue(int id)
        {
            var succursale = _context.Succursales.Find(id);

            if (succursale == null)
            {
                TempData["Erreur"] = "[ERREUR] La succursale n'existe pas. Impossible d'activer/désactiver.";
                return RedirectToAction("List");
            }

            succursale.Statut = succursale.Statut == StatutSuccursale.ACTIVE
                ? StatutSuccursale.DESACTIVE
                : StatutSuccursale.ACTIVE;

            _context.SaveChanges();

            TempData["Succes"] = $"La succursale {succursale.Nom} est maintenant {succursale.Statut}.";
            return RedirectToAction("List");
        }
    }
}