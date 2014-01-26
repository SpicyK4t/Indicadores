﻿using ProyectoIndicadores.Models;
using ProyectoIndicadores.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ProyectoIndicadores.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

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
    }

}