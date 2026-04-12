using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SystemeDeLocation.Website.Models.Succursale
{
    public class SuccursaleCreation
    {
        [Required(ErrorMessage = "Le nom de succursale est requis.")]
        [Display(Name = "Nom de succursale")]
        public string Nom { get; set; }

        [RegularExpression(@"\d*",
            ErrorMessage = "Le numéro civique ne peut contenir que des chiffres.")]
        [Display(Name = "Numéro civique")]
        public int? NumeroCivique { get; set; }

        [Display(Name = "Rue")]
        public string? Rue { get; set; }

        [Display(Name = "Ville")]
        public string? Ville { get; set; }

        // Prend en charge les codes postaux et les ZIP
        [RegularExpression(@"[A-Za-z]{1}\d{1}[A-Za-z]{1} ?\d{1}[A-Za-z]{1}\d{1}|\d{5}",
            ErrorMessage = "Code postal invalide.")]
        [Display(Name = "Code Postal")]
        public string? CodePostal { get; set; }
    }
}
