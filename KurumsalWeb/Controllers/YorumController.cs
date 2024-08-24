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
    public class YorumController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Yorum
        // Bu aksiyon, yorumların listelendiği ana sayfayı render eder.
        public ActionResult Index()
        {
            // Yorumları Blog ilişkisiyle birlikte yükleyip, YorumId'ye göre azalan sırada listeleyin.
            var yorumlar = db.Yorum.Include(y => y.Blog).OrderByDescending(x => x.YorumId).ToList();
            return View(yorumlar);
        }

        // GET: Yorum/Details/5
        // Bu aksiyon, belirli bir yorumun detaylarını görüntüler.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                // ID parametresi sağlanmadıysa, kötü istek hatası döndür.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Yorum nesnesini bulur ve varsa detay sayfasına gönderir.
            var yorum = db.Yorum.Include(y => y.Blog).SingleOrDefault(y => y.YorumId == id);
            if (yorum == null)
            {
                // Yorum bulunamadıysa, 404 hata döndür.
                return HttpNotFound();
            }
            return View(yorum);
        }

        // GET: Yorum/Create
        // Yorum eklemek için kullanılan formu render eder.
        public ActionResult Create()
        {
            // Blog başlıklarını içeren bir SelectList oluşturur.
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik");
            return View();
        }

        // POST: Yorum/Create
        // Yeni bir yorum oluşturur ve veritabanına kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,Onay,BlogId")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                db.Yorum.Add(yorum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Model geçersizse, tekrar formu gösterirken BlogId için SelectList ayarlar.
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }

        // GET: Yorum/Edit/5
        // Belirli bir yorumu düzenlemek için formu render eder.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // ID parametresi sağlanmadıysa, kötü istek hatası döndür.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Yorum nesnesini bulur ve varsa düzenleme formuna gönderir.
            var yorum = db.Yorum.Find(id);
            if (yorum == null)
            {
                // Yorum bulunamadıysa, 404 hata döndür.
                return HttpNotFound();
            }

            // Blog başlıklarını içeren bir SelectList oluşturur.
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }

        // POST: Yorum/Edit/5
        // Var olan bir yorumu günceller ve veritabanında kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,Onay,BlogId")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yorum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Model geçersizse, tekrar formu gösterirken BlogId için SelectList ayarlar.
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }

        // GET: Yorum/Delete/5
        // Belirli bir yorumu silmek için onay sayfasını render eder.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                // ID parametresi sağlanmadıysa, kötü istek hatası döndür.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Yorum nesnesini bulur ve varsa silme onay sayfasına gönderir.
            var yorum = db.Yorum.Find(id);
            if (yorum == null)
            {
                // Yorum bulunamadıysa, 404 hata döndür.
                return HttpNotFound();
            }
            return View(yorum);
        }

        // POST: Yorum/Delete/5
        // Yorumun veritabanından silinmesini sağlar.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var yorum = db.Yorum.Find(id);
            if (yorum != null)
            {
                db.Yorum.Remove(yorum);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Dispose metodunu override ederek, kullanılan kaynakları serbest bırakır.
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
