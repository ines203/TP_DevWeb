using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;

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
            var voitures = _context.Voitures.ToList();
            return View(voitures);
        }

        [HttpGet]
        public IActionResult Detail()
        {
            var voitures = _context.Voitures.ToList();
            return View(voitures);
        }
    }
}
