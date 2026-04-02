using FluentValidation;

namespace SystemeDeLocation.Website.Models.Location
{
    public class CreationValidator : AbstractValidator<Creation>
    {
        public CreationValidator()
        {
            RuleFor(r => r.Prenom)
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .WithMessage("S'il vous plaît, entrez un prénom!");

            RuleFor(r => r.Nom)
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .WithMessage("S'il vous plaît, entrez un nom!");

            RuleFor(r => r.NumeroDeTelephone)
            .Cascade(CascadeMode.Stop)
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Le numéro de téléphone est requis.")
            .Matches(@"^\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{4}$")
            .WithMessage("Numéro de téléphone invalide.");

            RuleFor(r => r.AdresseCourriel)
            .NotNull().NotEmpty()
            .WithMessage("Please enter an email address.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

            RuleFor(r => r.NumeroDuPermis)
            .Cascade(CascadeMode.Stop)
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Le numéro de permis est requis.")
            .Length(8, 12)
            .Matches(@"^[A-Za-z0-9]+$")
            .WithMessage("Numéro de permis invalide.");

            RuleFor(a => a.NumeroCivique)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Le numéro civique est requis.")
            .GreaterThan(0)
            .WithMessage("Le numéro civique doit être supérieur à 0.");

            RuleFor(a => a.Rue)
                .Cascade(CascadeMode.Stop)
                .Must(a => !string.IsNullOrWhiteSpace(a))
                .WithMessage("La rue est requise.")
                .Matches(@"^[A-Za-zÀ-ÿ0-9\s\.\-']+$")
                .WithMessage("Nom de rue invalide.");

            RuleFor(a => a.Ville)
                .Cascade(CascadeMode.Stop)
                .Must(a => !string.IsNullOrWhiteSpace(a))
                .WithMessage("La ville est requise.")
                .Matches(@"^[A-Za-zÀ-ÿ\s\-']+$")
                .WithMessage("Nom de ville invalide.");

            RuleFor(a => a.CodePostal)
                .Cascade(CascadeMode.Stop)
                .Must(a => !string.IsNullOrWhiteSpace(a))
                .WithMessage("La ville est requise.")
                .Matches(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$")
                .WithMessage("Code postal invalide (format canadien attendu, ex: H1A 1A1).");

            RuleFor(r => r.FermeturePlanifiee)
             .GreaterThan(DateTime.Today)
             .WithMessage("La date doit être après aujourd'hui.");

            RuleFor(r => r.ValiderIdentiteConducteur)
            .Equal(true)
            .WithMessage("Tu devrais valider l'identité du conduceur!");

            RuleFor(r => r.PreuveAssurance)
            .Equal(true)
            .WithMessage("Manque preuve d'assurance!");
        }
    }
}
