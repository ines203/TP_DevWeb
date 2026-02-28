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
            // Utilisateurs
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
            var v1 = new VoitureBD("Focus-01", "1HGBH41JXMN109186", "AB12345",
                "Ford", "Focus", 2022, "Rouge", EtatVoiture.USAGE, s1.Id, 45000, 12000);
            var v2 = new VoitureBD("Civic-01", "2T1BURHE0JC043821", "CD67890",
                "Honda", "Civic", 2023, "Blanc", EtatVoiture.NEUF, s1.Id, 5000, 25000);
            var v3 = new VoitureBD("RAV4-01", "3VWFE21C04M000001", "EF11223",
                "Toyota", "RAV4", 2021, "Noir", EtatVoiture.USAGE, s2.Id, 60000, 18000);
            context.Voitures.AddRange(v1, v2, v3);
            context.SaveChanges();

            // Adresses
            var a1 = new AdresseBD("123", "Rue Principale", "Saint-Hyacinthe", "J2S 1H1");
            var a2 = new AdresseBD("456", "Avenue des Pins", "Montreal", "H1A 1A1");
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
            v1.Disponible = false; // Voiture réservée

            
            var l2 = new LocationBD(v3.Id, c2.Id, DateTime.Now.AddDays(-1));
            l2.Ouverture = DateTime.Now.AddDays(-3);
            l2.Statut = StatutLocation.FERME;
            l2.FermetureReelle = DateTime.Now.AddDays(-1);

            context.Locations.AddRange(l1, l2);
            context.SaveChanges();
        }
    }
}
