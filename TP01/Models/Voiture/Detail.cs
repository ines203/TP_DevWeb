namespace TP01.Models.GestionVoiture
{
    public class Detail
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
    }
}
