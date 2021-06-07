using UFI_COMMON;
using UFI_PL;
using SatoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFI_DL
{

    public class DL_FG_LABEL_PRINTING
    {
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_FG_LABEL_PRINTING obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@PARENT_BARCODE", SqlDbType.VarChar, 100);
                param[1].Value = obj.Parent_Barcode;
                param[2] = new SqlParameter("@FG_BARCODE", SqlDbType.VarChar, 100);
                param[2].Value = obj.FG_Barcode;
                param[3] = new SqlParameter("@IS_MANUAL", SqlDbType.VarChar, 50);
                param[3].Value = obj.Is_Manual;
                param[4] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[4].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_FG_LABEL_PRINTING]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion      

    }
}
