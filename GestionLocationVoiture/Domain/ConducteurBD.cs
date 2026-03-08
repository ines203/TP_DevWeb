namespace SystemeDeLocation.Core.Domain
{
    public class ConducteurBD
    {
        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string AdresseCourriel { get; set; }
        public string NumeroDeTelephone { get; set; }
        public string NumeroDuPermis { get; set; }

        //cles étrangères
        public int AdresseId { get; set; }

        // Propriétés de navigation
        public virtual AdresseBD Adresse { get; set; }

        // Navigation vers la location
        public virtual LocationBD Location { get; set; }

        protected ConducteurBD() { }
        public ConducteurBD(string prenom, string nom, string adresseCourriel,
            string numeroDeTelephone, string numeroDuPermis, int adresseId)
        {
            Prenom = prenom;
            Nom = nom;
            AdresseCourriel = adresseCourriel;
            NumeroDeTelephone = numeroDeTelephone;
            NumeroDuPermis = numeroDuPermis;
            AdresseId = adresseId;
        }
    }
}
