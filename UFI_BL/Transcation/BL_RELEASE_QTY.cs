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
    public class BL_RELEASE_QTY
    {
        public DataTable BL_ExecuteTask(PL_RELEASE_QTY objPl)
        {
            DL_RELEASE_QTY objDl = new DL_RELEASE_QTY();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
