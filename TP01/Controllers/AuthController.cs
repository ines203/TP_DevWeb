using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemeDeLocation.Website.Models;
using SystemeDeLocation.Website.Models.Auth;


namespace TP01.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToDefaultPage();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.NomUtilisateur, model.MotDePasse,
                isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = "Nom d'utilisateur ou mot de passe invalide.";
                return View(model);
            }

            return RedirectToDefaultPage();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToDefaultPage()
        {
            if (User.IsInRole("ADMINISTRATEUR"))
                return RedirectToAction("List", "Succursale");
            if (User.IsInRole("GERANT"))
                return RedirectToAction("List", "Location");
            if (User.IsInRole("COMMIS"))
                return RedirectToAction("List", "Voiture");
            return RedirectToAction("List", "Location");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}