using KurumsalWeb.Models;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class AdminController : Controller
    {

        KurumsalDBContext db =   new KurumsalDBContext();
        // GET: Admin
        public ActionResult Index()
        {

            ViewBag.BlogSay = db.Blog.Count();

            ViewBag.KategoriSay = db.Kategorii.Count();

            ViewBag.HizmetSay = db.Hizmet.Count();

            ViewBag.YorumSay = db.Yorum.Count();

            var sorgu = db.Kategorii.ToList();
            return View(sorgu);
        }
        public ActionResult Login ()
        {

            return View();  
        }
        //ADMİN ŞİFRE GİRİŞ VERİ TABAINDA ALIP KONTROL EDEN KODUM
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var login =db.Admin.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();
            if (login.Eposta==admin.Eposta &&  login.Sifre==admin.Sifre)
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"]=login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index" , "Admin");
            }
            ViewBag.Uyari = "Giriş bilgileriniz hatalı olmaktadır.";
            return View(admin);

        }
        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
            
        }


        public ActionResult Adminler()


            { 
            
            return View( db.Admin.ToList()); 
        }

        [HttpPost]
        public ActionResult Ceate( Admin admin  , string sifre , string eposta  )
        {
            if (ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");

            }

           return View(admin);   
        }

       
        public ActionResult Edit(int id) 
        
        { 
           var a = db.Admin.Where( x => x.AdminId == id).SingleOrDefault(); 
         return View(a); 
        
        }

        [HttpPost]
        public ActionResult Edit(int id, Admin admin, string sifre, string eposta)

        {

            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                a.Sifre = Crypto.Hash(sifre, "MD5");
                a.Eposta = admin.Eposta;
                a.Yetki = admin.Yetki;        
                db.SaveChanges();
                return RedirectToAction("Adminler");

            }


            return View(admin);

        }
        public ActionResult Delete(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();

            if (a!= null)
            {
                db.Admin.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }


            return View(a);

        }

        public ActionResult SifremiUnuttum()


        {

            return View();  

        }
        [HttpPost]
        public ActionResult SifremiUnuttum( string eposta)


        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();
            if (mail !=null)
            {
                Random rnd = new Random();  

                int yenisifre =rnd .Next(); 

                Admin admin = new Admin();

                mail.Sifre = Crypto.Hash( Convert.ToString(yenisifre) , "MD5");
                db.SaveChanges();



                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "sancarsezai961@gmail.com";
                WebMail.Password = "Cemosezo6321. ";
                WebMail.SmtpPort = 587;
                WebMail.Send(eposta , " Admin Panel Giriş Şifreniz" ,  "Şifreniz:" + yenisifre);
                ViewBag.Uyari = "Şifreniz başalı bir şekilde  gönderildi";
            }
            else
            {

                ViewBag.Uyari = "Hata Oluştu.Tekrar deneyiniz";

            }


            return View();

            

        }



    }
}