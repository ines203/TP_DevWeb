namespace TP01.Models.Location
{
    public class Details
    {
        //conducteur
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string AdresseCourriel { get; set; }
        public string NumeroDeTelephone { get; set; }
        public string NumeroDuPermis { get; set; }
        public string NomSuccursale { get; set; }

        //voiture
        public string Surnom { get; set; }
        public StatutVoiture StatutVoit { get; set; }
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

        //location
        public int LocationId { get; set; }
        public StatutLocation StatutLoc { get; set; }
        public DateTime Ouverture { get; set; }
        public DateTime FermeturePlanifiee { get; set; }
        public DateTime? FermetureReelle { get; set; }

        //Notes
        public ICollection<NoteBD> Notes { get; set; } = new List<NoteBD>();
    }
}
