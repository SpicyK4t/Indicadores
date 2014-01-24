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
    public class AplicaController : Controller
    {
        private IndicadoresContext db = new IndicadoresContext();

        //
        // GET: /Aplica/

        public ActionResult Index()
        {
            var indicadores_areas = db.indicadores_areas.Include(a => a.indicador);
            return View(indicadores_areas.ToList());
        }

        //
        // GET: /Aplica/Details/5

        public ActionResult Details(int id = 0)
        {
            Aplica aplica = db.indicadores_areas.Find(id);
            if (aplica == null)
            {
                return HttpNotFound();
            }
            return View(aplica);
        }

        //
        // GET: /Aplica/Create

        public ActionResult Create()
        {
            ViewBag.indicador_id = new SelectList(db.indicadores, "pk", "nombre");
            return View();
        }

        //
        // POST: /Aplica/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aplica aplica)
        {
            if (ModelState.IsValid)
            {
                db.indicadores_areas.Add(aplica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.indicador_id = new SelectList(db.indicadores, "pk", "nombre", aplica.indicador_id);
            return View(aplica);
        }

        //
        // GET: /Aplica/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Aplica aplica = db.indicadores_areas.Find(id);
            if (aplica == null)
            {
                return HttpNotFound();
            }
            ViewBag.indicador_id = new SelectList(db.indicadores, "pk", "nombre", aplica.indicador_id);
            return View(aplica);
        }

        //
        // POST: /Aplica/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aplica aplica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aplica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.indicador_id = new SelectList(db.indicadores, "pk", "nombre", aplica.indicador_id);
            return View(aplica);
        }

        //
        // GET: /Aplica/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Aplica aplica = db.indicadores_areas.Find(id);
            if (aplica == null)
            {
                return HttpNotFound();
            }
            return View(aplica);
        }

        //
        // POST: /Aplica/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aplica aplica = db.indicadores_areas.Find(id);
            db.indicadores_areas.Remove(aplica);
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