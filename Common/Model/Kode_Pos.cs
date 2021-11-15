using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Kode_Pos
    {
            public int ID { get; set; }
            public int NO_KODE_POS { get; set; }
            public string KELURAHAN { get; set; }
            public string KECAMATAN { get; set; }
            public string JENIS { get; set; }
            public string KABUPATEN { get; set; }
            public string PROPINSI { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public DateTime MODIFIED_DATE { get; set; }
        
    }
}
