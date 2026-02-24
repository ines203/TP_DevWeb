using System.ComponentModel.DataAnnotations;

namespace TP01.Models.Auth
{
    public class AuthModel
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string MotDePasse { get; set; }
        public AuthModel() { }
        public AuthModel(string email, string motDePasse)
        {
            Email = email;
            MotDePasse = motDePasse;
        }
    }
}
