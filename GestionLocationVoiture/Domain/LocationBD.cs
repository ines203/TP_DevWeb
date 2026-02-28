namespace TP01.Models
{
    public enum StatutLocation
    {
        OUVERT,
        FERME
    }
    public class LocationBD
    {
        public int Id { get; set; }
        public StatutLocation Statut { get; set; }
        public DateTime Ouverture { get; set; }
        public DateTime FermeturePlanifiee { get; set; }
        public DateTime? FermetureReelle { get; set; }

        //cles étrangères
        public int VoitureId { get; set; }
        public int ConducteurId { get; set; }

        // Propriétés de navigation
        public virtual VoitureBD Voiture { get; set; }
        public virtual ConducteurBD Conducteur { get; set; }
        public virtual ICollection<NoteBD> Notes { get; set; }

        protected LocationBD() { }

        public LocationBD(int voitureId, int conducteurId, DateTime fermeturePlanifiee)
        {
            VoitureId = voitureId;
            ConducteurId = conducteurId;
            Ouverture = DateTime.Now;
            FermeturePlanifiee = fermeturePlanifiee;
            Statut = StatutLocation.OUVERT;
            Notes = new List<NoteBD>();
            
        }

    }
}
