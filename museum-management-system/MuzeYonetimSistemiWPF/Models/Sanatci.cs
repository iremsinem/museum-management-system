using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Sanatci
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public DateTime? OlumTarihi { get; set; }
        public string Ulke { get; set; }
        public string Biyografi { get; set; }
    }
}

