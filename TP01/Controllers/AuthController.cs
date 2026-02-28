using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP01.Models;
using TP01.Models.Auth;

namespace TP01.Controllers
{
    public class AuthController : BaseController
    {

        private readonly GestionVoitureContext _context;

        public AuthController(GestionVoitureContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            //return View();
            return RedirectToAction("Detail", "Voiture");
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

            // Stocker les informations de l'utilisateur dans la session
            HttpContext.Session.SetInt32("UtilisateurId", utilisateur.Id); 
            HttpContext.Session.SetString("RoleUtilisateur", utilisateur.Role.ToString());

            return RedirectToAction("List", "Succursale");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
