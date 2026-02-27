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
        public DateTime Fermeture { get; set; }
        public DateTime FermetureReel { get; set; }

        //cles étrangères
        public int NoteId { get; set; }

        // Propriétés de navigation
        public virtual NoteBD Note { get; set; }

        protected LocationBD() { }

    }
}
