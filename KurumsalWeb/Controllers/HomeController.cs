using KurumsalWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using KurumsalWeb.Models.Model;
using System.Net.Mail;
using System.Net;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {

        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
       
        
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        }

        public ActionResult HizmetPartial()
        {

            return View(db.Hizmet.ToList());


        }
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Hakkimizda.SingleOrDefault());
        }

        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));

        }

        public ActionResult Iletisim()
        {

            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());

        }
        //[HttpPost]
        //public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
        //{
        //    if (adsoyad != null && email != null)
        //    {

        //        WebMail.SmtpServer = "smtp.gmail.com";
        //        WebMail.EnableSsl = false;
        //        WebMail.UserName = "sancarsezai961@gmail.com";
        //        WebMail.Password = "Cemosezo6321. ";
        //        WebMail.SmtpPort = 587;
        //        WebMail.Send("sancarsezai961@gmail.com", konu, email + "-" + mesaj);
        //        ViewBag.Uyari = "Mesajiniz gönderildi";
        //    }
        //    else
        //    {

        //        ViewBag.Uyari = "Hata Oluştu.Tekrar deneyiniz";

        //    }


        //    return View();

        //}
 

    [HttpPost]
    public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
    {
        if (!string.IsNullOrEmpty(adsoyad) && !string.IsNullOrEmpty(email))
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sancarsezai961@gmail.com", "Cemosezo6321."),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sancarsezai961@gmail.com"),
                    Subject = konu,
                    Body = $"{email} - {mesaj}",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add("sancarsezai961@gmail.com");

                smtpClient.Send(mailMessage);
                ViewBag.Uyari = "Mesajınız gönderildi";
            }
            catch (Exception ex)
            {
                ViewBag.Uyari = $"Hata oluştu: {ex.Message}";
            }
        }
        else
        {
            ViewBag.Uyari = "Hata oluştu. Lütfen tekrar deneyiniz.";
        }

        return View();
    }




    public ActionResult Blog(int Sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Blog.Include("Kategorii").OrderByDescending(x => x.BlogId).ToPagedList(Sayfa, 3));

        }
        public ActionResult KategoriBlog(int id, int Sayfa = 1)

        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var b = db.Blog.Include("Kategorii").OrderByDescending(x => x.BlogId).Where(x => x.kategorii.KategoriID == id).ToPagedList(Sayfa , 5);
            return View(b);

        }

        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var b = db.Blog.Include("Kategorii").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();

            return View(b);

        }
        public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogid)
        {
            // İçerik boşsa, kullanıcıya uygun bir hata mesajı döndür
            if (string.IsNullOrWhiteSpace(icerik))
            {
                return Json(new { success = false, message = "Yorum içeriği boş olamaz." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                // Yeni yorum oluştur
                var yorum = new Yorum
                {
                    AdSoyad = adsoyad,
                    Eposta = eposta,
                    Icerik = icerik,
                    BlogId = blogid,
                    Onay = false // Yorumun admin tarafından onaylanması gerektiğini belirtiyoruz
                };

                // Yorum ekle ve değişiklikleri kaydet
                db.Yorum.Add(yorum);
                db.SaveChanges();

                // Başarı mesajı döndür
                return Json(new { success = true, message = "Yorumunuz eklendi, kontrol edildikten sonra yayınlanacaktır." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Hata mesajını döndür
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BlogKategoriPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Kategorii.Include("Blogs").ToList().OrderBy(x => x.KategoriAd));
        }
        public ActionResult BlogKayitPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Blog.ToList().OrderByDescending(x => x.BlogId));
        }
        public ActionResult FooterPartial()

        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            return PartialView();
        }

    }
}