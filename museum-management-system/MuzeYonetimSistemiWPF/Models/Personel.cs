using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Personel
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string Gorev { get; set; }
        public decimal? Maas { get; set; }
        public DateTime IseBaslamaTarihi { get; set; }

        public ICollection<EserBakimKaydi> BakimKayitlari { get; set; }
    }
}
