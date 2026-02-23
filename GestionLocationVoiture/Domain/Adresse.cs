namespace TP01.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public string NumeroCivic { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        protected Adresse() { }
    }
}
