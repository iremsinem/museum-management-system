using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Bagisci
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Kurum { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public ICollection<Bagis> Bagislar { get; set; }
    }
}
