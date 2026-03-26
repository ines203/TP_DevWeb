using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Succursale
{
    public class List
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public StatutSuccursale Statut { get; set; }
        public int NbVoitureDispo { get; set; }
        public int NbVoitureIndispo { get; set; }
        public int NbVoitureArchiver { get; set; }

    }
}
