using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Voiture
{
    public class VoitureCreation
    {
        [HiddenInput]
        [Display(Name = "")]
        public int SuccursaleId { get; set; }

        [Required]
        public string Surnom { get; set; }

        [Required]
        [Display(Name = "Numéro de série")]
        public string NumeroDeSerie { get; set; }

        [Required]
        [Display(Name = "Numéro d'immatriculation")]
        public string NumeroImmatriculation { get; set; }

        [Required]
        public string Marque { get; set; }

        [Required]
        [Display(Name = "Modèle")]
        public string Modele { get; set; }

        [Required]
        [Display(Name = "Année")]
        public int? Annee { get; set; }

        [Required]
        public string Couleur { get; set; }

        [Required]
        [Display(Name = "État")]
        public EtatVoiture? Etat { get; set; }

        [Display(Name = "Kilométrage")]
        public int? Kilometrage { get; set; }

        [Display(Name = "Valeur estimée")]
        public decimal? ValeurEstime { get; set; }

        [Display(Name = "Note initiale")]
        public string NoteInitiale { get; set; }
    }
}
