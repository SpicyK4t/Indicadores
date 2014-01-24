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
    }
}