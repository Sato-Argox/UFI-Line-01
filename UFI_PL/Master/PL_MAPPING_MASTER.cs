using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFI_COMMON;
namespace UFI_PL
{
    public class PL_MAPPING_MASTER : Common
    {
        public string StationNo { get; set; }

        public string ParentPartNo { get; set; }
        public string ChildPartNo { get; set; }
        public int EnterQty { get; set; }
        public string PLCValueId { get; set; }
        public bool Active { get; set; }
        public DataTable MAPPING_DETAILS { get; set; }
    }
}
