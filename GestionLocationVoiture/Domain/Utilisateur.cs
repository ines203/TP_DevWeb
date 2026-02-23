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
        public required int Id { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Email { get; set; }
        public required string MotDePasse { get; set; }
        public Role Role { get; set; }

        protected Utilisateur() { }
        public Utilisateur(int id, string nom, string prenom, string email, string motDePasse, Role role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
        }
    }
}
