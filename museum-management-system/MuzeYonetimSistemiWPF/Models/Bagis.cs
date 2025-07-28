using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class Bagis
    {
        public int ID { get; set; }
        public int BagisciID { get; set; }
        public decimal Miktar { get; set; }
        public DateTime BagisTarihi { get; set; }
        public string KullanimAlani { get; set; }
        public Bagisci Bagisci { get; set; }
    }
}

