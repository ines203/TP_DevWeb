using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Conducteur
{
    public class Profil
    {
        //conducteur
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string AdresseCourriel { get; set; }
        public string NumeroDeTelephone { get; set; }
        public string NumeroDuPermis { get; set; }

        //adresse
        public string NumeroCivic { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        //ses locations
        public int LocationId { get; set; }
        public StatutLocation Statut { get; set; }
        public DateTime Ouverture { get; set; }
        public DateTime FermeturePlanifiee { get; set; }

        //voiture liee a la location
        public string Surnom { get; set; }
    }
}
