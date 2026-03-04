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

        [HttpGet]
        public IActionResult List(int id)
        {
            if (!usagerEstAuthentifier())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (HttpContext.Session.GetString("RoleUtilisateur") == "CLIENT")
            {
                return RedirectToAction("List", "Succursale");
            }

            var succursale = _context.Succursales.Include(s => s.Voitures).FirstOrDefault(s => s.Id == id);
            if (succursale == null)
            {
                TempData["Message"] = "[ERREUR] La succursale n existe pas. Impossible d afficher les voitures.";
                return RedirectToAction("List", "Succursale");
            }
            var voitures = succursale.Voitures.Where(voit => voit.Statut != StatutVoiture.ARCHIVE).Select( voit => new List
            {
                SuccursaleId = voit.SuccursaleId,
                VoitureId = voit.Id,
                Surnom = voit.Surnom,
                NumeroImmatriculation = voit.NumeroImmatriculation,
                Marque = voit.Marque,
                Modele = voit.Modele,
                Couleur = voit.Couleur,
                Annee = voit.Annee
            });
            return View(voitures);
        }

        [HttpGet]
        public IActionResult Detail(int succursaleId, int voitureId)
        {
            if (!usagerEstAuthentifier())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (HttpContext.Session.GetString("RoleUtilisateur") == "CLIENT")
            {
                return RedirectToAction("List", "Succursale");
            }

            var succursale = _context.Succursales.Include(s => s.Voitures).ThenInclude(n => n.Notes).Include(s => s.Voitures).ThenInclude(l => l.Locations).FirstOrDefault(s => s.Id == succursaleId);
            if (succursale == null)
            {
                TempData["Message"] = "[ERREUR] La succursale n existe pas. Impossible d afficher les voitures.";
                return RedirectToAction("List", "Succursale");
            }

            var voit = succursale.Voitures.FirstOrDefault(v => v.Id == voitureId);
            if (voit == null)
            {
                TempData["Message"] = "[ERREUR] La voiture n existe pas. Impossible d afficher les details de la voiture.";
                return RedirectToAction("List", new { id = succursaleId });
            }

            var voitureDetail = new Detail
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
                nomSuccursale = succursale.Nom
            };
            return View(voitureDetail);
        }
    }
}
