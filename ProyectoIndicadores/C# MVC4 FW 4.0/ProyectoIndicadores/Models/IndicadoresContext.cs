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

        public void Seed(IndicadoresContext context)
        {
            #region Anexo de Administrador
            var cypher = new SimpleCrypto.PBKDF2();
            Usuario usuario = usuarios.Create();
            usuario.pk = Guid.NewGuid();
            usuario.correo = "admin@udec.edu.mx";
            usuario.nombre = "Administrador";
            usuario.apellido = "Administrador";
            usuario.nombre_usuario = "admin";
            usuario.is_admin = true;
            usuario.contrasenia = cypher.Compute("admin123456789");
            usuario.salt = cypher.Salt;
            context.usuarios.Add(usuario);
            context.SaveChanges();
            #endregion

            #region Anexo de Indicadores
            context.indicadores.Add(new Indicador() { pk = 1, nombre = "7. Tasa de eficiencia terminal por generación y área (%)", menor_meta = false, institucional=0.0f, meta=80.0f });
            context.indicadores.Add(new Indicador() { pk = 2, nombre = "18. % de Titulados a 1 año de haber egresado", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 3, nombre = "23. Promedio de egresados con trabajo a 1 año de haber egresado (%)", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 4, nombre = "27. % Egresados de licenciatura que emprenden su negocio a un año de haber egresado", menor_meta = false, institucional = 0.0f, meta = 30.0f });
            context.indicadores.Add(new Indicador() { pk = 5, nombre = "30. Evaluación de empleadores a los egresados de licenciatura", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 6, nombre = "2. Tasa de deserción (%)", menor_meta = true, institucional = 0.0f, meta = 5.0f });
            context.indicadores.Add(new Indicador() { pk = 7, nombre = "35. Efectividad del programa de tutorías (%)", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 8, nombre = "73i.% alumnos con TOEIC aprobado al concluir sus estudios", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 9, nombre = "81. Autoevaluación competencias (importancia)", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 10, nombre = "82. Autoevaluación competencias (desempeño)", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 11, nombre = "80. Nivel académico del área, percibido por los alumnos.", menor_meta = true, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 12, nombre = "3. Promedio del Sistema de evaluación docente (SED)", menor_meta = false, institucional = 0.0f, meta = 85.0f });
            context.indicadores.Add(new Indicador() { pk = 13, nombre = "8. Promedio de evaluación . MTX (Docente y Servicios)", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 14, nombre = "4. Efectividad de la mejora del desempeño docente", menor_meta = false, institucional = 0.0f, meta = 0.0f });
            context.indicadores.Add(new Indicador() { pk = 15, nombre = "(10-11).Porcentaje de HORAS -CURSO impartidas con docentes por grado superior al nivel donde enseña ", menor_meta = false, institucional = 0.0f, meta = 60.0f });
            context.indicadores.Add(new Indicador() { pk = 16, nombre = "(13) Porcentaje de profesores con experiencia docente universitaria de 5 años o más", menor_meta = true, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 17, nombre = "(84) Porcentaje de profesores con experiencia laboral de 5 años o más", menor_meta = false, institucional = 0.0f, meta = 60.0f });
            context.indicadores.Add(new Indicador() { pk = 18, nombre = "(19) Porcentaje de materias impartidas por profesores de tiempo completo", menor_meta = false, institucional = 0.0f, meta = 40.0f });
            context.indicadores.Add(new Indicador() { pk = 19, nombre = "6. RVOES actualizados de acuerdo con el procedimiento y política institucional", menor_meta = false, institucional = 0.0f, meta = 1.0f });
            context.indicadores.Add(new Indicador() { pk = 20, nombre = "74.% Alumnos con nivel satisfactorio CENEVAL) o ENLACE", menor_meta = false, institucional = 0.0f, meta = 60.0f });
            context.indicadores.Add(new Indicador() { pk = 21, nombre = "75. Programas acreditados o certificados por instancias externas", menor_meta = true, institucional = 0.0f, meta = 1.0f });
            context.indicadores.Add(new Indicador() { pk = 22, nombre = "i22. Porcentaje de participación por área en la Encuesta", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 23, nombre = "57.Utilidad neta institucional y por programas", menor_meta = false, institucional = 0.0f, meta = 0.0f });
            context.indicadores.Add(new Indicador() { pk = 24, nombre = "53. % de proyectos alcanzados", menor_meta = false, institucional = 0.0f, meta = 80.0f });
            context.indicadores.Add(new Indicador() { pk = 25, nombre = "50. Evaluación de pares de los clientes internos", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 26, nombre = "(60) porcentaje de cumplimiento en la entrega de Indicadores de Gestión", menor_meta = true, institucional = 0.0f, meta = 95.0f });
            context.indicadores.Add(new Indicador() { pk = 27, nombre = "(69) Porcentaje de 'promotores netos'", menor_meta = false, institucional = 0.0f, meta = 75.0f });
            context.indicadores.Add(new Indicador() { pk = 28, nombre = "50. % de Becas", menor_meta = true, institucional = 0.0f, meta = 17.0f });
            context.indicadores.Add(new Indicador() { pk = 29, nombre = "33. % de alumnos en programas de internacionalización por áreas", menor_meta = false, institucional = 0.0f, meta = 10.0f });
            context.indicadores.Add(new Indicador() { pk = 30, nombre = "88. Evaluación promedio en la satisfacción de los eventos de extensión universitaria", menor_meta = false, institucional = 0.0f, meta = 9.0f });
            context.indicadores.Add(new Indicador() { pk = 31, nombre = "14. No. de publicaciones en la institución (al menos 1 por área)", menor_meta = false, institucional = 0.0f, meta = 1.0f });
            #endregion

            #region Anexo de Sectores
            context.sectores.Add(new Sector() { pk = 1, nombre = "Uis" });
            context.sectores.Add(new Sector() { pk = 2, nombre = "Profesional" });
            context.sectores.Add(new Sector() { pk = 3, nombre = "P. Ejecutivos" });
            context.sectores.Add(new Sector() { pk = 4, nombre = "Maestrias" });
            context.sectores.Add(new Sector() { pk = 5, nombre = "Universidad Virtual" });
            context.sectores.Add(new Sector() { pk = 6, nombre = "Doctorados" });     
            context.sectores.Add(new Sector() { pk = 7, nombre = "UNI Servicios Licenciaturas"});
            #endregion

            #region Anexo de Areas
            context.areas.Add(new Area() { pk = 1, nombre = "kinder", sector_id = 1 });
            context.areas.Add(new Area() { pk = 2, nombre = "Primaria", sector_id = 1 });
            context.areas.Add(new Area() { pk = 3, nombre = "Secundaria", sector_id = 1 });
            context.areas.Add(new Area() { pk = 4, nombre = "Preparatoria", sector_id = 1 });
            context.areas.Add(new Area() { pk = 5, nombre = "IDIOC", sector_id = 1 });

            context.areas.Add(new Area() { pk = 6, nombre = "ARQ", sector_id = 2 });
            context.areas.Add(new Area() { pk = 7, nombre = "DER", sector_id = 2 });
            context.areas.Add(new Area() { pk = 8, nombre = "LCC", sector_id = 2 });
            context.areas.Add(new Area() { pk = 9, nombre = "LDD", sector_id = 2 });
            context.areas.Add(new Area() { pk = 10, nombre = "MKT", sector_id = 2 });
            context.areas.Add(new Area() { pk = 11, nombre = "LAE", sector_id = 2 });
            context.areas.Add(new Area() { pk = 12, nombre = "LCI", sector_id = 2 });
            context.areas.Add(new Area() { pk = 13, nombre = "FLI", sector_id = 2 });
            context.areas.Add(new Area() { pk = 14, nombre = "IIA", sector_id = 2 });
            context.areas.Add(new Area() { pk = 15, nombre = "LID", sector_id = 2 });
            context.areas.Add(new Area() { pk = 16, nombre = "MEDIC", sector_id = 2 });
            context.areas.Add(new Area() { pk = 17, nombre = "PSIC", sector_id = 2 });
            context.areas.Add(new Area() { pk = 18, nombre = "LT", sector_id = 2 });
            context.areas.Add(new Area() { pk = 19, nombre = "LN", sector_id = 2 });
            context.areas.Add(new Area() { pk = 20, nombre = "LG", sector_id = 2 });
            context.areas.Add(new Area() { pk = 21, nombre = "LIB", sector_id = 2 });
            
            context.areas.Add(new Area() { pk = 22, nombre = "FNI", sector_id = 3 });
            context.areas.Add(new Area() { pk = 23, nombre = "IIAM", sector_id = 3 });
            context.areas.Add(new Area() { pk = 24, nombre = "FPH", sector_id = 3});

            context.areas.Add(new Area() { pk = 25, nombre = "MDH", sector_id = 4 });
            context.areas.Add(new Area() { pk = 26, nombre = "FML", sector_id = 4 });
            context.areas.Add(new Area() { pk = 27, nombre = "MAE", sector_id = 4 });
            context.areas.Add(new Area() { pk = 28, nombre = "IASE", sector_id = 4 });
            context.areas.Add(new Area() { pk = 29, nombre = "MIC", sector_id = 4 });


            context.areas.Add(new Area() { pk = 30, nombre = "W-MCL", sector_id = 5 });
            context.areas.Add(new Area() { pk = 31, nombre = "W-MTI", sector_id = 5 });
            context.areas.Add(new Area() { pk = 32, nombre = "W-MEM", sector_id = 5 });

            context.areas.Add(new Area() { pk = 33, nombre = "DA", sector_id = 6 });
            context.areas.Add(new Area() { pk = 34, nombre = "DDH", sector_id = 6 });

            #endregion

            #region Anexo Usuarios
            var cipher = new SimpleCrypto.PBKDF2();

            #region instancias usuario

            Usuario alma = new Usuario() { pk = Guid.NewGuid(), nombre = "Alma", correo = "alma@udec.edu.mx", apellido = "nose", nombre_usuario = "alma", contrasenia = cypher.Compute("alma2014"), salt = cypher.Salt, is_admin = false };
            Usuario viridiana = new Usuario() { pk = Guid.NewGuid(), nombre = "Viridiana", correo = "viridiana@udec.edu.mx", apellido = "nose", nombre_usuario = "viridiana", contrasenia = cypher.Compute("viridiana2014"), salt = cypher.Salt, is_admin = false };
            Usuario tonoz = new Usuario() { pk = Guid.NewGuid(), nombre = "Tonoz", correo = "tonoz@udec.edu.mx", apellido = "Zacarias", nombre_usuario = "tonoz", contrasenia = cypher.Compute("tonoz2014"), salt = cypher.Salt, is_admin = false };
            Usuario armando = new Usuario() { pk = Guid.NewGuid(), nombre = "Armando", correo = "armando@udec.edu.mx", apellido = "nose", nombre_usuario = "armando", contrasenia = cypher.Compute("armando2014"), salt = cypher.Salt, is_admin = false };
            Usuario moises = new Usuario() { pk = Guid.NewGuid(), nombre = "Moises", correo = "moises@udec.edu.mx", apellido = "nose", nombre_usuario = "moises", contrasenia = cypher.Compute("moises2014"), salt = cypher.Salt, is_admin = false };
            Usuario lucero = new Usuario() { pk = Guid.NewGuid(), nombre = "Lucero", correo = "lucero@udec.edu.mx", apellido = "nose", nombre_usuario = "lucero", contrasenia = cypher.Compute("lucero2014"), salt = cypher.Salt, is_admin = false };
            Usuario ernesto = new Usuario() { pk = Guid.NewGuid(), nombre = "Ernesto", correo = "ernesto@udec.edu.mx", apellido = "Nieto", nombre_usuario = "ernesto", contrasenia = cypher.Compute("ernesto2014"), salt = cypher.Salt, is_admin = true };
            Usuario luisa = new Usuario() { pk = Guid.NewGuid(), nombre = "Luisa", correo = "luisa@udec.edu.mx", apellido = "nose", nombre_usuario = "luisa", contrasenia = cypher.Compute("luisa2014"), salt = cypher.Salt, is_admin = false };
            Usuario sandra = new Usuario() { pk = Guid.NewGuid(), nombre = "Sandra", correo = "sandra@udec.edu.mx", apellido = "nose", nombre_usuario = "sandra", contrasenia = cypher.Compute("sandra2014"), salt = cypher.Salt, is_admin = false };
            Usuario eduardo = new Usuario() { pk = Guid.NewGuid(), nombre = "Eduardo", correo = "eduardo@udec.edu.mx", apellido = "nose", nombre_usuario = "eduardo", contrasenia = cypher.Compute("eduardo2014"), salt = cypher.Salt, is_admin = false };
            Usuario ana = new Usuario() { pk = Guid.NewGuid(), nombre = "Ana", correo = "ana@udec.edu.mx", apellido = "nose", nombre_usuario = "ana", contrasenia = cypher.Compute("ana2014"), salt = cypher.Salt, is_admin = false };  

            #endregion

            #region rol usuario proveedor
            alma.provee.Add(indicadores.Find(1));
            alma.provee.Add(indicadores.Find(2));
            alma.provee.Add(indicadores.Find(6));
            alma.provee.Add(indicadores.Find(19));

            ana.provee.Add(indicadores.Find(31));

            armando.provee.Add(indicadores.Find(8));

            eduardo.provee.Add(indicadores.Find(30));

            ernesto.provee.Add(indicadores.Find(13));
            ernesto.provee.Add(indicadores.Find(14));
            ernesto.provee.Add(indicadores.Find(20));
            ernesto.provee.Add(indicadores.Find(21));
            ernesto.provee.Add(indicadores.Find(22));
            ernesto.provee.Add(indicadores.Find(25));
            ernesto.provee.Add(indicadores.Find(26));
            ernesto.provee.Add(indicadores.Find(27));

            lucero.provee.Add(indicadores.Find(12));

            luisa.provee.Add(indicadores.Find(23));
            luisa.provee.Add(indicadores.Find(28));

            moises.provee.Add(indicadores.Find(9));
            moises.provee.Add(indicadores.Find(10));
            moises.provee.Add(indicadores.Find(11));
            moises.provee.Add(indicadores.Find(15));
            moises.provee.Add(indicadores.Find(16));
            moises.provee.Add(indicadores.Find(17));
            moises.provee.Add(indicadores.Find(18));
            moises.provee.Add(indicadores.Find(24));

            sandra.provee.Add(indicadores.Find(29));

            tonoz.provee.Add(indicadores.Find(7));
            
            viridiana.provee.Add(indicadores.Find(3));
            viridiana.provee.Add(indicadores.Find(4));
            viridiana.provee.Add(indicadores.Find(5));

            #endregion

            #region añadir a la base de datos
            context.usuarios.Add(alma);            
            context.usuarios.Add(viridiana);
            context.usuarios.Add(tonoz);
            context.usuarios.Add(armando);
            context.usuarios.Add(moises);
            context.usuarios.Add(lucero);
            context.usuarios.Add(ernesto);
            context.usuarios.Add(luisa);
            context.usuarios.Add(sandra);
            context.usuarios.Add(eduardo);            
            context.usuarios.Add(ana);
            #endregion

            #endregion

            #region indicadores usuarios
            
            #endregion

        }

        public class CreateInitializer : CreateDatabaseIfNotExists<IndicadoresContext>
        {
            protected override void Seed(IndicadoresContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        static IndicadoresContext()
        {
            Database.SetInitializer<IndicadoresContext>(new CreateInitializer());
        }
    }
}