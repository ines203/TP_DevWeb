using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TP01.Models;

namespace GestionLocationVoiture.Data
{
    public class GestionVoitureContext(DbContextOptions<GestionVoitureContext> options) : DbContext(options)
    {
        public DbSet<SuccursaleBD> Succursales { get; set; }
        //public DbSet<VoitureBD> Voitures { get; set; }
        public DbSet<UtilisateurBD> Utilisateurs { get; set; }

        public DbSet<LocationBD> Locations { get; set; }
        public DbSet<ConducteurBD> Conducteurs { get; set; }

    }
}
