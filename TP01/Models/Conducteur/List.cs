namespace TP01.Models.Conducteur
{
    public class List
    {
        //conducteur
        public int ConducteurId { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }

        //location
        public DateTime Ouverture { get; set; }
        public DateTime FermeturePlanifiee { get; set; }

        //voiture
        public string Surnom { get; set; }
    }
}
