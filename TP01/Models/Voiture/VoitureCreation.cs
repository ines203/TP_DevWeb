using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Voiture
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

	public class VoitureCreation
    {
        [HiddenInput]
        public int SuccursaleId { get; set; }

        [Required(ErrorMessage = "Le surnom est obligatoire.")]
        [Display(Name = "Surnom")]
        [RegularExpression(@"\S+", ErrorMessage = "Le surnom ne peut pas contenir uniquement des espaces.")]
        public string Surnom { get; set; }

        [Required(ErrorMessage = "Le numéro de série est obligatoire.")]
        [Display(Name = "Numéro de série")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le numéro de série doit contenir exactement 17 caractères.")]
        [RegularExpression("^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Le numéro de série est invalide (caractères interdits ou format incorrect).")]
        public string NumeroDeSerie { get; set; }

        [Required(ErrorMessage = "Le numéro d'immatriculation est obligatoire.")]
        [Display(Name = "Numéro d'immatriculation")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Le numéro d'immatriculation doit contenir entre 2 et 10 caractères.")]
        [RegularExpression(@"^[A-Z0-9 ]+$", ErrorMessage = "Le numéro d'immatriculation ne doit contenir que des lettres et des chiffres.")]
        public string NumeroImmatriculation { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire.")]
        [Display(Name = "Marque")]
        [RegularExpression(@"\S+", ErrorMessage = "La marque ne peut pas contenir uniquement des espaces.")]
        public string Marque { get; set; }

        [Required(ErrorMessage = "Le modèle est obligatoire.")]
        [Display(Name = "Modèle")]
        [RegularExpression(@"\S+", ErrorMessage = "Le modèle ne peut pas contenir uniquement des espaces.")]
        public string Modele { get; set; }

		[Required(ErrorMessage = "L'année est obligatoire.")]
        [Display(Name = "Année")]
        [Range(1900, 9999, ErrorMessage = $"L'année doit être comprise entre 1900 et l'année courante.")]
        public int? Annee { get; set; }

        [Required(ErrorMessage = "La couleur est obligatoire.")]
        [Display(Name = "Couleur")]
        [RegularExpression(@"\S+", ErrorMessage = "La couleur ne peut pas contenir uniquement des espaces.")]
        public string Couleur { get; set; }

        [Required(ErrorMessage = "L'état est obligatoire.")]
        [Display(Name = "État")]
        public EtatVoiture? Etat { get; set; }

        [Display(Name = "Kilométrage")]
        [Range(0, int.MaxValue, ErrorMessage = "Le kilométrage doit être un nombre positif.")]
        public int? Kilometrage { get; set; }

        [Display(Name = "Valeur estimée")]
        [Range(0, 100000000, ErrorMessage = "La valeur estimée doit être positive.")]
        public decimal? ValeurEstime { get; set; }

        [Display(Name = "Note initiale")]
        public string? NoteInitiale { get; set; }
    }
}
