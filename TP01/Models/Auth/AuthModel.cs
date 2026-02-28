using System.ComponentModel.DataAnnotations;

namespace TP01.Models.Auth
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "Le nom d'utilisateur doit contenir entre 5 et 20 caractères.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "Le nom d'utilisateur ne peut contenir que des lettres, chiffres et underscores.")]
        public string NomUtilisateur { get; set; }
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string MotDePasse { get; set; }
    }
}
