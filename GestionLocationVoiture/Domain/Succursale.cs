namespace TP01.Models
{
    public enum StatutSuccursale
    {
        ACTIVE,
        DESACTIVE
    }
    public class Succursale
    {
        // Propriétés metier
        public int Id { get; set; }
        public StatutSuccursale Statut { get; set; }

        //cles étrangères
        public int AdresseId { get; set; }

        // Propriétés de navigation
        public virtual Adresse Adresse { get; set; }

        private Succursale() { }
    }
}
