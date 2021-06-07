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
    public partial class frmRejectedChildPart : Form
    {

        #region Variables

        private BL_REJECTION _blObj = null;
        private PL_REJECTION _plObj = null;
        private string _IsManual = "0";
        private DataTable dtMapping = null;
        #endregion

        #region Form Methods

        public frmRejectedChildPart()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_REJECTION();
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
                txtScanParentQrCode.Enabled = true;

                txtScanParentQrCode.Focus();


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

                txtScanParentQrCode.Text = "";

                txtScanParentQrCode.Enabled = true;

                txtScanParentQrCode.Focus();


            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }






        private bool ValidateInput()
        {
            try
            {
                if (txtScanParentQrCode.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Parent QR Code can't be blank!!", 3);
                    txtScanParentQrCode.Focus();
                    txtScanParentQrCode.SelectAll();
                    return false;
                }


                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        private bool CheckFGPart()
        {
            bool bChekFG = false;
            try
            {
                _plObj = new PL_REJECTION();
                _blObj = new BL_REJECTION();
                _plObj.DbType = "CHK_FG_PART";
                _plObj.Barcode = txtScanParentQrCode.Text.Trim();
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RESULT"].ToString() == "Y")
                    {
                        bChekFG = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
            return bChekFG;
        }

        #endregion

        #region Label Event

        #endregion

        #region DataGridView Events


        #endregion

        #region TextBox Event






        #endregion

        private void txtScanParentQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                if (e.KeyCode == Keys.Enter)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (string.IsNullOrEmpty(txtScanParentQrCode.Text.Trim()))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Scan Parent QR Code!!!", 3);
                        txtScanParentQrCode.Focus();
                        txtScanParentQrCode.Text = "";
                        return;
                    }
                    if (txtScanParentQrCode.Text.Trim().Length != 12)
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Parent QR Code!!!", 3);
                        txtScanParentQrCode.Focus();
                        txtScanParentQrCode.Text = "";
                        return;
                    }
                    string ParentChar = txtScanParentQrCode.Text.Trim().Substring(txtScanParentQrCode.Text.Trim().Length - 1);
                    if (!ParentChar.Equals("A") && !ParentChar.Equals("B"))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Parent QR Code!!!", 3);
                        txtScanParentQrCode.Focus();
                        txtScanParentQrCode.Text = "";
                        return;
                    }
                    if (CheckFGPart())
                    {
                        if (!GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Are You Sure Want To Reject??"))
                        {
                            return;
                        }

                    }
                    _plObj = new PL_REJECTION();
                    _blObj = new BL_REJECTION();
                    _plObj.DbType = "REJECTION";
                    _plObj.Barcode = txtScanParentQrCode.Text.Trim();
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {

                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {

                            lblScanMessage.Text = "";
                            lblScanMessage.Text = txtScanParentQrCode.Text.Trim();
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Rejected Successfully!!", 1);
                            txtScanParentQrCode.Text = "";
                            txtScanParentQrCode.Focus();
                        }
                        else
                        {
                            txtScanParentQrCode.Focus();
                            txtScanParentQrCode.Text = "";
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);

                        }
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        

    }
}
