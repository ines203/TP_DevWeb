namespace SystemeDeLocation.Core.Domain
{
    public class AdresseBD
    {
        public int Id { get; set; }
        public string NumeroCivic { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        public virtual ConducteurBD Conducteur { get; set; }
        public virtual SuccursaleBD Succursale { get; set; }

        protected AdresseBD() { }

        public AdresseBD(string numeroCivic, string rue, string ville, string codePostal)
        {
            NumeroCivic = numeroCivic;
            Rue = rue;
            Ville = ville;
            CodePostal = codePostal;
        }
    }
}
