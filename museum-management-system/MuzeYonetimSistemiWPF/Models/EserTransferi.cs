using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class EserTransferi
    {
        public int ID { get; set; }
        public int EserID { get; set; }
        public string KaynakMuze { get; set; }
        public string HedefMuze { get; set; }
        public DateTime Tarih { get; set; }
        public string TransferDurumu { get; set; }
    }
}

