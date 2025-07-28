using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Eser
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public int Tur_ID { get; set; }
        public int Sanatci_ID { get; set; }
        public int YapimYili { get; set; }
        public string BulunduguMuze { get; set; }
        public string MevcutDurum { get; set; }
        public string DijitalKoleksiyonURL { get; set; }
    }

}
