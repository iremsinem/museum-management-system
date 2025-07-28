using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class MuzeGideri
    {
        public int ID { get; set; }
        public string Aciklama { get; set; }
        public decimal? Tutar { get; set; }
        public DateTime Tarih { get; set; }
    }
}
