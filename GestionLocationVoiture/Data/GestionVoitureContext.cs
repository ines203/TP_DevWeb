using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SystemeDeLocation.Core.Domain;

namespace SystemeDeLocation.Core.Data;

public class GestionVoitureContext(DbContextOptions<GestionVoitureContext> options) :
    IdentityDbContext<UtilisateurBD, IdentityRole<Guid>, Guid>(options)  
{
    public DbSet<SuccursaleBD> ?Succursales { get; set; }
    public DbSet<VoitureBD> ?Voitures { get; set; }
    public DbSet<LocationBD> ?Locations { get; set; }
    public DbSet<ConducteurBD> ?Conducteurs { get; set; }
    public DbSet<AdresseBD> ?Adresses { get; set; }
}