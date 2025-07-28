using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class MuzeGeliri
    {
        public int ID { get; set; }
        public string KaynakTuru { get; set; }
        public decimal? Tutar { get; set; }
        public DateTime Tarih { get; set; }
    }
}

