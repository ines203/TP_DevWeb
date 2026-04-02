namespace SystemeDeLocation.Core.Domain
{
    public class AdresseBD
    {
        public int Id { get; set; }
        public int NumeroCivique { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        public virtual ConducteurBD Conducteur { get; set; }
        public virtual SuccursaleBD Succursale { get; set; }

        protected AdresseBD() { }

        public AdresseBD(int numeroCivique, string rue, string ville, string codePostal)
        {
            NumeroCivique = numeroCivique;
            Rue = rue;
            Ville = ville;
            CodePostal = codePostal;
        }
    }
}
