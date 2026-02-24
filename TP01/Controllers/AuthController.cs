using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP01.Models;
using TP01.Models.Auth;

namespace TP01.Controllers
{
    public class AuthController : Controller
    {

        private readonly GestionVoitureContext _context;

        public AuthController(GestionVoitureContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var utilisateur = _context.Utilisateurs
                .FirstOrDefault(u => u.Email == model.Email && u.MotDePasse == model.MotDePasse);

            if (utilisateur == null)
            {
                ViewBag.ErrorMessage = "Email ou mot de passe invalide";
                return View(model);
            }

            return RedirectToAction("List", "Succursale");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
