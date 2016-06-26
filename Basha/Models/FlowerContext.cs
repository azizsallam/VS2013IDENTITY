using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Basha.Models
{
    public class FlowerContext :DbContext 
    {
        public FlowerContext()
            : base("FlowerContext")
        {
        }
        public System.Data.Entity.DbSet<Flower > Flowers { get; set; }
         protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}

