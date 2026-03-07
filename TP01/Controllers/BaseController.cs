using GestionLocationVoiture.Data;
using Microsoft.AspNetCore.Mvc;

namespace TP01.Controllers
{
    public class BaseController : Controller
    {
        protected readonly GestionVoitureContext _context;

        public BaseController(GestionVoitureContext context)
        {
            _context = context;
        }

        protected bool UsagerEstAuthentifier()
        {
            return HttpContext.Session.GetInt32("UtilisateurId") != null;
        }

        protected string ObtenirRoleUsager()
        {
            return HttpContext.Session.GetString("RoleUtilisateur") ?? string.Empty;
        }

        protected IActionResult RetourAuth()
        {
            return RedirectToAction("Login", "Auth");
        }

        protected IActionResult RetourPageAccueilUtilisateur()
        {
            return RedirectToAction(GetDefaultAction(), GetDefaultController());
        }

        private string GetDefaultController()
        {
            var role = ObtenirRoleUsager();
            return role switch
            {
                "ADMINISTRATEUR" => "Succursale",
                "GERANT" => "Location",
                "COMMIS" => "Voiture",
                _ => "Location"
            };
        }
        private string GetDefaultAction()
        {
            var role = ObtenirRoleUsager();
            return role switch
            {
                "ADMINISTRATEUR" => "List",
                "GERANT" => "List",
                "COMMIS" => "List",
                _ => "List"
            };
        }
    }
}
