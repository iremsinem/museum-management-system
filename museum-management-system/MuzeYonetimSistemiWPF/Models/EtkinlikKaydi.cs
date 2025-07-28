using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class EtkinlikKaydi
    {
        public int ID { get; set; }
        public int ZiyaretciID { get; set; }
        public int EtkinlikID { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}

