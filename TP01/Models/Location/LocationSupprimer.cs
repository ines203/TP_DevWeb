using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationSupprimer
    {
        public int LocationId { get; set; }
        public int SuccursaleId { get; set; }
        public DateTime FermetureReelle { get; set; } = DateTime.Now;
        public DateTime FermeturePlanifiee { get; set; }
        public string? Note { get; set; }
    }
}
