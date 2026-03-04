using System;
using System.Collections.Generic;
using System.Text;
using TP01.Models;

namespace GestionLocationVoiture.Data
{
    public static class InitaliserBD
    {
        public static void Seed(GestionVoitureContext context)
        {
            //(string nom, string prenom, string email, string motDePasse, Role role)
            context.Utilisateurs.Add(new UtilisateurBD("administrateur", "John", "admin@hotmail.com", "admin", Role.ADMINISTRATEUR));
            context.Utilisateurs.Add(new UtilisateurBD("client", "John", "client@hotmail.com", "client", Role.CLIENT));
            context.Utilisateurs.Add(new UtilisateurBD("gerant", "John", "gerant@hotmail.com", "gerant", Role.GERANT));
            context.Utilisateurs.Add(new UtilisateurBD("commis", "John", "commis@hotmail.com", "commis", Role.COMMIS));


            var Succursale1 = new SuccursaleBD("Succursale Centrale", StatutSuccursale.ACTIVE);
            var Succursale2 = new SuccursaleBD("Succursale Nord", StatutSuccursale.ACTIVE);
            var Succursale3 = new SuccursaleBD("Succursale Est", StatutSuccursale.ACTIVE);
            var Succursale4 = new SuccursaleBD("Succursale Ouest", StatutSuccursale.ACTIVE);

            var voiture1 = new VoitureBD("bebe", StatutVoiture.ACTIVE, true, EtatVoiture.NEUF, "WBA8E9G57HNU38472", "F32 KLM", "Volkswagen", "Taos", 2025, "blanc", 0, 40000);
            var voiture2 = new VoitureBD("bebe 2", StatutVoiture.ACTIVE, true, EtatVoiture.NEUF, "1HGCM82633A749562", "728 ZRT", "Volkswagen", "Taos", 2025, "noir", 0, 40000); 
            var voiture3 = new VoitureBD( "Auto usagée", StatutVoiture.ACTIVE, true, EtatVoiture.USAGE, "2FTRX18W1XCA34567", "482 QLM", "Toyota", "Corolla", 2019, "gris", 68500, 18500);
            var voiture4 = new VoitureBD("Auto archivée", StatutVoiture.ARCHIVE, false, EtatVoiture.USAGE, "3N1AB7AP4KY256891", "913 HTR", "Honda", "Civic", 2018, "blanc", 92000, 15900);

            voiture1.Notes = new List<NoteBD>
            {
                new NoteBD("Excellent état, très confortable."),
                new NoteBD("Bonne voiture, mais un peu bruyante.")
            };
            voiture2.Notes = new List<NoteBD>
            {
                new NoteBD("Très économique en carburant."),
                new NoteBD("Intérieur spacieux et moderne.")
            };
            voiture3.Notes = new List<NoteBD>
            {
                new NoteBD("Voiture fiable, mais l'intérieur montre des signes d'usure."),
                new NoteBD("Bon rapport qualité-prix pour une voiture d'occasion.")
            };

            voiture4.Notes = new List<NoteBD>
            {
                new NoteBD("Voiture en bon état, mais un peu ancienne."),
                new NoteBD("Idéale pour les trajets urbains, mais pas pour les longs voyages.")
            };

            voiture1.Locations = new List<LocationBD>
            {
                new LocationBD(StatutLocation.FERME, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-2)),
                new LocationBD(StatutLocation.OUVERT, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2), DateTime.Now.AddDays(5))
            };
            voiture2.Locations = new List<LocationBD>
            {
                new LocationBD(StatutLocation.FERME, DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-15), DateTime.Now.AddDays(-12)),
                new LocationBD(StatutLocation.OUVERT, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(0), DateTime.Now.AddDays(3))
            };
            voiture3.Locations = new List<LocationBD>
            {
                new LocationBD(StatutLocation.FERME, DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-25), DateTime.Now.AddDays(-22)),
                new LocationBD(StatutLocation.OUVERT, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-2))
            };
            voiture4.Locations = new List<LocationBD>
            {
                new LocationBD(StatutLocation.FERME, DateTime.Now.AddDays(-40), DateTime.Now.AddDays(-35), DateTime.Now.AddDays(-32)),
                new LocationBD(StatutLocation.OUVERT, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-7))
            };

            Succursale1.Voitures.Add(voiture1);
            Succursale2.Voitures.Add(voiture2);
            Succursale3.Voitures.Add(voiture3);
            Succursale4.Voitures.Add(voiture4);

            context.Succursales.AddRange(Succursale1, Succursale2, Succursale3, Succursale4);

            context.SaveChanges();
        }
    }
}
