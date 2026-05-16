using System.ComponentModel.DataAnnotations;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationNote
    {
        public int LocationId { get; set; }
        public int SuccursaleId { get; set; }

        [Required(ErrorMessage = "Le texte de la note est requis.")]
        [StringLength(500, MinimumLength = 1,
            ErrorMessage = "La note doit contenir entre 1 et 500 caractères.")]
        [Display(Name = "Note")]
        public string Texte { get; set; }
    }
}
