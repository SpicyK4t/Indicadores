﻿using ProyectoIndicadores.Models;
using ProyectoIndicadores.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.Entity;


namespace ProyectoIndicadores.Controllers
{
    public class HomeController : Controller
    {
        private IndicadoresContext db = new IndicadoresContext();

        public ActionResult SinPermisos()
        {
            return View();
        }

        public ActionResult VerIndicadoresArea(int id)
        {
            ViewBag.area = db.areas.Find(id).nombre;
            return View(db.indicadores_areas.Where(m => m.area_id == id).Include(m => m.indicador));
        }

        public ActionResult Index()
        {      
            var usuario = db.usuarios.FirstOrDefault(m => m.nombre_usuario == User.Identity.Name);
            if (usuario == null) return HttpNotFound();
            
            return View(usuario.provee.ToList());
        }

        public ActionResult Captura(int id = 0)
        {            
            var usuario = db.usuarios.FirstOrDefault(m => m.nombre_usuario == User.Identity.Name);
            var indicador = db.indicadores.Find(id);
            if (indicador == null || usuario == null ) return HttpNotFound();
            if (indicador.proveedor_id != usuario.pk)
                return HttpNotFound();
            ViewBag.indid = id;
            return View(db.indicadores_areas.Where(m => m.indicador_id == id).Include(m=>m.area));
   
            
        }

        [HttpPost]
        public ActionResult Captura(int id, int? i, float? v)        
        {
            try
            {
                var aplica = db.indicadores_areas.Find(i);
                aplica.valor = v;
                db.Entry(aplica).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                ViewBag.error = "No es un valor numerico";
            }

            ViewBag.indid = id;
            return View(db.indicadores_areas.Where(m => m.indicador_id == id).Include(m => m.area));
        }
       

        #region Control de Sesion 
        public ActionResult Ingreso()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Ingreso(FormularioIngreso formulario_ingreso)
        {
            if (ModelState.IsValid)
            {
                var cypher = new SimpleCrypto.PBKDF2();
                using (var db = new IndicadoresContext())
                {
                    Usuario usuario = db.usuarios.FirstOrDefault(u => u.nombre_usuario == formulario_ingreso.usuario);
                    if (usuario != null)
                    {
                        if (usuario.contrasenia == cypher.Compute(formulario_ingreso.contrasenia, usuario.salt))
                        {
                            FormsAuthentication.SetAuthCookie(usuario.nombre_usuario, true);
                            if (usuario.is_admin)
                                return RedirectToAction("Index", "Admin");
                            else
                                return RedirectToAction("Index", "Home");
                        }
                    }
                }

            }

            return View(formulario_ingreso);
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Ingreso", "Home");
        }

        #endregion


    }

}