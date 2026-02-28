namespace TP01.Models
{
    public class NoteBD
    {
        public int Id { get; set; }
        public string Texte { get; set; }
        public DateTime DateCreation { get; set; }

        protected NoteBD() { }

        public NoteBD(string texte)
        {
            Texte = texte;
            DateCreation = DateTime.Now;
        }
    }
}
