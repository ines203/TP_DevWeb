using FluentValidation;
using Humanizer;

namespace SystemeDeLocation.Website.Models.Location
{
    public class LocationCreationValidator : AbstractValidator<LocationCreation>
    {
        public LocationCreationValidator()
        {

            RuleFor(r => r.Prenom)
            .Must(p => !string.IsNullOrWhiteSpace(p) && !p.Any(char.IsDigit))
            .WithMessage("S'il vous plaît, entrez un prénom!");

            RuleFor(r => r.Nom)
            .Must(p => !string.IsNullOrWhiteSpace(p) && !p.Any(char.IsDigit))
            .WithMessage("S'il vous plaît, entrez un nom!");

            RuleFor(r => r.NumeroDeTelephone)
            .Cascade(CascadeMode.Stop)
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Le numéro de téléphone est requis.")
            .Matches(@"^\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{4}$")
            .WithMessage("Numéro de téléphone invalide.");

            RuleFor(r => r.AdresseCourriel)
             .Cascade(CascadeMode.Stop)
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Veuillez entrer un courriel.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("Le courriel doit être valide (ex: nom@domaine.com).");

            RuleFor(r => r.NumeroDuPermis)
            .Cascade(CascadeMode.Stop)
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Le numéro de permis est requis.")
            .Custom((permisConduire, context) => PermisDeConduireCanadiensValide(permisConduire, context));

            RuleFor(a => a.NumeroCivique)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Le numéro civique est requis.")
            .GreaterThan(0)
            .WithMessage("Le numéro civique doit être supérieur à 0.");

            RuleFor(a => a.Rue)
                .Cascade(CascadeMode.Stop)
                .Must(a => !string.IsNullOrWhiteSpace(a) && !a.Any(char.IsDigit))
                .WithMessage("La rue est requise.")
                .Matches(@"^[A-Za-zÀ-ÿ0-9\s\.\-']+$")
                .WithMessage("Nom de rue invalide.");

            RuleFor(a => a.Ville)
                .Cascade(CascadeMode.Stop)
                .Must(a => !string.IsNullOrWhiteSpace(a) && !a.Any(char.IsDigit))
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
        private void PermisDeConduireCanadiensValide(string? permisDeConduire, ValidationContext<LocationCreation> context)
        {
            var nom = context.InstanceToValidate.Nom;// va me permettre de vérifier des modèles de données
            //permis de conduire québécois
            if (permisDeConduire.Length == 15)
            {
                if(nom != null)
                {
                    nom = nom.Trim();
                }
                if (nom == null)
                {
                    context.AddFailure("Le Nom est requis pour la validation du permis de conduire. Quebecois.");
                    return;
                }
                if (permisDeConduire[0] != nom[0])
                {
                    context.AddFailure("Le premier caractère du permis de conduire n'égale pas le premier caractère du nom de famille.");
                    return;
                }
                if (permisDeConduire[5] != '-' || permisDeConduire[12] != '-')
                {
                    context.AddFailure("Tiret manquant ou pas à la bonne place!");
                    return;
                }
                for (int i = 1; i < permisDeConduire.Length; i++)
                {
                    if(i != 5 && i != 12)
                    {
                        if (!char.IsDigit(permisDeConduire[i]))
                        {
                            context.AddFailure("Après premier caractère, on devrait avoir que des chiffres et les tirets à la bonne place");
                            return;
                        }
                    }
                }
            }
            // permis ontarien
            else if(permisDeConduire.Length == 17)
            {
                if (nom != null)
                {
                    nom = nom.Trim();
                }
                if (nom == null)
                {
                    context.AddFailure("Le Nom est requis pour la validation du permis de conduire. Ontarien.");
                    return;
                }
                if (permisDeConduire[0] != nom[0])
                {
                    context.AddFailure("Le premier caractère du permis de conduire n'égale pas le premier caractère du nom de famille.");
                    return;
                }
                if (permisDeConduire[5] != '-' || permisDeConduire[11] != '-')
                {
                    context.AddFailure("Tiret manquant ou pas à la bonne place!");
                    return;
                }
                for (int i = 1; i < permisDeConduire.Length; i++)
                {
                    if (i != 5 && i != 11)
                    {
                        if (!char.IsDigit(permisDeConduire[i]))
                        {
                            context.AddFailure("Après premier caractère, on devrait avoir que des chiffres et les tirets à la bonne place");
                            return;
                        }
                    }
                }
            }
            //colombie-britannique, Saskatchewan, Manitoba, Nouveau-Brunswick, Île-du-Prince-Édouard, Terre-Neuve-et-Labrador, Yukon, Nunavut
            else if ((permisDeConduire.Length > 4 && permisDeConduire.Length < 10) || permisDeConduire.Length == 12)
            {
                if (permisDeConduire.Any(char.IsLetter))
                {
                    context.AddFailure("Ne doit contenir que des chiffres!!");
                    return;
                }
            }//Alberta
            else if(permisDeConduire.Length == 10)
            {
                if (permisDeConduire.Any(c => c == '-'))
                {
                    permisDeConduire = permisDeConduire.Replace("-", "");
                }
                else
                {
                    context.AddFailure("Ne contient pas un tiret!!");
                    return;
                }
                if (permisDeConduire.Any(char.IsLetter))
                {
                    context.AddFailure("Ne doit contenir que des chiffres!!");
                    return;
                }
            }
            //Nouvelle-Ecosse
            else if (permisDeConduire.Length == 14)
            {
                if (nom != null)
                {
                    nom = nom.Trim();
                }
                if (nom == null)
                {
                    context.AddFailure("Le Nom est requis pour la validation du permis de conduire. Nouvelle-Ecosse.");
                    return;
                }
                if(nom.Length > 4)
                {
                    for(int i = 0; i < 5; ++i)
                    {
                        if(char.ToUpper(permisDeConduire[i]) != char.ToUpper(nom[i]))
                        {
                            context.AddFailure("Le caractère du debut du permis de conduire ne correspond pas au debut du nom de famille.");
                            return;
                        }
                    }
                    permisDeConduire = permisDeConduire.Substring(5);
                    if (permisDeConduire.Any(char.IsLetter))
                    {
                        context.AddFailure("Le reste doit contenir que des chiffres!!");
                        return;
                    }
                }
                else
                {
                    for (int i = 0; i < nom.Length; ++i)
                    {
                        if (char.ToUpper(permisDeConduire[i]) != char.ToUpper(nom[i]))
                        {
                            context.AddFailure("Le caractère du debut du permis de conduire ne correspond pas au debut du nom de famille.");
                            return;
                        }
                    }
                    permisDeConduire = permisDeConduire.Substring(nom.Length);
                    if (permisDeConduire.Any(char.IsLetter))
                    {
                        context.AddFailure("Le reste doit contenir que des chiffres!!");
                        return;
                    }
                }
            }
            else
            {
                context.AddFailure("Permis de conduire INVALIDE!");
                return;
            }
        }
    }
}
