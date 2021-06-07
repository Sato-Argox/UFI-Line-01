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

    public class DL_RELEASE_QTY
    {
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_RELEASE_QTY obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@STATION", SqlDbType.VarChar, 100);
                param[1].Value = obj.Station;
                param[2] = new SqlParameter("@MODEL", SqlDbType.VarChar, 100);
                param[2].Value = obj.Model;
                param[3] = new SqlParameter("@CHILD_PART", SqlDbType.VarChar, 100);
                param[3].Value = obj.Child_Part_No;
                param[4] = new SqlParameter("@QTY", SqlDbType.VarChar, 100);
                param[4].Value = obj.Qty;
                param[5] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 100);
                param[5].Value = obj.CreatedBy;
            
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_RELEASE_PART_QTY]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion      

    }
}
