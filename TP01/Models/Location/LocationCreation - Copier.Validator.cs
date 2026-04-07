using FluentValidation;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationSupprimerValidator : AbstractValidator<LocationSupprimer>
    {
        public LocationSupprimerValidator()
        {
            RuleFor(r => r.FermetureReelle)
             .GreaterThan(DateTime.Today)
             .WithMessage("La date doit être après aujourd'hui.");
        }
        
    }
}
