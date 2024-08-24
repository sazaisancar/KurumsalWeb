using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class KategoriiController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Kategorii
        public ActionResult Index()
        {
            return View(db.Kategorii.ToList());
        }

        // GET: Kategorii/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorii kategorii = db.Kategorii.Find(id);
            if (kategorii == null)
            {
                return HttpNotFound();
            }
            return View(kategorii);
        }

        // GET: Kategorii/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorii/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriID,KategoriAd,Aciklama")] Kategorii kategorii)
        {
            if (ModelState.IsValid)
            {
                db.Kategorii.Add(kategorii);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategorii);
        }

        // GET: Kategorii/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorii kategorii = db.Kategorii.Find(id);
            if (kategorii == null)
            {
                return HttpNotFound();
            }
            return View(kategorii);
        }

        // POST: Kategorii/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriID,KategoriAd,Aciklama")] Kategorii kategorii)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategorii).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorii);
        }

        // GET: Kategorii/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorii kategorii = db.Kategorii.Find(id);
            if (kategorii == null)
            {
                return HttpNotFound();
            }
            return View(kategorii);
        }

        // POST: Kategorii/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategorii kategorii = db.Kategorii.Find(id);
            db.Kategorii.Remove(kategorii);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
