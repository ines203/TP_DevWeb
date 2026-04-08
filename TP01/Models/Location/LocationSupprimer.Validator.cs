using FluentValidation;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationSupprimerValidator : AbstractValidator<LocationSupprimer>
    {
        public LocationSupprimerValidator()
        {
            RuleFor(r => r.FermetureReelle)
             .GreaterThanOrEqualTo(DateTime.Now)
             .WithMessage("La date doit être aujourd'hui ou après.")
             .LessThan(r => r.FermeturePlanifiee)
             .WithMessage("La date doit être avant la fermeture planifié.");
        }
        
    }
}
