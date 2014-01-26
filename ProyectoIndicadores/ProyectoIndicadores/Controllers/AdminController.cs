using ProyectoIndicadores.Forms;
using ProyectoIndicadores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProyectoIndicadores.Controllers
{
    public class AdminController : Controller
    {
                
        //
        // GET: /Admin/
        
        // Dashboard Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Manejo de Usuarios
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

                return RedirectToAction("Index", "Admin");
                
            }
            return View(nuevo_usuario);
        }
        
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

        #region Manejo de Sectores

        

        #endregion
    }
}

