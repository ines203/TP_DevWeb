using Microsoft.AspNetCore.Mvc;

namespace TP01.Controllers
{
    public class BaseController : Controller
    {
        protected bool usagerEstAuthentifier()
        {
            return HttpContext.Session.GetInt32("UtilisateurId") != null;
        }
    }
}
