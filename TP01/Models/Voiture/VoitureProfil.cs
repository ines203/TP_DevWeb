using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Voiture
{
    public class VoitureProfil
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
        public ICollection<NoteBD> Notes { get; set; } = new List<NoteBD>();
        public ICollection<LocationBD> Locations { get; set; } = new List<LocationBD>();
        public int succursaleId { get; set; }
        public string nomSuccursale { get; set; }
    }
}
