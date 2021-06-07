using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UFI_BL;
using UFI_COMMON;
using UFI_PC_APP;
using UFI_PL;

namespace UFI_PC_APP
{
    public partial class RejectedChildPartReport : Form
    {

        #region Variables

        private BL_REJECTION_AND_REUSE_REPORT _blObj = null;
        private PL_REPORT _plObj = null;
        private string _RPT_Type = "";
        #endregion

        #region Form Methods

        public RejectedChildPartReport()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_REJECTION_AND_REUSE_REPORT();
              
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void frmModelMaster_Load(object sender, EventArgs e)
        {
            try
            {
                // this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, false, null, null);
                }
                rbRejectedReport_CheckedChanged(null, null);


            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }

        #endregion

        #region Button Event
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _plObj = new PL_REPORT();
                _blObj = new BL_REJECTION_AND_REUSE_REPORT();
                _plObj.DbType = _RPT_Type;
                _plObj.FromDate = dpFromDate.Value.ToString("yyyy-MM-dd");
                _plObj.ToDate = dpToDate.Value.ToString("yyyy-MM-dd");
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;

                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "No Data Found!!", 2);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv.Rows.Count == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "No Data Found!!", 2);
                    return;
                }
                GlobalVariable.ExportInCSV(dgv);

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, false, null, null);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Methods

        private void Clear()
        {
            try
            {
               
                dpFromDate.Text = dpToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                dgv.DataSource = null;

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }



        private bool ValidateInput()
        {
            try
            {

                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        private void BindGrid()
        {

        }

        #endregion

        #region Label Event

        #endregion

        #region DataGridView Events


        #endregion

        #region TextBox Event


        private void rbRejectedReport_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRejectedReport.Checked)
            {

                dgv.DataSource = null;
                _RPT_Type = "";
                _RPT_Type = "REJECTION_RPT";
            }
        }

        private void rbReleaseRPT_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReleaseRPT.Checked)
            {
                dgv.DataSource = null;
                _RPT_Type = "";
                _RPT_Type = "RELEASE_RPT";
            }
        }




        #endregion











    }
}
