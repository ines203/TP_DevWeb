namespace TP01.Models
{
    public class NoteBD
    {
        public int Id { get; set; }
        public string Texte { get; set; }
        public DateTime DateCreation { get; set; }
        // Clé étrangère vers la voiture
        public int VoitureId { get; set; }

        // Propriété de navigation
        public virtual VoitureBD Voiture { get; set; }

        protected NoteBD() { }

        public NoteBD(string texte)
        {
            Texte = texte;
            DateCreation = DateTime.Now;
        }
    }
}
