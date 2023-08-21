using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using MakaleWeb_MVC.Filter;
using MakaleWeb_MVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MakaleWeb_MVC.Controllers
{
    public class YorumController : Controller
    {
        // GET: Yorum
        YorumYonet yy = new YorumYonet();
        Yorum yorum=new Yorum();
        MakaleYonet my=new MakaleYonet();
        public ActionResult YorumGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale=my.MakaleBul(id.Value);
            if (makale==null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialPageYorumlar",makale.Yorumlar);
        }
        [Auth]
        [HttpPost]
        public ActionResult YorumGuncelle(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yorum = yy.YorumBul(id.Value);
            if (yorum==null)
            {
                return HttpNotFound();
            }
            yorum.Text = text;
            if (yy.YorumUpdate(yorum)>0)
            {
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { hata = true }, JsonRequestBehavior.AllowGet); 
    
        }
        [Auth]
        public ActionResult YorumSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yorum = yy.YorumBul(id.Value);
            if (yorum == null)
            {
                return HttpNotFound();
            }
          
            if (yy.YorumDelete(yorum) > 0)
            {
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
         
        }
        [Auth]
        [HttpPost]
        public ActionResult YorumEkle(int? id,Yorum nesne)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale= my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            nesne.Makale = makale;
            nesne.Kullanici = SessionUser.Login;
            if (yy.YorumKaydet(nesne) > 0)
            {
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
        }

    }
}