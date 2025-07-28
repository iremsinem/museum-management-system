using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class EserBakimKaydi
    {
        public int ID { get; set; }
        public int EserID { get; set; }
        public int PersonelID { get; set; }
        public DateTime BakimTarihi { get; set; }
        public string YapilanIslem { get; set; }
        
        public Personel Personel { get; set; }
    }
}

