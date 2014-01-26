using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoIndicadores.Models
{
    public class IndicadoresContext : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Sector> sectores { get; set; }
        public DbSet<Indicador> indicadores { get; set; }
        public DbSet<Area> areas { get; set; }
        public DbSet<Aplica> indicadores_areas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().Property(m => m.contrasenia).HasMaxLength(500);
            modelBuilder.Entity<Usuario>().Property(m => m.salt).HasMaxLength(500);


            base.OnModelCreating(modelBuilder);
        }
    }
}