using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class ZiyaretciGirisKaydi
    {
        public int ID { get; set; }
        public int ZiyaretciID { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime? CikisTarihi { get; set; }
        public int BiletTuru { get; set; }
        public int? SergiID { get; set; }
        public int? EtkinlikID { get; set; }
    }
}
