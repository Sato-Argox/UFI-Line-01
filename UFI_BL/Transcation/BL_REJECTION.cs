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
    public class BL_REJECTION
    {
        public DataTable BL_ExecuteTask(PL_REJECTION objPl)
        {
            DL_REJECTION objDl = new DL_REJECTION();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
