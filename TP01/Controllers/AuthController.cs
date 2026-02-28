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
            if (usagerEstAuthentifier())
                return RedirectToAction(GetDefaultAction(), GetDefaultController());
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
            return RedirectToAction(GetDefaultAction(), GetDefaultController());
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        private string GetDefaultController()
        {
            var role = HttpContext.Session.GetString("RoleUtilisateur");
            return role switch
            {
                "ADMINISTRATEUR" => "Succursale",
                "GERANT" => "Location",
                "COMMIS" => "Voiture",
                _ => "Auth"
            };
        }
        private string GetDefaultAction()
        {
            var role = HttpContext.Session.GetString("RoleUtilisateur");
            return role switch
            {
                "ADMINISTRATEUR" => "List",
                "GERANT" => "List",
                "COMMIS" => "List",
                _ => "Login"
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
