using Common.Model;
using Common.Service;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MitraSolusiTelematika_Test
{
    public partial class _Default : Page
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Service_kode_pos service_Kode_Pos;

        public _Default()
        {
            service_Kode_Pos = new Service_kode_pos();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind(gv, "order by CREATED_DATE");
            }
        }
        private void Bind(GridView gv, string param)
        {
            try
            {
                service_Kode_Pos.LoadGv(gv, param);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + "," + ex.StackTrace);
                ClientScript.RegisterStartupScript(this.GetType(), "FailedMessage", utils.AlertPopUp(ex.Message + "," + ex.StackTrace), true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                divData.Visible = true;

                if (txtKodePos.Text == "" && txtKelurahan.Text == "" && txtKecamatan.Text == "" && txtJenis.Text == "" && txtKabupaten.Text == "" && txtPropinsi.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "FailedMessage", utils.AlertPopUp("Please fill."), true);
                }
                else
                {

                    service_Kode_Pos.Insert(Convert.ToInt32(txtKodePos.Text), txtKelurahan.Text, txtKecamatan.Text, txtJenis.Text, txtKabupaten.Text, txtPropinsi.Text, "", "");


                    Bind(gv, "order by CREATED_DATE desc");
                    clear();
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", utils.AlertPopUp("Your template bank version have been saved successfully."), true);
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message + "," + ex.StackTrace);
                ClientScript.RegisterStartupScript(this.GetType(), "FailedMessage", utils.AlertPopUp(ex.Message + "," + ex.StackTrace), true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Kode_Pos kode_Pos = null;
            try
            {
                if (txtKodePos.Text == "" && txtKelurahan.Text == "" && txtKecamatan.Text == "" && txtJenis.Text == "" && txtKabupaten.Text == "" && txtPropinsi.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "FailedMessage", utils.AlertPopUp("Please fill."), true);
                }
                else
                {
                    kode_Pos = service_Kode_Pos.Find("WHERE ID='" + txtiD.Text + "'");



                    kode_Pos.ID = Convert.ToInt32(txtiD.Text);
                    kode_Pos.NO_KODE_POS = Convert.ToInt32(txtKodePos.Text);
                    kode_Pos.KELURAHAN = txtKelurahan.Text;
                    kode_Pos.KECAMATAN = txtKecamatan.Text;
                    kode_Pos.JENIS = txtJenis.Text;
                    kode_Pos.KABUPATEN = txtKabupaten.Text;
                    kode_Pos.PROPINSI = txtPropinsi.Text;
                    kode_Pos.MODIFIED_BY = Request.QueryString["RESOURCE_NAME"];
                    kode_Pos.MODIFIED_DATE = DateTime.Now;

                    service_Kode_Pos.Update(Convert.ToInt32(txtiD.Text), kode_Pos);

                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", utils.AlertPopUp("Your template bank version have been updated successfully."), true);

                    clear();
                    Bind(gv, "order by a.MODIFIED_DATE desc");



                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + "," + ex.StackTrace);
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", utils.AlertPopUp(ex.Message + "," + ex.StackTrace), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {

            txtiD.Text = "";
            txtKodePos.Text = "";
            txtKelurahan.Text = "";
            txtKecamatan.Text = "";
            txtJenis.Text = "";
            txtKabupaten.Text = "";
            txtPropinsi.Text = "";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Kode_Pos kode_Pos = null;
            try
            {
                if (e.CommandName == "Edt")
                {
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;

                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gv.Rows[index];
                    TableCell Id = row.Cells[0];
                    TableCell kodePos = row.Cells[1];
                    TableCell kelurahan = row.Cells[2];
                    TableCell kecamatan  = row.Cells[3];
                    TableCell jenis = row.Cells[4];
                    TableCell kabupaten = row.Cells[5];
                    TableCell propinsi = row.Cells[6];
                    txtiD.Text = Id.Text;

                    txtKodePos.Text = kodePos.Text;
                    txtKelurahan.Text = kelurahan.Text;
                    txtKecamatan.Text = kecamatan.Text;
                    txtJenis.Text = jenis.Text;
                    txtKabupaten.Text = kabupaten.Text;
                    txtPropinsi.Text = propinsi.Text;
                }
                else if (e.CommandName == "Del")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gv.Rows[index];
                    TableCell ID = row.Cells[0];
                    txtiD.Text = ID.Text;

                    kode_Pos = service_Kode_Pos.Find("WHERE ID='" + txtiD.Text + "'");
                    service_Kode_Pos.Delete(Convert.ToInt32(txtiD.Text));

                    Bind(gv, "order by CREATED_DATE");
                }
              
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + "," + ex.StackTrace);
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", utils.AlertPopUp(ex.Message + "," + ex.StackTrace), true);
            }
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string item = e.Row.Cells[4].Text;
                    foreach (Button button in e.Row.Cells[7].Controls.OfType<Button>())
                    {
                        if (button.CommandName == "Del")
                        {
                            button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false;  } alert('Deleted " + item + " successfully');";
                        }
                    }
                }
                //check if the row is the header row
                if (e.Row.RowType == DataControlRowType.Pager)
                {
                    //add the thead and tbody section programatically
                    e.Row.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + "," + ex.StackTrace);
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", utils.AlertPopUp(ex.Message + "," + ex.StackTrace), true);
            }
        }
    }
}