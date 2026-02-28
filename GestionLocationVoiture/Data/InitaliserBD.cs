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

            context.Succursales.Add(new SuccursaleBD("Succursale Nord", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new SuccursaleBD("Succursale Est", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new SuccursaleBD("Succursale Ouest", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new SuccursaleBD("Succursale Sud", StatutSuccursale.ACTIVE));

            context.Voitures.Add(new VoitureBD("bebe", StatutVoiture.ACTIVE, true, EtatVoiture.NEUF, "WBA8E9G57HNU38472", "F32 KLM", "Volkswagen", "Taos", 2025, "blanc", 0, 40000));
            context.Voitures.Add(new VoitureBD("bebe 2", StatutVoiture.ACTIVE, true, EtatVoiture.NEUF, "1HGCM82633A749562", "728 ZRT", "Volkswagen", "Taos", 2025, "noir", 0, 40000));
            context.Voitures.Add(new VoitureBD( "Auto usagée", StatutVoiture.ACTIVE, true, EtatVoiture.USAGE, "2FTRX18W1XCA34567", "482 QLM", "Toyota", "Corolla", 2019, "gris", 68500, 18500));
            context.Voitures.Add(new VoitureBD("Auto archivée", StatutVoiture.ARCHIVE, false, EtatVoiture.USAGE, "3N1AB7AP4KY256891", "913 HTR", "Honda", "Civic", 2018, "blanc", 92000, 15900));


            context.SaveChanges();
        }
    }
}
