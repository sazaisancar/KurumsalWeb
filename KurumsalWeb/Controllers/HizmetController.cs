using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HizmetController : Controller
    {
       private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hizmet
        // Hizmetlerin listelendiği ana sayfayı gösterime işlemini sağladı Index
        public ActionResult Index()
        {
            // Veritabanından tüm Hizmet kayıtlarını alır ve görünümde listeler.
            return View(db.Hizmet.ToList());
        }
        // GET: Hizmet/Create
        // Yeni Hizmet oluşturmak için bir form sayfası sağlar.
        public ActionResult Create()
        {
            return View(); 
        }
        // POST: Hizmet/Create
        // Yeni bir Hizmet kaydı oluşturur.
        [HttpPatch]
        [ValidateInput(false)]  
        public ActionResult Create(Hizmet hizmet , HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı bir resim yüklemişse
                if (ResimURL != null)
                {
                    // Yüklenen resim üzerinde işlem sağladı kısım
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);
                    // Resmin benzersiz bir isimle kaydedilmesini sağlar
                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Hizmet/" + logoname);
                    hizmet.ResimURL = "/Uploads/Hizmet/" + logoname;
                }
                db.Hizmet.Add(hizmet);
                db.SaveChanges(); // Veritabanındaki değişiklikleri kaydeme işlemini sağlar
                return RedirectToAction("Index"); // Listeleme sayfasına yönlendirir
            }
            return View(hizmet); 
        
        }
        // GET: Hizmet/Edit/5
        // Belirtilen ID'ye sahip Hizmet kaydını düzenleme formunu getirir.
        public ActionResult Edit( int? id) 
        { 
            if (id== null)
            {
                // ID verilmemişse hata mesajı gösterir
                ViewBag.Uyari = "Güncellenecek Hizmet Bulunamadı";
            }
            // ID'ye sahip Hizmet kaydını veritabanından blumasını sağladığım yer
            var hizmet = db.Hizmet.Find(id);
            if (hizmet == null)
            {
                return HttpNotFound(); // Hizmet bulunamazsa 404 hata sayfası döner
            }
          return View(hizmet);  // Hizmet verileriyle düzenleme formunu gösterir

        }

        // POST: Hizmet/Edit/5
        // Var olan bir Hizmet kaydını güncelleme işlemini sağladığım kod alanı
        [HttpPost]
        [ValidateInput(false)]  
        public ActionResult Edit(int? id, Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            
            if (ModelState.IsValid)
            {
                // ID'ye sahip Hizmet kaydını veritabanından bulur
                var h = db.Hizmet.Where(x => x.HizmetId == id).SingleOrDefault();   
                if (ResimURL != null)

                {
                    // Mevcut resim varsa, onu siler
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }
                    // Yeni resim üzerinde işlem yapar
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    // Resmi benzersiz bir isimle kaydetme işlemini sağlar
                    string hizmetname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Hizmet/" + hizmetname);
                    h.ResimURL = "/Uploads/Hizmet/" + hizmetname;

                }
                h.Baslık = hizmet.Baslık;
                h.Aciklama = hizmet.Aciklama;
                db.SaveChanges(); 
                return RedirectToAction("Index");
            }

            return View(hizmet);
        
        }
        public ActionResult Delete(int id)
        { 
        
            if (id == null) 
            { 
             return HttpNotFound();
            }
            var h = db.Hizmet.Find(id);

            if (h == null) 
            {
                return HttpNotFound();
            }
            db.Hizmet.Remove(h);
            db.SaveChanges();
            return RedirectToAction("Index");

         
        
        }
    }
}