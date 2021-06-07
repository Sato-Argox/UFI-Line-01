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
    public partial class frmReleaseBOPQty : Form
    {

        #region Variables

        private BL_RELEASE_QTY _blObj = null;
        private PL_RELEASE_QTY _plObj = null;
        #endregion

        #region Form Methods

        public frmReleaseBOPQty()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_RELEASE_QTY();

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


                txtSearch.Enabled = true;

                txtSearch.Focus();
                GetStationNo();
                BindGrid();

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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
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

                txtSearch.Text = "";

                txtSearch.Enabled = true;
                if (cbStationNo.SelectedIndex > 0)
                {
                    cbStationNo.SelectedIndex = 0;
                }
                txtSearch.Focus();
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
                if (txtSearch.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Parent QR Code can't be blank!!", 3);
                    txtSearch.Focus();
                    txtSearch.SelectAll();
                    return false;
                }


                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        private void GetStationNo()
        {
            try
            {

                _plObj = new PL_RELEASE_QTY();
                _blObj = new BL_RELEASE_QTY();
                _plObj.DbType = "BIND_STATION";
                _plObj.Station = cbStationNo.Text.Trim();
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
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
        private void BindGrid()
        {
            try
            {

                _plObj = new PL_RELEASE_QTY();
                _blObj = new BL_RELEASE_QTY();
                _plObj.DbType = "BIND_GRID";
                _plObj.Station = cbStationNo.Text.Trim();
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
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Enter_Qty_KeyPress);
            if (dgv.CurrentCell.ColumnIndex == 0) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                    {
                        dgv.Rows[i].Cells["Enter_Qty"].Value = "";
                    }
                    tb.KeyPress += new KeyPressEventHandler(Enter_Qty_KeyPress);
                }
            }

        }
        private void Enter_Qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allowed only numeric value  ex.10
            //if (!char.IsControl(e.KeyChar)
            //    && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}

            // allowed numeric and one dot  ex. 10.23
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                 )
            {
                e.Handled = true;
            }

            // only allow one decimal point
            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = false;
            //}
        }
      

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (string.IsNullOrEmpty(dgv.Rows[e.RowIndex].Cells["Enter_Qty"].Value.ToString()) ||
                    dgv.Rows[e.RowIndex].Cells["Enter_Qty"].Value.ToString() == "0")
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter Qty Can't be Blank Or Zero!!!", 2);
                    return;
                }
                if (Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Enter_Qty"].Value.ToString())>Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Avail_Qty"].Value.ToString()))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Enter Qty Can't be greater than Available Qty!!!", 2);
                    return;
                }
                if (!GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Are you sure want to realse the Qty.??"))
                {
                    return;
                }
                //TODO - Button Clicked - Execute Code Here
                _plObj = new PL_RELEASE_QTY();
                _blObj = new BL_RELEASE_QTY();
                _plObj.DbType = "RELEASE_UPDATE";
                _plObj.Station = dgv.Rows[e.RowIndex].Cells["Station_No"].Value.ToString();
                _plObj.Model = dgv.Rows[e.RowIndex].Cells["Model"].Value.ToString();
                _plObj.Child_Part_No = dgv.Rows[e.RowIndex].Cells["Child_Part_No"].Value.ToString();
                _plObj.Qty =Convert.ToInt32( dgv.Rows[e.RowIndex].Cells["Enter_Qty"].Value.ToString());
                _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                    {
                        btnReset_Click(sender, e);
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Qty Release Successfully!!", 1);
                        BindGrid();


                    }
                    else
                    {

                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                        BindGrid();

                    }
                }
            }
        }





        #endregion

        #region TextBox Event


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgv.DataSource as DataTable).DefaultView.RowFilter = string.Format("Child_Part_No LIKE '%{0}%'", txtSearch.Text);
        }



        #endregion


    }
}
