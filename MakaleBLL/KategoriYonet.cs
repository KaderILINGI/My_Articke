using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KategoriYonet
    {
        Repository<Kategori> rep_kat = new Repository<Kategori>();
        MakaleBLLSonuc<Kategori> sonuc=new MakaleBLLSonuc<Kategori>();
        public List<Kategori> Listele()
        {
            return rep_kat.Liste();
        }

        public Kategori KategoriBul (int id)
        {
            return rep_kat.Find(x => x.Id == id);
        }

        public MakaleBLLSonuc<Kategori> KategoriEkle(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x => x.Baslik == model.Baslik);
            if (sonuc.nesne!=null)
            {
                sonuc.hatalar.Add("Bu kategori kayıtlı");
                
            }
            else
            {
               if(rep_kat.Insert(model)<1)
               {
                    sonuc.hatalar.Add("Kategori kadedilemedi");
               }

            }
            return sonuc;
        }

        public MakaleBLLSonuc<Kategori> KategoriUpdate(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x => x.Id == model.Id);
            Kategori kategori = rep_kat.Find(x => x.Baslik == model.Baslik && x.Id != model.Id);
            if (sonuc.nesne!=null && kategori==null)
            {
                sonuc.nesne.Baslik = model.Baslik;
                sonuc.nesne.Aciklama = model.Aciklama;
               if( rep_kat.Update(sonuc.nesne)<1)
                {
                    sonuc.hatalar.Add("Kategori güncellenemedi.");
                }
            }
            else
            {
                if (kategori!=null)
                {
                    sonuc.hatalar.Add("Bu kategori zaten kayıtlı");
                }
                else
                {
                    sonuc.hatalar.Add("Kategori bulunamadı.");
                }               
            }
          
            return sonuc;
        }

        public MakaleBLLSonuc<Kategori> KategoriSil(int id)
        {
            Kategori kategori= sonuc.nesne = rep_kat.Find(x => x.Id == id);
            //Ketegorinin Makalelerini Sil
            //Makalenin yorumlarını sil
            //Makalenin beğenilerini sil
            Repository<Makale> rep_makale = new Repository<Makale>();
            Repository<Yorum> rep_yorum = new Repository<Yorum>();
            Repository<Begeni> rep_begeni = new Repository<Begeni>();
            foreach (Makale item in kategori.Makaleler.ToList())
            {
                foreach (Yorum yorum in item.Yorumlar.ToList())
                {
                    rep_yorum.Delete(yorum);
                }
                foreach (Begeni begeni in item.Begeniler.ToList())
                {
                    rep_begeni.Delete(begeni);
                }
                rep_makale.Delete(item);
            }
            rep_kat.Delete(kategori);
            return sonuc;
        }
    }
}
