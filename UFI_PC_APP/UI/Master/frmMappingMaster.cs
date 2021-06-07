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
    public partial class frmMappingMaster : Form
    {

        #region Variables

        private BL_MAPPING_MASTER _blObj = null;
        private PL_MAPPING_MASTER _plObj = null;
        private bool _IsUpdate = false;
        private string _stationNo = "";
        private DataTable dtMapping = null;
        #endregion

        #region Form Methods

        public frmMappingMaster()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_MAPPING_MASTER();
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

                btnDelete.Enabled = false;
                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
                SetColumns();
                GetStationNo();
                BindGrid();
                cbStationNo.Focus();
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    _plObj = new PL_MAPPING_MASTER();
                    _plObj.StationNo = cbStationNo.Text.Trim();
                    _plObj.ParentPartNo = cbParentPartNo.Text.Trim();
                    dtMapping = (DataTable)dgvDtl.DataSource;
                    _plObj.MAPPING_DETAILS = dtMapping;
                    _plObj.Active = chkActive.Checked;
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        _plObj.DbType = "INSERT";
                        DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                            {
                                btnReset_Click(sender, e);
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Saved successfully!!", 1);
                                frmModelMaster_Load(null, null);
                            }
                            else
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            }
                        }
                    }
                    else // if updating data
                    {
                        _plObj.DbType = "UPDATE";
                        DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                            {
                                btnReset_Click(sender, e);
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Updated successfully!!", 1);
                            }
                            else
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "UserId already exist!!", 3);
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();
                BindGrid();

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbStationNo.Text))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Revision Code can't be blank!!", 3);
                    return;
                }
                if (GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Äre you sure to delete the record !!"))
                {
                    _plObj = new PL_MAPPING_MASTER();
                    _blObj = new BL_MAPPING_MASTER();
                    _plObj.StationNo = cbStationNo.Text.Trim();
                    _plObj.DbType = "DELETE";
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0][0].ToString().StartsWith("Y"))
                        {
                            btnReset_Click(sender, e);
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Deleted successfully!!", 1);
                            frmModelMaster_Load(null, null);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("conflicted with the REFERENCE constraint"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "This is already in use!!!", 3);
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Methods
        private void GetStationNo()
        {
            try
            {
                Common common = new Common();
                DataTable dt = common.GetStaionNo();
                if (dt.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbStationNo, dt);
                    cbStationNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Sation No. data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private bool CheckDuplicate()
        {
            bool bCheckDup = false;
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "CHECK_DUPLICATE";
                _plObj.StationNo = cbStationNo.SelectedValue.ToString();
                _plObj.ParentPartNo = cbParentPartNo.SelectedValue.ToString();
                _plObj.ChildPartNo = cbChildPartNo.SelectedValue.ToString();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RESULT"].Equals("DUPLICATE RECORD!!!!"))
                    {
                        bCheckDup = true;
                    }
                    else
                    {
                        bCheckDup = false;
                    }


                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
            return bCheckDup;
        }
        private bool RemoveChildPart()
        {
            bool bCheckDup = false;
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "REMOVE_CHILD_PART";
                _plObj.StationNo = cbStationNo.SelectedValue.ToString();
                _plObj.ParentPartNo = cbParentPartNo.SelectedValue.ToString();
                _plObj.ChildPartNo = cbChildPartNo.SelectedValue.ToString();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                    {
                       
                    }
                    else
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                    }
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
            return bCheckDup;
        }
        private void GetParentPart()
        {
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "BIND_PARENT_PART";
                _plObj.StationNo = cbStationNo.SelectedValue.ToString();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbParentPartNo, dt);
                    cbParentPartNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Parent Part data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void GetChildPart()
        {
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "BIND_CHILD_PART";
                _plObj.StationNo = cbStationNo.SelectedValue.ToString();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbChildPartNo, dt);
                    cbChildPartNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Child Part data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void GetPartPackSize()
        {
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "BIND_PART_PACK_SIZE";
                _plObj.ChildPartNo = cbChildPartNo.SelectedValue.ToString();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    lblPackSize.Text = dt.Rows[0]["PACK_SIZE"].ToString();
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Child Part Pack Size data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void SetColumns()
        {
            dtMapping = new DataTable();
            dtMapping.Columns.Add("CHILD_PART_NO");
            dtMapping.Columns.Add("CHILD_PART_CONS_QTY");
            dtMapping.Columns.Add("PLC_VALUE_ID");


        }
        private void Clear()
        {
            try
            {
                cbStationNo.SelectedIndex = 0;
                if (cbParentPartNo.Items.Count > 0)
                {
                    cbParentPartNo.SelectedIndex = 0;
                }

                cbStationNo.Text = "";
                dtMapping.Rows.Clear();
                btnAdd.Enabled = true;
                dgvDtl.DataSource = dtMapping;
                cbStationNo.Enabled = cbParentPartNo.Enabled = true;
                btnDelete.Enabled = false;
                lblPackSize.Text = "0";
                txtPLCValueId.Text = "";
                txtEnterQty.Text = "";

                _IsUpdate = false;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }




        private void BindGrid()
        {
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "SELECT";
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
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
                if (cbStationNo.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Station No. can't be blank!!", 3);
                    cbStationNo.Focus();
                    cbStationNo.SelectAll();
                    return false;
                }
                if (cbParentPartNo.SelectedIndex<=0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Parent Part No. can't be blank!!", 3);
                    cbParentPartNo.Focus();
                    cbParentPartNo.SelectAll();
                    return false;
                }
                if (dgvDtl.Rows.Count == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Child Part Details can't be blank!!", 3);
                    return false;
                }

                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        private void BindGridDtl()
        {
            try
            {
                _plObj = new PL_MAPPING_MASTER();
                _blObj = new BL_MAPPING_MASTER();
                _plObj.DbType = "SELECT_DTL";
                _plObj.StationNo = cbStationNo.Text.Trim();
                _plObj.ParentPartNo = cbParentPartNo.SelectedValue.ToString().Trim();
                _plObj.MAPPING_DETAILS = dtMapping;
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                dgvDtl.DataSource = dt;
                dtMapping = dt;

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
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex <= -1)
                {
                    return;
                }
                Clear();
                cbStationNo.Text = dgv.Rows[e.RowIndex].Cells["Station_No"].Value.ToString();
                cbParentPartNo.Text = dgv.Rows[e.RowIndex].Cells["PARENT_PART_NO"].Value.ToString();
                chkActive.Checked = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["IS_ACTIVE"].Value);
                BindGridDtl();
                btnDelete.Enabled = true;
                cbStationNo.Enabled = cbParentPartNo.Enabled = false;
                _IsUpdate = true;
                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        #endregion

        #region TextBox Event

        private void txtStationNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbChildPartNo.SelectedIndex <= 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Child Part No. can't be blank!!", 3);
                    cbChildPartNo.SelectAll();
                    cbChildPartNo.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(lblPackSize.Text))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Pack Size can't be blank!!", 3);
                    txtEnterQty.SelectAll();
                    txtEnterQty.Focus();
                    return;
                }
                if (lblPackSize.Text.Equals("0"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Pack Size can't be Zero!!", 3);
                    txtEnterQty.SelectAll();
                    txtEnterQty.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtEnterQty.Text))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter Qty can't be blank!!", 3);
                    txtEnterQty.SelectAll();
                    txtEnterQty.Focus();
                    return;
                }
                if (txtEnterQty.Text.Equals("0"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter Qty can't be Zero!!", 3);
                    txtEnterQty.SelectAll();
                    txtEnterQty.Focus();
                    return;
                }
                if (Convert.ToInt32(txtEnterQty.Text.Trim()) > Convert.ToInt32(lblPackSize.Text.Trim()))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter Consume Qty can't be greater than Pack Size!!", 3);
                    txtEnterQty.SelectAll();
                    txtEnterQty.Focus();
                    return;

                }
                if (string.IsNullOrEmpty(txtPLCValueId.Text))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter PLC Value can't be blank!!", 3);
                    txtPLCValueId.SelectAll();
                    txtPLCValueId.Focus();
                    return;
                }
                if (dgvDtl.Rows.Count==6)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Child Part Can't be Added More than 6 !!", 3);
                    cbChildPartNo.Focus();
                    return;
                }
                bool iUpdate = false;
                for (int i = 0; i < dgvDtl.Rows.Count; i++)
                {
                    if (dgvDtl.Rows[i].Cells["CHILD_PART_NO"].Value.ToString() == cbChildPartNo.Text.Trim())
                    {
                        bool dialogResult = GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Are you sure want to update?");
                        if (dialogResult)
                        {
                            dtMapping.Rows[i]["CHILD_PART_CONS_QTY"] = Convert.ToString(Convert.ToInt32(dtMapping.Rows[i]["CHILD_PART_CONS_QTY"]) + Convert.ToInt32(txtEnterQty.Text.Trim()));
                            // dtMapping.Rows[i]["PLC_VALUE_ID"] = txtPLCValueId.Text.Trim().PadLeft(2, '0');
                            iUpdate = true;
                        }
                        else
                        {
                            return;
                        }



                    }
                }
                if (!iUpdate)
                {
                    if (CheckDuplicate()) { GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "DUPLICATE RECORD!!!!", 2); return; }
                    for (int i = 0; i < dgvDtl.Rows.Count; i++)
                    {
                        if (dgvDtl.Rows[i].Cells["PLC_VALUE_ID"].Value.ToString() == txtPLCValueId.Text.Trim().PadLeft(2, '0')|| dgvDtl.Rows[i].Cells["PLC_VALUE_ID"].Value.ToString() == txtPLCValueId.Text.Trim())
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "PLC Value can't be Duplicate!!", 3);
                            txtPLCValueId.SelectAll();
                            txtPLCValueId.Focus();
                            return;
                        }
                    }
                    DataRow dataRow = dtMapping.NewRow();
                    dataRow["CHILD_PART_NO"] = cbChildPartNo.SelectedValue;
                    dataRow["CHILD_PART_CONS_QTY"] = txtEnterQty.Text.Trim();
                    dataRow["PLC_VALUE_ID"] = txtPLCValueId.Text.Trim().PadLeft(2, '0');
                    dtMapping.Rows.Add(dataRow);
                }
                dgvDtl.DataSource = dtMapping;
                cbChildPartNo.SelectedIndex = 0;
                txtEnterQty.Text = "";
                txtPLCValueId.Text = "";
                lblPackSize.Text = "0";
            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbChildPartNo.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Select Child Part No.!!", 3);
                    cbChildPartNo.Focus();
                    return;
                }
                dtMapping.Rows.RemoveAt(dgvDtl.CurrentRow.Index);
                //cbChildPartNo.SelectedIndex = 0;
                //txtEnterQty.Text = "";
                RemoveChildPart();
                btnRemove.Enabled = false;
            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void cbStationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStationNo.SelectedIndex > 0)
            {
                cbParentPartNo.DataSource = null;
                cbChildPartNo.DataSource = null;
                GetParentPart();
                GetChildPart();
            }
        }



        private void txtEnterQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }

        private void dgvDtl_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex <= -1)
                {
                    return;
                }
                cbChildPartNo.Text = dgvDtl.Rows[e.RowIndex].Cells["CHILD_PART_NO"].Value.ToString();
                txtEnterQty.Text = dgvDtl.Rows[e.RowIndex].Cells["CHILD_PART_CONS_QTY"].Value.ToString();
                txtPLCValueId.Text = dgvDtl.Rows[e.RowIndex].Cells["PLC_VALUE_ID"].Value.ToString();
                btnRemove.Enabled = true;
                //btnAdd.Enabled = false;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void txtPLCValueId_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }




        #endregion

        private void cbChildPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChildPartNo.SelectedIndex > 0)
            {
                lblPackSize.Text = "0";
                GetPartPackSize();
            }
        }
    }
}
