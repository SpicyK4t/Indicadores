using ProyectoIndicadores.Forms;
using ProyectoIndicadores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using System.Data.Entity;

namespace ProyectoIndicadores.Controllers
{
    public class AdminController : Controller
    {
        //private IndicadoresContext bd = new IndicadoresContext();
        public ActionResult Index()
        {
            var usuario = new IndicadoresContext().usuarios.FirstOrDefault(m => m.nombre_usuario == User.Identity.Name);
            if (usuario.is_admin)
                return View();
            else
                return RedirectToAction("SinPermisos", "Home");
        }

        #region Manejo de Usuarios

        #region Crear Usuario
        [HttpGet]
        public ActionResult NuevoUsuario()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoUsuario(FormularioRegistroUsuario nuevo_usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.pk = Guid.NewGuid();
                usuario.correo = nuevo_usuario.correo;
                usuario.nombre = nuevo_usuario.nombre;
                usuario.apellido = nuevo_usuario.apellido;
                usuario.nombre_usuario = nuevo_usuario.nombre_usuario;
                usuario.is_admin = nuevo_usuario.is_admin;

                var cypher = new SimpleCrypto.PBKDF2();
                usuario.contrasenia = cypher.Compute(nuevo_usuario.contrasenia);
                usuario.salt = cypher.Salt;
                using (var db = new IndicadoresContext())
                {
                    db.usuarios.Add(usuario);
                    db.SaveChanges();                    
                }

                return RedirectToAction("ListaUsuarios", "Admin");
                
            }
            return View(nuevo_usuario);
        }
        #endregion

        #region Lista de Usuarios
        public ActionResult ListaUsuarios()
        {
            return View(new IndicadoresContext().usuarios.ToList());
        }        
        #endregion

        #region Proveedor de Indicadores
        public ActionResult Proveedor(Guid id)
        {
            var db = new IndicadoresContext();
            Usuario usuario = db.usuarios.Find(id);
            IList<Indicador> indicadores = db.indicadores.ToList();
            IList<Area> areas = db.areas.ToList();

            var modelo = new VistaProveedoresConsumidores();
            modelo.id_usuario = usuario.pk;
            modelo.nombre_usuario = usuario.nombre_usuario;
           
            modelo.provee_pk = usuario.provee.Select(x => x.pk);
            modelo.provee = indicadores
                .Select(x => new SelectListItem
                {
                    Value = x.pk.ToString(), 
                    Text = x.nombre,
                })
                .ToList();

            modelo.consumidor__pk = usuario.consume.Select(x => x.pk);
            modelo.consumidor = areas
                .Select(x => new SelectListItem
                {
                    Value = x.pk.ToString(),
                    Text = x.nombre,
                })
                .ToList();

            db.Dispose();

            return View(modelo);
        }

        [HttpPost]       
        public ActionResult Proveedor(VistaProveedoresConsumidores model)
        {
            using (var db = new IndicadoresContext())
            {
                Usuario usuario = db.usuarios.Find(model.id_usuario);
                if (model.provee_pk == null) { foreach (var indicador in usuario.provee.ToList()) usuario.provee.Remove(indicador); }
                else foreach (var indicador_pk in model.provee_pk) { usuario.provee.Add(db.indicadores.Find(indicador_pk)); }
                if (model.consumidor__pk == null) { foreach (var area in usuario.consume.ToList()) usuario.consume.Remove(area); }
                else foreach ( var area_pk in model.consumidor__pk ) { usuario.consume.Add(db.areas.Find(area_pk)); }

                db.SaveChanges();
            }
            return RedirectToAction("ListaUsuarios", "Admin");
        }       

        #endregion

        #region Validacion de Usuario
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult ChecarDisponibilidadUsuario(string nombre_usuario)
        {
            var existe = false;
            using (var db = new IndicadoresContext())
            {
                existe = db.usuarios.Count(u => u.nombre_usuario == nombre_usuario) == 0;
            }
            return Json(existe, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Manejo de Sectores

        #region Agregar Sectores
        [HttpGet]
        public ActionResult NuevoSector()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoSector(Sector nuevo_sector)
        {
            if(ModelState.IsValid)
            {
                using(var db = new IndicadoresContext())
                {
                    db.sectores.Add(nuevo_sector);
                    db.SaveChanges();

                    return RedirectToAction("ListaSectores", "Admin");
                }
            }
            return View(nuevo_sector);
        }
        #endregion

        #region Lista de Sectores
        public ActionResult ListaSectores()
        {         
            return View(new IndicadoresContext().sectores.ToList());
        }
        #endregion

        #region Detalles del Sector

        public ActionResult DetallesSector(int id = 0) 
        {
            Sector sector = new IndicadoresContext().sectores.Find(id);
            if (sector == null)
                return HttpNotFound();
            return View(sector);
        }
        #endregion

        #region Editar Sector
        [HttpGet]
        public ActionResult EditarSector(int id = 0)
        {
            Sector sector = new IndicadoresContext().sectores.Find(id);
            if (sector == null)
                return HttpNotFound();
            return View(sector);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarSector(Sector edicion_sector)
        {
            if (ModelState.IsValid)
            {
                using (var db = new IndicadoresContext())
                {
                    db.Entry(edicion_sector).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ListaSectores", "Admin");
                }
            }
            return View(edicion_sector);
        }
        #endregion

        #region Borrar Sector
        public ActionResult BorrarSector(int id = 0)
        {
            Sector sector = new IndicadoresContext().sectores.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        [HttpPost, ActionName("BorrarSector")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarBorradoSector(int id)
        {
            using (var db = new IndicadoresContext())
            {
                Sector sector = db.sectores.Find(id);
                db.sectores.Remove(sector);
                db.SaveChanges();
                return RedirectToAction("ListaSectores", "Admin");
            }            
        }

        #endregion

        #endregion

        #region Manejo de Areas

        #region Lista de Areas
        public ActionResult ListaAreas()
        {
            return View(new IndicadoresContext().areas.ToList());
        }
        #endregion

        #region Agregar Areas
        public ActionResult NuevaArea()
        {
            ViewBag.sector_id = new SelectList(new IndicadoresContext().sectores, "pk", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaArea(Area nueva_area)
        {
            if (ModelState.IsValid)
            {
                using (var db = new IndicadoresContext()) 
                {
                    db.areas.Add(nueva_area);
                    db.SaveChanges();

                    return RedirectToAction("ListaAreas", "Admin");
                }
            }
            ViewBag.sector_id = new SelectList(new IndicadoresContext().sectores, "pk", "nombre", nueva_area.sector_id);
            return View(nueva_area);
        }
        #endregion

        #region Editar Areas

        public ActionResult EditarArea(int id)
        {
            Area area = new IndicadoresContext().areas.Find(id);
            if (area == null)
                return HttpNotFound();
            ViewBag.sector_id = new SelectList(new IndicadoresContext().sectores, "pk", "nombre", area.sector_id);
            return View(area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArea(Area area_editada)
        {
            if (ModelState.IsValid)
            {
                using (var db = new IndicadoresContext())
                {
                    db.Entry(area_editada).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ListaAreas", "Admin");
                }
            }
            ViewBag.sector_id = new SelectList(new IndicadoresContext().sectores, "pk", "nombre", area_editada.sector_id);
            return View(area_editada);
        }

        #endregion

        #region Detalles del Area

        public ActionResult DetallesArea(int id = 0)
        {
            Area area = new IndicadoresContext().areas.Find(id);
            if (area == null)
                return HttpNotFound();
            return View(area);
        }

        #endregion

        #region Borrar Area

        public ActionResult BorrarArea(int id = 0) 
        {
            Area area = new IndicadoresContext().areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost, ActionName("BorrarArea")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarBorradoArea(int id)
        {
            using (var db = new IndicadoresContext())
            {
                Area area = db.areas.Find(id);
                db.areas.Remove(area);
                db.SaveChanges();

                return RedirectToAction("ListaAreas", "Admin");
            }
        }

        #endregion

        
        #endregion

        #region Manejo de Indicadores

        #region Lista de Indicadores
        public ActionResult ListaIndicadores()
        {
            return View(new IndicadoresContext().indicadores.ToList());
        }
        #endregion

        #region Crear Indicador
        public ActionResult NuevoIndicador()
        {
            ViewBag.proveedor_id = new SelectList(new IndicadoresContext().usuarios, "pk", "nombre_usuario");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoIndicador(Indicador nuevo_indicador)
        {
            if (ModelState.IsValid)
            {
                using (var db = new IndicadoresContext())
                {
                    db.indicadores.Add(nuevo_indicador);
                    db.SaveChanges();

                    return RedirectToAction("ListaIndicadores", "Admin");
                }
            }

            ViewBag.proveedor_id = new SelectList(new IndicadoresContext().usuarios, "pk", "nombre_usuario", nuevo_indicador.proveedor_id);
            return View(nuevo_indicador);
        }
        #endregion

        #region Editar Indicador
        public ActionResult EditarIndicador(int id)
        {
            Indicador indicador = new IndicadoresContext().indicadores.Find(id);
            if (indicador == null)
                return HttpNotFound();
            ViewBag.proveedor_id = new SelectList(new IndicadoresContext().usuarios, "pk", "nombre_usuario", indicador.proveedor_id);            
            return View(indicador);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarIndicador(Indicador editar_indicador)
        {
            if (ModelState.IsValid)
            {
                using (var db = new IndicadoresContext())
                {
                    db.Entry(editar_indicador).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ListaIndicadores", "Admin");
                }
            }

            ViewBag.proveedor_id = new SelectList(new IndicadoresContext().usuarios, "pk", "nombre_usuario", editar_indicador.proveedor_id);
            return View();
        }
        #endregion

        #region Detalles Indicador
        public ActionResult DetallesIndicador(int id)
        {
            Indicador indicador = new IndicadoresContext().indicadores.Find(id);
            if (indicador == null)
                return HttpNotFound();
            return View(indicador);
        }
        #endregion 

        #region BorrarIndicador
        public ActionResult BorrarIndicador(int id)
        {
            Indicador indicador = new IndicadoresContext().indicadores.Find(id);
            if (indicador == null)
                return HttpNotFound();
            return View(indicador);
        }

        [HttpPost, ActionName("BorrarIndicador")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarBorradoIndicador(int id)
        {
            using (var db = new IndicadoresContext())
            {
                Indicador indicador = db.indicadores.Find(id);
                db.indicadores.Remove(indicador);
                db.SaveChanges();

                return RedirectToAction("ListaIndicadores", "Admin");
            }
        }

        #endregion

        #region EnrolarArea

        public ActionResult AnexarArea(int id = 0)
        {
            var db = new IndicadoresContext();
            Indicador indicador = db.indicadores.Find(id);
            if (indicador == null)
                return HttpNotFound();
            IList<Area> areas = db.areas.ToList();

            var modelo = new VistaIndicadorArea();
            modelo.pk = indicador.pk;
            modelo.nombre = indicador.nombre;

            //modelo.aplica_a = indicador.aplica_en.Select(x => x.area.pk);
            modelo.aplica_a = db.indicadores_areas.Where(m => m.indicador_id == indicador.pk).Select(m => m.area_id);
            modelo.areas = areas
                .Select(x => new SelectListItem
                {
                    Value = x.pk.ToString(),
                    Text = x.nombre,
                })
                .ToList();


            

            return View(modelo);
        }
    
        [HttpPost]
        public ActionResult AnexarArea(VistaIndicadorArea modelo)
        {
            using (var db = new IndicadoresContext())
            {
                Indicador indicador = db.indicadores.Find(modelo.pk);
                if (modelo.aplica_a == null) foreach (var aplica in indicador.aplica_en.ToList()) indicador.aplica_en.Remove(aplica);
                else
                {
                    foreach (var item in indicador.aplica_en.ToList()) { db.indicadores_areas.Remove(item); }
                    db.SaveChanges();
                    foreach (var area_pk in modelo.aplica_a)
                        if (indicador.aplica_en.Where(m => m.area_id == area_pk).Count() == 0)
                        {
                            indicador.aplica_en.Add(new Aplica()
                            {
                                area = db.areas.Find(area_pk),
                                valor = null,
                                area_id = area_pk,
                                indicador = indicador,
                                indicador_id = indicador.pk
                            });
                        }

                    db.SaveChanges();
                }
            }

            return RedirectToAction("ListaIndicadores", "Admin");
        }

        #endregion

        #endregion

       
        public MultiSelectList ListaDeConsumo()
        {
            return ListaDeConsumo(null);
        }

        public MultiSelectList ListaDeConsumo(List<Area> seleccionados)
        {
            return new MultiSelectList(new IndicadoresContext().areas.ToList(), "pk", "nombre", seleccionados);
        }

        public MultiSelectList ListaDeIndicadoresDisponibles()
        {
            return ListaDeIndicadoresDisponibles(null);
        }

        public MultiSelectList ListaDeIndicadoresDisponibles(List<Indicador> seleccionados)
        {
            return new MultiSelectList(new IndicadoresContext().indicadores.ToList(), "pk", "nombre", seleccionados);
        }
    }
}

