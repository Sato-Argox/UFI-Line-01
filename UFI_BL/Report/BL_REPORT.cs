﻿using UFI_PL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFI_DL;


namespace UFI_BL
{
    public class BL_REPORT
    {
        public DataTable BL_ExecuteTask(PL_REPORT objPl)
        {
            DL_REPORT objDl = new DL_REPORT();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
