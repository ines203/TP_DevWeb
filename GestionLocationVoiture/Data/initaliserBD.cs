using System;
using System.Collections.Generic;
using System.Text;
using GestionLocationVoiture.Data; 
using TP01.Models;

namespace GestionLocationVoiture.Data
{
    public static class initaliserBD
    {
        public static void Seed(GestionVoitureContext context)
        {
            //Utulisateurs
            context.Utilisateurs.Add(new UtilisateurBD("admin", "admin", Role.ADMINISTRATEUR));
            context.Utilisateurs.Add(new UtilisateurBD("gerant1", "gerant", Role.GERANT));
            context.Utilisateurs.Add(new UtilisateurBD("commis1", "commis", Role.COMMIS));
            context.Utilisateurs.Add(new UtilisateurBD("user1", "user", Role.UTILISATEUR));

            // Succursales
            var s1 = new SuccursaleBD("Succursale-Nord", StatutSuccursale.ACTIVE);
            var s2 = new SuccursaleBD("Succursale-Est", StatutSuccursale.ACTIVE);
            var s3 = new SuccursaleBD("Succursale-Ouest", StatutSuccursale.DESACTIVE);
            context.Succursales.AddRange(s1, s2, s3);
            context.SaveChanges();

            // Voitures
            context.Voitures.Add(new VoitureBD("Focus-01", "1HGBH41JXMN109186", "AB12345",
                "Ford", "Focus", 2022, "Rouge", EtatVoiture.USAGE, s1.Id, 45000, 12000));
            context.Voitures.Add(new VoitureBD("Civic-01", "2T1BURHE0JC043821", "CD67890",
                "Honda", "Civic", 2023, "Blanc", EtatVoiture.NEUF, s1.Id, 5000, 25000));
            context.Voitures.Add(new VoitureBD("RAV4-01", "3VWFE21C04M000001", "EF11223",
                "Toyota", "RAV4", 2021, "Noir", EtatVoiture.USAGE, s2.Id, 60000, 18000));

            context.SaveChanges();
            
        }
    }
}
