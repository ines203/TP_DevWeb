using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationSupprimer
    {
        public int LocationId { get; set; }
        public DateTime FermetureReelle { get; set; }
        public string? Note { get; set; }
    }
}
