using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using MakaleWeb_MVC.Filter;
using MakaleWeb_MVC.Models;


namespace MakaleWeb_MVC.Controllers
{
   
    public class MakaleController : Controller
    {
        MakaleYonet my = new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        MakaleBLLSonuc<Makale> sonuc = new MakaleBLLSonuc<Makale>();
        BegeniYonet by = new BegeniYonet();
        // GET: Makale
        [Auth]
        public ActionResult Index()
        {
            if (SessionUser.Login!=null)
            {
                return View(my.Listele().Where(x => x.Kullanici.Id == SessionUser.Login.Id));
            }
            return View(my.Listele());
        }

        // GET: Makale/Details/5
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: Makale/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(),"Id","Baslik");
            return View();
        }

        // POST: Makale/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            makale.Kategori = ky.KategoriBul(makale.Kategori.Id);
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", makale.Kategori.Id);
            if (ModelState.IsValid)
            {
                makale.Kullanici = SessionUser.Login;
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);
                sonuc=my.MakaleEkle(makale);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
          
            return View(makale);
        }

        // GET: Makale/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            Makale makale = my.MakaleBul(id.Value);
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik",makale.Kategori.Id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if (makale == null)
            {
                return HttpNotFound();
            }
           
            return View(makale);
        }

        // POST: Makale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit(Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", makale.Kategori.Id);
            if (ModelState.IsValid)
            {
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);
                sonuc= my.MakaleUpdate(makale);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        // GET: Makale/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Makale/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }
        [Auth]
        [HttpPost]
        public ActionResult MakaleGetir(int[] mid)
        {
            List<int> mliste = null;
            if (SessionUser.Login != null&& mid!=null)
            {
                mliste = by.Liste().Where(x => x.Kullanici.Id == SessionUser.Login.Id && mid.Contains(x.Makale.Id)).Select(x => x.Makale.Id).ToList();
            }
            return Json(new {liste=mliste});
        }
        [Auth]
        [HttpPost]
        public ActionResult MakaleBegen(int makaleid,bool begeni)
                {
            Begeni like= by.BegeniBul(makaleid, SessionUser.Login.Id);
            Makale makale = my.MakaleBul(makaleid);
            int sonuc = 0;
            if (like!=null && begeni==false)
            {
                sonuc= by.BegeniSil(like);
            }
            else if (like==null&&begeni==true)
            {
                sonuc=by.BegeniEkle(new Begeni()
                {
                    Kullanici = SessionUser.Login,
                    Makale = makale
                });
            }
            if (sonuc>0)
            {
                if (begeni)
                {
                    makale.BegeniSayisi++;
                    
                }
                else
                {
                    makale.BegeniSayisi--;
                }
                my.MakaleUpdate(makale);
                return Json(new { hata = false, begenisayisi = makale.BegeniSayisi });
            }
            else
            {
                return Json(new { hata = true, begenisayisi = makale.BegeniSayisi });
            }
        }
        
        public ActionResult MakaleGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPageDevamı", makale);
        }
    }
}
