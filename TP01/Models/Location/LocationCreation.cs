

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationCreation
    {
        //conducteur
        public string? Prenom { get; set; }
        public string? Nom { get; set; }
        public string? NumeroDeTelephone { get; set; }
        public string? AdresseCourriel { get; set; }
        public string? NumeroDuPermis { get; set; }
        public bool ValiderIdentiteConducteur { get; set; }
        public bool PreuveAssurance { get; set; }

        //Adresse civique
        public int NumeroCivique { get; set; }
        public string? Rue { get; set; }
        public string? Ville { get; set; }
        public string? CodePostal { get; set; }


        //voiture
        public int VoitureId { get; set; }

        //succursale
        public int SuccursaleId { get; set; }

        //location
        public DateTime FermeturePlanifiee { get; set; } = DateTime.Now.AddDays(1);

        //Note
        public string? Note { get; set; }

    }
}
