﻿using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class YorumYonet
    {
        Repository<Yorum> rep_yorum = new Repository<Yorum>();

        public Yorum YorumBul(int id)
        {
            return rep_yorum.Find(x => x.Id == id);
        }
        public int YorumUpdate(Yorum yorum)
        {
            return rep_yorum.Update(yorum);
        }
        public int YorumDelete(Yorum yorum)
        {
            return rep_yorum.Delete(yorum);
        }

        public int YorumKaydet(Yorum nesne)
        {
            return rep_yorum.Insert(nesne);
        }
    }
}
