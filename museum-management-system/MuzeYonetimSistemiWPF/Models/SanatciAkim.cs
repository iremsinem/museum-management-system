using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Models
{
    public class SanatciAkim
    {
        public int SanatciID { get; set; }
        public int AkimID { get; set; }
        public string SanatciAd { get; set; }
        public string AkimAd { get; set; }

        // ✔️ Görüntüleme için ID + Ad formatlı string
        public string SanatciGosterim => $"{SanatciID} ({SanatciAd})";
        public string AkimGosterim => $"{AkimID} ({AkimAd})";
    }
}

