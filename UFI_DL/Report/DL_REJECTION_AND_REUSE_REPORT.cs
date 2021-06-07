﻿using UFI_COMMON;
using UFI_PL;
using SatoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFI_PL
{
    public class DL_REJECTION_AND_REUSE_REPORT
    {
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_REPORT obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@FROM_DATE", SqlDbType.VarChar, 100);
                param[1].Value = obj.FromDate;
                param[2] = new SqlParameter("@TO_DATE", SqlDbType.VarChar, 100);
                param[2].Value = obj.ToDate;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_REJECTION_REUSE_RPT]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  
    }
}
