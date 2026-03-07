using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP01.Models;
using TP01.Models.Auth;

namespace TP01.Controllers
{
    public class AuthController : BaseController
    {

        public AuthController(GestionVoitureContext context) : base(context)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (UsagerEstAuthentifier())
                return RetourPageAccueilUtilisateur();
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var utilisateur = _context.Utilisateurs
                .FirstOrDefault(u => u.NomUtilisateur == model.NomUtilisateur
                                  && u.MotDePasse == model.MotDePasse);

            if (utilisateur == null)
            {
                ViewBag.ErrorMessage = "Nom d'utilisateur ou mot de passe invalide.";
                return View(model);
            }

            HttpContext.Session.SetInt32("UtilisateurId", utilisateur.Id);
            HttpContext.Session.SetString("RoleUtilisateur", utilisateur.Role.ToString());

            // Redirection 
            return RetourPageAccueilUtilisateur();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RetourAuth();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
