using System.ComponentModel.DataAnnotations;

namespace SystemeDeLocation.Website.Models.Utilisateur
{
    public class UtilisateurModificationMdp
    {
        public Guid? Id { get; set; }
        public string? NomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [Display(Name = "Mot de passe")]
        public string? MotDePasse { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        [Display(Name = "Confirmer le mot de passe")]
        public string? ConfirmationMotDePasse { get; set; }
    }
}
