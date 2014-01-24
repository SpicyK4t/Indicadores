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
    public class SectorController : Controller
    {
        private IndicadoresContext db = new IndicadoresContext();

        //
        // GET: /Sector/

        public ActionResult Index()
        {
            return View(db.sectores.ToList());
        }

        //
        // GET: /Sector/Details/5

        public ActionResult Details(int id = 0)
        {
            Sector sector = db.sectores.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        //
        // GET: /Sector/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sector/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.sectores.Add(sector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sector);
        }

        //
        // GET: /Sector/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sector sector = db.sectores.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        //
        // POST: /Sector/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sector);
        }

        //
        // GET: /Sector/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sector sector = db.sectores.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        //
        // POST: /Sector/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sector sector = db.sectores.Find(id);
            db.sectores.Remove(sector);
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