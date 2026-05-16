using System.ComponentModel.DataAnnotations;

namespace SystemeDeLocation.Website.Models.Voiture
{
    public class VoitureNote
    {
        public int VoitureId { get; set; }
        public string Surnom { get; set; }

        [Required(ErrorMessage = "Le texte de la note est requis.")]
        [StringLength(500, MinimumLength = 1,
            ErrorMessage = "La note doit contenir entre 1 et 500 caractères.")]
        [Display(Name = "Note")]
        public string Texte { get; set; }
    }
}
