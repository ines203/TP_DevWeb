namespace TP01.Models
{
    public class NoteBD
    {
        public int Id { get; set; }
        public string Texte { get; set; }
        public int VoitureId { get; set; }

        public virtual VoitureBD Voiture { get; set; }

        protected NoteBD() { }

        public NoteBD(string texte)
        {
            Texte = texte;
        }
    }
}
