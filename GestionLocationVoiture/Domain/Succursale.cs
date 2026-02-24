namespace TP01.Models
{
    public enum StatutSuccursale
    {
        ACTIVE,
        DESACTIVE
    }
    public class Succursale
    {
        // Clé primaire
        public int Id { get; set; }

        // Propriétés métier
        public StatutSuccursale Statut { get; set; }

        public string Nom { get; set; }

        // Clé étrangère
        public int AdresseId { get; set; }

        // Propriété de navigation
        public virtual Adresse Adresse { get; set; }

        public virtual ICollection<Voiture> Voitures { get; set; }

        // Constructeur requis par EF
        protected Succursale() { }

        public Succursale(string nom, StatutSuccursale statut)
        {
            Nom = nom;
            Statut = statut;
            AdresseId = 0;
            Voitures = new List<Voiture>();
        }
    }
}
