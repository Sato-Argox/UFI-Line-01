using UFI_PL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFI_DL;


namespace UFI_BL
{
    public class BL_FG_LABEL_RE_PRINTING
    {
        public DataTable BL_ExecuteTask(PL_FG_LABEL_RE_PRINTING objPl)
        {
            DL_FG_LABEL_RE_PRINTING objDl = new DL_FG_LABEL_RE_PRINTING();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
