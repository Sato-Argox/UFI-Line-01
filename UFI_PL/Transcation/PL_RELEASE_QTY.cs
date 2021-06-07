using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFI_COMMON;

namespace UFI_PL
{
    public class PL_RELEASE_QTY : Common
    {
        public string Station { get; set; }
        public string Model { get; set; }
        public string Child_Part_No { get; set; }
        public int Qty { get; set; }

    }
}
