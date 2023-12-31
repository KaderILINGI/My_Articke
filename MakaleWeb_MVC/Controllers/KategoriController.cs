﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using MakaleWeb_MVC.Filter;
using MakaleWeb_MVC.Models;

namespace MakaleWeb_MVC.Controllers
{
    [Auth]
    [AuthAdmin]
    public class KategoriController : Controller
    {
        KategoriYonet ky=new KategoriYonet();
        MakaleBLLSonuc<Kategori> sonuc = new MakaleBLLSonuc<Kategori>();

        
        public ActionResult Index()
        {
            return View(ky.Listele());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori =ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
           
            if (ModelState.IsValid)
            {
                sonuc = ky.KategoriEkle(kategori);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(kategori);
                }

                CacheHelper.CacheTemizle();
                return RedirectToAction("Index");
            }

            return View(kategori);
        }


        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori =ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                sonuc= ky.KategoriUpdate(kategori);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(kategori);
                }
                CacheHelper.CacheTemizle();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }

            return View(kategori);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            ky.KategoriSil(id);
            CacheHelper.CacheTemizle();
            return RedirectToAction("Index");
        }

      

    }
}
