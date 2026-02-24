using System;
using System.Collections.Generic;
using System.Text;
using TP01.Models;

namespace GestionLocationVoiture.Data
{
    public static class InitaliserDB
    {
        public static void Seed(GestionVoitureContext context)
        {

            context.Succursales.Add(new Succursale("Succursale Nord", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new Succursale("Succursale Est", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new Succursale("Succursale Ouest", StatutSuccursale.ACTIVE));
            context.Succursales.Add(new Succursale("Succursale Sud", StatutSuccursale.ACTIVE));


            context.SaveChanges();
        }
    }
}
