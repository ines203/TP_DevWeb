namespace SystemeDeLocation.Core.Domain
{
    public enum EtatVoiture
    {
        NEUF,
        USAGE
    }
    public enum StatutVoiture
    {
        ACTIVE,
        DESACTIVE
    }

    public class VoitureBD
    {
        public int Id { get; set; }
        public string Surnom { get; set; }
        public StatutVoiture Statut { get; set; }
        public bool Disponible { get; set; }
        public EtatVoiture Etat { get; set; }
        public string NumeroDeSerie { get; set; }
        public string NumeroImmatriculation { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Annee { get; set; }
        public string Couleur { get; set; }
        public int Kilometrage { get; set; }
        public decimal ValeurEstime { get; set; }
        public bool Archiver {  get; set; }

        // Clé étrangère vers la succursale
        public int SuccursaleId { get; set; }
        public virtual SuccursaleBD Succursale { get; set; }

        // Collection de notes
        public virtual ICollection<NoteBD> Notes { get; set; } = new List<NoteBD>();

        // Journal des locations
        public virtual ICollection<LocationBD> Locations { get; set; } = new List<LocationBD>();

        protected VoitureBD() { }

        public VoitureBD(string surnom, string numeroDeSerie, string numeroImmatriculation,
            string marque, string modele, int annee, string couleur, EtatVoiture etat,
            int succursaleId, int kilometrage = 0, decimal valeurEstime = 0)
        {
            Surnom = surnom;
            NumeroDeSerie = numeroDeSerie;
            NumeroImmatriculation = numeroImmatriculation;
            Marque = marque;
            Modele = modele;
            Annee = annee;
            Couleur = couleur;
            Etat = etat;
            SuccursaleId = succursaleId;
            Kilometrage = kilometrage;
            ValeurEstime = valeurEstime;
            Statut = StatutVoiture.ACTIVE;
            Disponible = true;
        }
    }
}
