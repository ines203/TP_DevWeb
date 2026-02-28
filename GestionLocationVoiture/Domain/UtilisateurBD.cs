namespace TP01.Models
{
    public enum Role
    {
        ADMINISTRATEUR,
        GERANT,
        COMMIS,
        UTILISATEUR
    }
    public class UtilisateurBD
    {
        public int Id { get; set; }
        public string NomUtilisateur { get; set; }
        public string MotDePasse { get; set; }
        public Role Role { get; set; }

        public int? SuccursaleId { get; set; }
        public virtual SuccursaleBD Succursale { get; set; }

        protected UtilisateurBD() { }
        public UtilisateurBD(string nomUtilisateur, string motDePasse, Role role, int? succursaleId = null)
        {
            NomUtilisateur = nomUtilisateur;
            MotDePasse = motDePasse;
            Role = role;
            SuccursaleId = succursaleId;
        }
    }
}
