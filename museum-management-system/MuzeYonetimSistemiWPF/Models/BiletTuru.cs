using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class BiletTuru
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
        public int GecerlilikSuresi { get; set; }
    }
}

