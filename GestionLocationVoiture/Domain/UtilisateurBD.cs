using Microsoft.AspNetCore.Identity;

namespace SystemeDeLocation.Core.Domain
{
    public class UtilisateurBD : IdentityUser<Guid>
    {

        public int? SuccursaleId { get; set; }
        public virtual SuccursaleBD? Succursale { get; set; }

        private UtilisateurBD() { }
        private UtilisateurBD(string userName) : base(userName) { }

        public static UtilisateurBD Create(string nomUtilisateur, SuccursaleBD succursale)
        {
            if(succursale != null)
                return new(nomUtilisateur) { Id = Guid.NewGuid(), Succursale = succursale, SuccursaleId = succursale.Id };
            else
                return new(nomUtilisateur) { Id = Guid.NewGuid() };

        }
    }
}
