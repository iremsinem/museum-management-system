using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Admin
    {
        public int ID { get; set; }
        public string KullaniciAdi { get; set; }
        public string SifreHash { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string YetkiSeviyesi { get; set; }
    }
}

