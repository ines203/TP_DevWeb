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
        public int NoteId { get; set; }

        // Propriétés de navigation
        public virtual NoteBD Note { get; set; }


        protected VoitureBD() { } 
    }
}
