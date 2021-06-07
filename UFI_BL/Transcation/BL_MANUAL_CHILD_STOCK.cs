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
    public class BL_MANUAL_CHILD_STOCK
    {
        public DataTable BL_ExecuteTask(PL_MANUAL_CHILD_STOCK objPl)
        {
            DL_MANUAL_CHILD_STOCK objDl = new DL_MANUAL_CHILD_STOCK();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
