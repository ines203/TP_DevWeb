using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SystemeDeLocation.Website.Models.Utilisateur
{
    public class UtilisateurCreation
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "Le nom d'utilisateur doit contenir entre 5 et 20 caractères.")]
        [RegularExpression(@"^(?=.*[a-zA-Z0-9])[a-zA-Z0-9_]+$",
        ErrorMessage = "Le nom d'utilisateur doit contenir au moins une lettre ou un chiffre.")]
        [Display(Name = "Nom d'utilisateur")]
        public string? NomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [Display(Name = "Mot de passe")]
        public string? MotDePasse { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        [Display(Name = "Confirmer le mot de passe")]
        public string? ConfirmationMotDePasse { get; set; }

        [Required(ErrorMessage = "Le rôle est requis.")]
        [Display(Name = "Rôle")]
        public string? Role { get; set; }

        [Display(Name = "Succursale")]
        public int? SuccursaleId { get; set; }

        // Listes pour les dropdowns
        public List<string> Roles { get; set; } = new();
        public List<SelectListItem> Succursales { get; set; } = new();
    }
}