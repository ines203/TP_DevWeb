using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using TP01.Models;
using TP01.Models.Succursale;

namespace TP01.Controllers
{
    public class SuccursaleController : BaseController
    {
            private readonly GestionVoitureContext _context;
    
            public SuccursaleController(GestionVoitureContext context)
            {
                _context = context;
            }
    
            [HttpGet]
            public IActionResult List()
            {

                if (!usagerEstAuthentifier())
                {
                    return RedirectToAction("Login", "Auth");
                }

                var succursales = _context.Succursales.Select(suc => new ListSuccursale
                {
                    Id = suc.Id,
                    Nom = suc.Nom,
                    Statut = suc.Statut,
                }).ToList();

                return View(succursales);
            }

            [HttpPost]
            public IActionResult changementStatue(int id)
            {
                var succursale = _context.Succursales.Find(id);

                if (succursale == null)
                {
                    TempData["MessageEditionSuccursale"] = "[ERREUR] La succursale n'existe pas. Impossible d'activer/désactiver.";
                    return RedirectToAction("List");
                }

                succursale.Statut = succursale.Statut == StatutSuccursale.ACTIVE ? StatutSuccursale.DESACTIVE : StatutSuccursale.ACTIVE;
            
                _context.SaveChanges();

                TempData["MessageEditionSuccursale"] = $"La succursale {succursale.Nom} est maintenant {succursale.Statut}.";
                
                return RedirectToAction("List");
            }
    }
}
