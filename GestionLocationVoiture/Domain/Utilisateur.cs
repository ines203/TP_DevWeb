namespace TP01.Models
{
    public enum Role
    {
        ADMINISTRATEUR,
        GERANT,
        COMMIS,
        CLIENT
    }
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public Role Role { get; set; }

        protected Utilisateur() { }
        public Utilisateur (string nom, string prenom, string email, string motDePasse, Role role)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
        }
    }
}
