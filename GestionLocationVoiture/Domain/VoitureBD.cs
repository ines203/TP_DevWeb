namespace TP01.Models
{
    public enum EtatVoiture
    {
        NEUF,
        USAGE
    }
    public enum StatutVoiture
    {
        ACTIVE,
        DESACTIVE,
        ARCHIVE
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

        //cles étrangères
        public int SuccursaleId { get; set; }

        // Propriétés de navigation
        public virtual SuccursaleBD Succursale { get; set; }

        public virtual ICollection<NoteBD> Notes { get; set; }
        public virtual ICollection<LocationBD> Locations { get; set; }



        protected VoitureBD() { } 

        public VoitureBD(string surnom, StatutVoiture statut, bool disponible, EtatVoiture etat, string numeroDeSerie, string numeroImmatriculation, string marque, string modele, int annee, string couleur, int kilometrage, decimal valeurEstime)
        {
            Surnom = surnom;
            Statut = statut;
            Disponible = disponible;
            Etat = etat;
            NumeroDeSerie = numeroDeSerie;
            NumeroImmatriculation = numeroImmatriculation;
            Marque = marque;
            Modele = modele;
            Annee = annee;
            Couleur = couleur;
            Kilometrage = kilometrage;
            ValeurEstime = valeurEstime;
        }
    }
}
