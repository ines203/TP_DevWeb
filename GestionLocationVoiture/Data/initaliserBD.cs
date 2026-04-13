using Microsoft.AspNetCore.Identity;
using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Core.Data
{
    public static class initaliserBD
    {
        public static async Task Seed(GestionVoitureContext context,
            UserManager<UtilisateurBD> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            // Créer les rôles
            string[] roles = { "ADMINISTRATEUR", "GERANT", "COMMIS", "UTILISATEUR" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };

                    var result = await roleManager.CreateAsync(role);

                    if (!result.Succeeded)
                    {
                        var erreurs = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Erreur lors de la création du rôle '{roleName}' : {erreurs}");
                    }
                }
            }

            // Succursales
            var s1 = new SuccursaleBD("Succursale-Nord", StatutSuccursale.ACTIVE);
            var s2 = new SuccursaleBD("Succursale-Est", StatutSuccursale.ACTIVE);
            var s3 = new SuccursaleBD("Succursale-Ouest", StatutSuccursale.DESACTIVE);
            context.Succursales.AddRange(s1, s2, s3);
            context.SaveChanges();

            // Créer les utilisateurs
            await CreerUtilisateur(userManager, "admin", "Admin123!", "ADMINISTRATEUR", null);
            await CreerUtilisateur(userManager, "gerant1", "Gerant123!", "GERANT", s1);
            await CreerUtilisateur(userManager, "commis1", "Commis123!", "COMMIS", s1);
            await CreerUtilisateur(userManager, "gerant2", "Gerant123!", "GERANT", s2);
            await CreerUtilisateur(userManager, "commis2", "Commis123!", "COMMIS", s2);

            // Voitures
            var v1 = new VoitureBD("Focus-01", "1HGBH41JXMN109186", "AB12345",
                "Ford", "Focus", 2022, "Rouge", EtatVoiture.USAGE, s1.Id, 45000, 12000);
            var v2 = new VoitureBD("Civic-01", "2T1BURHE0JC043821", "CD67890",
                "Honda", "Civic", 2023, "Blanc", EtatVoiture.NEUF, s1.Id, 5000, 25000);
            var v3 = new VoitureBD("RAV4-01", "3VWFE21C04M000001", "EF11223",
                "Toyota", "RAV4", 2021, "Noir", EtatVoiture.USAGE, s2.Id, 60000, 18000);
            context.Voitures.AddRange(v1, v2, v3);
            context.SaveChanges();

            // Adresses
            var a1 = new AdresseBD(123, "Rue Principale", "Saint-Hyacinthe", "J2S 1H1");
            var a2 = new AdresseBD(456, "Avenue des Pins", "Montreal", "H1A 1A1");
            context.Adresses.AddRange(a1, a2);
            context.SaveChanges();

            // Conducteurs
            var c1 = new ConducteurBD("Jean", "Tremblay", "jean.tremblay@email.com",
                "514-555-1234", "T1234-010185-01", a1.Id);
            var c2 = new ConducteurBD("Marie", "Dupont", "marie.dupont@email.com",
                "450-555-5678", "D5678-150390-02", a2.Id);
            context.Conducteurs.AddRange(c1, c2);
            context.SaveChanges();

            // Locations
            var l1 = new LocationBD(v1.Id, c1.Id, DateTime.Now.AddDays(3));
            v1.Disponible = false;

            var l2 = new LocationBD(v3.Id, c2.Id, DateTime.Now.AddDays(-1));
            l2.Ouverture = DateTime.Now.AddDays(-3);
            l2.Statut = StatutLocation.FERME;
            l2.FermetureReelle = DateTime.Now.AddDays(-1);

            context.Locations.AddRange(l1, l2);
            context.SaveChanges();
        }

        private static async Task CreerUtilisateur(UserManager<UtilisateurBD> userManager,
            string userName, string password, string role, SuccursaleBD? succursale)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = UtilisateurBD.Create(userName, succursale);
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}