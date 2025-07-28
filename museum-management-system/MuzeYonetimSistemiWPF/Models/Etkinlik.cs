using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Etkinlik
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public string Tur { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string Aciklama { get; set; }

        public int? TurID { get; set; }   // FK → EtkinlikTurleri

        public string EtkinlikTuruAd { get; set; }  // Sadece görüntüleme için
    }
}
