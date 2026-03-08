using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Location
{
    public class List
    {
        //conducteur
        public string Prenom { get; set; }
        public string Nom { get; set; }

        //voiture
        public string Surnom { get; set; }

        //location
        public int LocationId { get; set; }
        public StatutLocation StatutLoc { get; set; }
        public DateTime Ouverture { get; set; }
        public DateTime FermeturePlanifiee { get; set; }
        public DateTime? FermetureReelle { get; set; }
    }
}
