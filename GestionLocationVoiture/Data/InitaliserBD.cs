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


            context.SaveChanges();
        }
    }
}
