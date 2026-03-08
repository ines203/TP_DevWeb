namespace SystemeDeLocation.Core.Domain
{
    public enum StatutSuccursale
    {
        ACTIVE,
        DESACTIVE
    }
    public class SuccursaleBD
    {
        // Clé primaire
        public int Id { get; set; }

        // Propriétés métier
        public StatutSuccursale Statut { get; set; }

        public string Nom { get; set; }

        // Clé étrangère
        public int? AdresseId { get; set; }

        // Propriété de navigation
        public virtual AdresseBD Adresse { get; set; }

        public virtual ICollection<VoitureBD> Voitures { get; set; } = new List<VoitureBD>();

        // Constructeur requis par EF
        protected SuccursaleBD() { }

        public SuccursaleBD(string nom, StatutSuccursale statut)
        {
            Nom = nom;
            Statut = statut;
            AdresseId = 0;
        }
    }
}
