﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFI_COMMON;
using UFI_PL;

using SatoLib;


namespace UFI_DL
{
    public class DL_MAPPING_MASTER
    {

        
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_MAPPING_MASTER obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@STATION_NO", SqlDbType.VarChar, 50);
                param[1].Value = obj.StationNo;
                param[2] = new SqlParameter("@PARENT_PART_NO", SqlDbType.VarChar, 50);
                param[2].Value = obj.ParentPartNo;
                param[3] = new SqlParameter("@CHILD_PART_NO", SqlDbType.VarChar, 50);
                param[3].Value = obj.ChildPartNo;
                param[4] = new SqlParameter("@IS_ACTIVE", SqlDbType.VarChar, 50);
                param[4].Value = obj.Active;
                param[5] = new SqlParameter("@MAPPING_DETAILS", SqlDbType.Structured, 50);
                param[5].Value = obj.MAPPING_DETAILS;
                param[6] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[6].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_MAPPING_MASTER]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion      
    }
}
