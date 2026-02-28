using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult List()
        {
            var voitures = _context.Voitures.Select( voit => new List
            {
                Id = voit.Id,
                Surnom = voit.Surnom,
                Statut = voit.Statut,
                Disponible = voit.Disponible,
                Etat = voit.Etat,
                Marque = voit.Marque,
                Modele = voit.Modele,
                Annee = voit.Annee,
                Couleur = voit.Couleur
            });
            return View(voitures);
        }

        [HttpGet]
        public IActionResult Detail()
        {
            var voitures = _context.Voitures.Select(voit => new Detail
            {
                Id = voit.Id,
                Surnom = voit.Surnom,
                Statut = voit.Statut,
                Disponible = voit.Disponible,
                Etat = voit.Etat,
                NumeroDeSerie = voit.NumeroDeSerie,
                Marque = voit.Marque,
                Modele = voit.Modele,
                Annee = voit.Annee,
                Couleur = voit.Couleur,
                Kilometrage = voit.Kilometrage,
                ValeurEstime = voit.ValeurEstime
            });
            return View(voitures);
        }
    }
}
