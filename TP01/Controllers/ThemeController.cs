using Microsoft.AspNetCore.Mvc;

namespace TP01.Controllers
{
    public class ThemeController : Controller
    {
        public IActionResult Set(string theme, string returnUrl)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1)
            };

            Response.Cookies.Append("Theme", theme, options);

            return Redirect(returnUrl ?? "/");
        }
    }
}
