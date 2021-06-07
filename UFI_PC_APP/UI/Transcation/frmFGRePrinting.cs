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
    public partial class frmFGRePrinting : Form
    {

        #region Variables

        private BL_FG_LABEL_RE_PRINTING _blObj = null;
        private PL_FG_LABEL_RE_PRINTING _plObj = null;
        private bool _IsUpdate = false;
        private string _stationNo = "";
        private DataTable dtMapping = null;
        #endregion

        #region Form Methods

        public frmFGRePrinting()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_FG_LABEL_RE_PRINTING();
                dtMapping = new DataTable();
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
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;

                
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }

        #endregion

        #region Button Event
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
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, null, null);
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
                txtScanBarcode.Text = "";
                _IsUpdate = false;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }






        #endregion

        #region Label Event

        #endregion

        #region DataGridView Events


        #endregion

        #region TextBox Event

        private void txtScanBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrEmpty(txtScanBarcode.Text.Trim()))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Scan Barcode Code!!!", 3);
                        return;
                    }
                    

                    _plObj = new PL_FG_LABEL_RE_PRINTING();
                    _blObj = new BL_FG_LABEL_RE_PRINTING();
                    _plObj.DbType = "RE_PRINT";
                    _plObj.Scan_Barcode = txtScanBarcode.Text.Trim();
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {
                            Common common = new Common();
                            common.LablePrint(dataTable.Rows[0]["FG_BARCODE"].ToString());
                            btnReset_Click(sender, e);
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Re-Print Successfully!!", 1);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            txtScanBarcode.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }

        }



        #endregion


    }
}
