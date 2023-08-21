using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities.ViewModel
{
    public class RegisterModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(30)]
        public string KullaniciAdi { get; set; }
        [DisplayName("E-mail"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(200),EmailAddress(ErrorMessage ="{0} için geçerli bir e-mail giriniz")]
        public string Email { get; set; }

        [DisplayName("Şifre"), StringLength(20), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Sifre { get; set; }
        [DisplayName("Şifre(Tekrar)"), StringLength(20), Required(ErrorMessage = "{0} alanı boş geçilemez"),Compare(nameof(Sifre),ErrorMessage ="{1} ile {0} birbirleriyle uyuşmuyor")]
        public string SifreTekrar { get; set; }
    }
}
