using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemeDeLocation.Core.Data;

namespace SystemeDeLocation.Website.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly GestionVoitureContext _context;
        protected readonly UserManager<IdentityUser> _userManager;


        public BaseController(GestionVoitureContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected string ObtenirRoleUsager()
        {
            if (User.IsInRole("ADMINISTRATEUR")) return "ADMINISTRATEUR";
            if (User.IsInRole("GERANT")) return "GERANT";
            if (User.IsInRole("COMMIS")) return "COMMIS";
            return "UTILISATEUR";
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
            return "List";
        }
    }
}