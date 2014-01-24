using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIndicadores.Models;

namespace ProyectoIndicadores.Controllers
{
    public class IndicadorController : Controller
    {
        private IndicadoresContext db = new IndicadoresContext();

        //
        // GET: /Indicador/

        public ActionResult Index()
        {
            var indicadores = db.indicadores.Include(i => i.proveedor);
            return View(indicadores.ToList());
        }

        //
        // GET: /Indicador/Details/5

        public ActionResult Details(int id = 0)
        {
            Indicador indicador = db.indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            return View(indicador);
        }

        //
        // GET: /Indicador/Create

        public ActionResult Create()
        {
            ViewBag.proveedor_id = new SelectList(db.usuarios, "pk", "correo");
            return View();
        }

        //
        // POST: /Indicador/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Indicador indicador)
        {
            if (ModelState.IsValid)
            {
                db.indicadores.Add(indicador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proveedor_id = new SelectList(db.usuarios, "pk", "correo", indicador.proveedor_id);
            return View(indicador);
        }

        //
        // GET: /Indicador/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Indicador indicador = db.indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            ViewBag.proveedor_id = new SelectList(db.usuarios, "pk", "correo", indicador.proveedor_id);
            return View(indicador);
        }

        //
        // POST: /Indicador/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Indicador indicador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(indicador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proveedor_id = new SelectList(db.usuarios, "pk", "correo", indicador.proveedor_id);
            return View(indicador);
        }

        //
        // GET: /Indicador/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Indicador indicador = db.indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            return View(indicador);
        }

        //
        // POST: /Indicador/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Indicador indicador = db.indicadores.Find(id);
            db.indicadores.Remove(indicador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}