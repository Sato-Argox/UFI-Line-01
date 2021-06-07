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
    public partial class frmFGPrinting : Form
    {

        #region Variables

        private BL_FG_LABEL_PRINTING _blObj = null;
        private PL_FG_LABEL_PRINTING _plObj = null;
        private string _IsManual = "0";
        private DataTable dtMapping = null;
        #endregion

        #region Form Methods

        public frmFGPrinting()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_FG_LABEL_PRINTING();
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
                this.WindowState = FormWindowState.Maximized;

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, false, null, null);
                }
                cbModelNo.SelectedIndex = 0;
                cbModelNo.Focus();


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
                txtScanFGBarcode.Text = "";
                txtScanParentQrCode.Text = "";
                chkIsManual.Checked = false;
                txtScanParentQrCode.Enabled = true;
                txtScanFGBarcode.Enabled = false;
                txtScanParentQrCode.Focus();
                for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                {
                    dgv.Rows.RemoveAt(i);
                }

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
        private void BindGrid()
        {
            try
            {
                if (txtScanParentQrCode.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Parent QR Code can't be blank!!", 3);
                    txtScanParentQrCode.Focus();
                    txtScanParentQrCode.SelectAll();
                    return;
                }
                _plObj = new PL_FG_LABEL_PRINTING();
                _blObj = new BL_FG_LABEL_PRINTING();
                _plObj.DbType = "BIND_GRID";
                _plObj.Parent_Barcode = txtScanParentQrCode.Text.Trim();
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                dgv.DataSource = dt;

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        #endregion

        #region Label Event

        #endregion

        #region DataGridView Events


        #endregion

        #region TextBox Event

        private void txtScanParentQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                if (e.KeyCode == Keys.Enter)
                {
                    lblVerifedMsg.Text = "";
                    if (cbModelNo.SelectedIndex <= 0)
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Select Model!!", 2);
                        cbModelNo.Focus();
                        return;

                    }
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
                    if (!cbModelNo.Text.Trim().Equals(ParentChar=="A"?"D22":"G20"))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Parent QR Code against selected Model!!!", 3);
                        txtScanParentQrCode.Focus();
                        txtScanParentQrCode.Text = "";
                        return;
                    }
                    chkIsManual_CheckedChanged(null, null);
                    _plObj = new PL_FG_LABEL_PRINTING();
                    _blObj = new BL_FG_LABEL_PRINTING();
                    _plObj.DbType = "SCAN_PARENT_BARCODE";
                    _plObj.Parent_Barcode = txtScanParentQrCode.Text.Trim();
                    _plObj.FG_Barcode = txtScanFGBarcode.Text.Trim();
                    _plObj.Is_Manual = _IsManual;
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {
                            Common common = new Common();
                            common.LablePrint(dataTable.Rows[0]["FG_BARCODE"].ToString().ToUpper());

                            //btnReset_Click(sender, e);
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Print Successfully!!", 1);
                            //frmModelMaster_Load(null, null);

                            BindGrid();
                            txtScanParentQrCode.Enabled = false;
                            txtScanFGBarcode.Enabled = true;
                            txtScanFGBarcode.Focus();
                        }
                        else
                        {
                            if (dataTable.Rows[0][0].ToString().Contains("IS NOT VERIFIED"))
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                                txtScanParentQrCode.Text = "";
                                txtScanParentQrCode.Focus();
                                return;
                            }
                            else
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                                BindGrid();
                                txtScanParentQrCode.Enabled = false;
                                txtScanFGBarcode.Enabled = true;
                                txtScanFGBarcode.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void chkIsManual_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsManual.Checked)
            {
                _IsManual = "1";

            }
            else
            {
                _IsManual = "0";
            }
        }

        private void txtScanFGBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {


                if (e.KeyCode == Keys.Enter)
                {
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
                    if (string.IsNullOrEmpty(txtScanFGBarcode.Text.Trim()))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Scan FG Barcode!!!", 3);
                        txtScanFGBarcode.Focus();
                        txtScanFGBarcode.Text = "";
                        return;
                    }
                    if (char.IsLower(txtScanFGBarcode.Text.Trim(), 8))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Scan FG Barcode is in Lower Case!!!", 3);
                        txtScanFGBarcode.Focus();
                        txtScanFGBarcode.Text = "";
                        return;
                    }
                    _plObj = new PL_FG_LABEL_PRINTING();
                    _blObj = new BL_FG_LABEL_PRINTING();
                    _plObj.DbType = "SCAN_VERIFY_FG_BARCODE";
                    _plObj.Parent_Barcode = txtScanParentQrCode.Text.Trim().ToUpper();
                    _plObj.FG_Barcode = txtScanFGBarcode.Text.Trim().ToUpper();
                    _plObj.Is_Manual = _IsManual;
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {
                            lblVerifedMsg.Text = "Verified Successfully!! (" + txtScanFGBarcode.Text.Trim() + ")";///Added  by dipak 03_sep-20
                            btnReset_Click(sender, e);
                            //GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Verified Successfully!!", 1);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            txtScanFGBarcode.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void chkIsManual_Click(object sender, EventArgs e)
        {
            if (_IsManual == "0")
            {
                frmAccessPassword frmObj = new frmAccessPassword();
                frmObj.ShowDialog();
                if (frmObj.IsCancel == true)
                {
                    _IsManual = "1";
                    chkIsManual.Checked = true;
                }
                else
                {
                    _IsManual = "0";
                    chkIsManual.Checked = false;

                }


            }
            else
            {
                chkIsManual.Checked = false;
                _IsManual = "0";

            }
            txtScanParentQrCode.Focus();
        }

        private void cbModelNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbModelNo.SelectedIndex > 0)
            {
                btnReset_Click(null, null);
                txtScanParentQrCode.Enabled = true;
                txtScanFGBarcode.Enabled = false;
                txtScanParentQrCode.Focus();
            }
        }




        #endregion


    }
}
