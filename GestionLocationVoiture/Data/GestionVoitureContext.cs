using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TP01.Models;

namespace GestionLocationVoiture.Data
{
    public class GestionVoitureContext(DbContextOptions<GestionVoitureContext> options) : DbContext(options)
    {
        public DbSet<Succursale> Succursales { get; set; }
        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Conducteur> Conducteurs { get; set; }

    }
}
