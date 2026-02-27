namespace TP01.Models
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

        protected ConducteurBD() { }
    }
}
