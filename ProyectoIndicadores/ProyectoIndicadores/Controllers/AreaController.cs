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
    public class AreaController : Controller
    {
        private IndicadoresContext db = new IndicadoresContext();

        //
        // GET: /Area/

        public ActionResult Index()
        {
            var areas = db.areas.Include(a => a.sector);
            return View(areas.ToList());
        }

        //
        // GET: /Area/Details/5

        public ActionResult Details(int id = 0)
        {
            Area area = db.areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // GET: /Area/Create

        public ActionResult Create()
        {
            ViewBag.sector_id = new SelectList(db.sectores, "pk", "nombre");
            return View();
        }

        //
        // POST: /Area/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Area area)
        {
            if (ModelState.IsValid)
            {
                db.areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sector_id = new SelectList(db.sectores, "pk", "nombre", area.sector_id);
            return View(area);
        }

        //
        // GET: /Area/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Area area = db.areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            ViewBag.sector_id = new SelectList(db.sectores, "pk", "nombre", area.sector_id);
            return View(area);
        }

        //
        // POST: /Area/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sector_id = new SelectList(db.sectores, "pk", "nombre", area.sector_id);
            return View(area);
        }

        //
        // GET: /Area/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Area area = db.areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // POST: /Area/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.areas.Find(id);
            db.areas.Remove(area);
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