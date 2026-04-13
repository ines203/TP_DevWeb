namespace SystemeDeLocation.Core.Domain
{
    public enum Role
    {
        UTILISATEUR,
        COMMIS,
        GERANT,
        ADMINISTRATEUR
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
        public UtilisateurBD(string nomUtilisateur, string motDePasse, Role role, SuccursaleBD succursale)
        {
            NomUtilisateur = nomUtilisateur;
            MotDePasse = motDePasse;
            Role = role;
            Succursale = succursale;
            SuccursaleId = succursale.Id;
        }
    }
}
