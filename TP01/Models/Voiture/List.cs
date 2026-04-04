using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Voiture
{
    public class List
    {
        public int SuccursaleId { get; set; }
        public int VoitureId { get; set; }

        public string? Surnom { get; set; }
        public string? NumeroImmatriculation { get; set; }
        public string? Marque { get; set; }
        public string? Modele { get; set; }
        public int Annee { get; set; }
        public string? Couleur { get; set; }
        public StatutVoiture Statut { get; set; }
        public bool Disponible { get; set; }
    }
}
