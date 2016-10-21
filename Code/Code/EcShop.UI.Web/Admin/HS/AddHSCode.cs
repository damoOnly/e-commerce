using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Entities.HS;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;
using EcShop.ControlPanel.Commodities;

namespace EcShop.UI.Web.Admin
{
    public class AddHSCode : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtHSCode;
        protected System.Web.UI.WebControls.TextBox txtHSCodeName;
        protected System.Web.UI.WebControls.TextBox txtLowRate;
        protected System.Web.UI.WebControls.TextBox txtHighRate;
        protected System.Web.UI.WebControls.TextBox txtOutRate;
        protected System.Web.UI.WebControls.TextBox txtTaxRate;
        protected System.Web.UI.WebControls.TextBox txtTslRate;
        protected System.Web.UI.WebControls.TextBox txtControlMa;
        protected System.Web.UI.WebControls.TextBox txtTempInRate;
        protected System.Web.UI.WebControls.TextBox txtTempOutRate;
        protected System.Web.UI.WebControls.TextBox txtNote;
        protected System.Web.UI.WebControls.TextBox txtControlInspection;
        protected System.Web.UI.WebControls.TextBox txtConsumptionRate;
        protected System.Web.UI.WebControls.TextBox elementsSearchText;
        protected System.Web.UI.WebControls.HiddenField elmentsJson;

        protected ElmentsListBox elmentsListBox;
        protected ProductCategoriesListBox listbox;

        protected UnitDropDownList dropUnit1;
        protected UnitDropDownList dropUnit2;

        protected System.Web.UI.WebControls.Button btnElementsSearchButton;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnAddHSCode;

        protected Pager pager;
        protected string searchkey;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.LoadParameters();

            this.btnElementsSearchButton.Click += new System.EventHandler(this.btnElementsSearchButton_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            if (!base.IsPostBack)
            {
                this.dropUnit1.DataBind();
                this.dropUnit2.DataBind();
                this.elmentsListBox.DataBind();
            }
            
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }

                this.elementsSearchText.Text = this.searchkey;
                return;
            }
            this.searchkey = this.elementsSearchText.Text.Trim();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            HSCodeInfo hscodeinfo = this.GetHSCode();
            if (HSCodeHelper.AddHSCodeInfo(hscodeinfo) > 0)
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/HS/HSCode.aspx"), true);
                return;
            }
            else
            {
                this.ShowMsg("添加海关编码失败,未知错误", false);
            }
        }

        private void btnElementsSearchButton_Click(object sender, EventArgs e)
        {
            this.elmentsListBox.HSElmentsName = this.searchkey;
            this.elmentsListBox.DataBind();
            DataSet ds = this.CreateTable();
            if (ds != null)
            {
                elmentsJson.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }

        private HSCodeInfo GetHSCode()
        {
            Hashtable hs=new Hashtable();

            HSCodeInfo hscodeinfo = new HSCodeInfo();
            hscodeinfo.HS_CODE = this.txtHSCode.Text.Trim();
            hscodeinfo.HS_NAME = this.txtHSCodeName.Text.Trim();

            hscodeinfo.UNIT_1 = this.dropUnit1.SelectedValue;
            hscodeinfo.UNIT_2 = this.dropUnit2.SelectedValue;
            hscodeinfo.CONTROL_MA = this.txtControlMa.Text.Trim();

            hscodeinfo.NOTE_S = this.txtNote.Text.Trim();
            hscodeinfo.CONTROL_INSPECTION = this.txtControlInspection.Text.Trim();

            hscodeinfo.LOW_RATE = decimal.Parse(Globals.HtmlEncode(this.txtLowRate.Text.Trim()));
            hscodeinfo.HIGH_RATE = decimal.Parse(Globals.HtmlEncode(this.txtHighRate.Text.Trim()));
            hscodeinfo.OUT_RATE = decimal.Parse(Globals.HtmlEncode(this.txtOutRate.Text.Trim()));
            hscodeinfo.TAX_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTaxRate.Text.Trim()));
            hscodeinfo.TSL_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTslRate.Text.Trim()));
            hscodeinfo.TEMP_IN_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTempInRate.Text.Trim()));
            hscodeinfo.TEMP_OUT_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTempOutRate.Text.Trim()));
            hscodeinfo.CONSUMPTION_RATE = decimal.Parse(Globals.HtmlEncode(this.txtConsumptionRate.Text.Trim()));

            DataSet ds = this.CreateTable();
            if (ds != null)
            {
                hscodeinfo.FND_HS_ELMENTS = ds;
            }

            return hscodeinfo;
        }

        private DataSet CreateTable()
        {
            string str = this.Page.Request["elmentsID"];
            DataSet ds = null;
            DataTable dt = null;

            if (!string.IsNullOrEmpty(str))
            {
                ds = new DataSet();
                dt = new DataTable();

                DataColumn dcID = new DataColumn("HS_ELMENTS_ID");
                DataColumn dcName = new DataColumn("HS_ELMENTS_NAME");
                DataColumn dcDesc = new DataColumn("HS_ELMENTS_DESC");

                dt.Columns.Add(dcID);
                dt.Columns.Add(dcName);
                dt.Columns.Add(dcDesc);

                string[] arrelmentsID = str.Split(',');

                foreach (string elmentsID in arrelmentsID)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = elmentsID;
                    dr[1] = this.Page.Request["elmentsName_" + elmentsID];
                    dr[2] = this.Page.Request["elmentsDesc_" + elmentsID];
                    dt.Rows.Add(dr);
                }

                ds.Tables.Add(dt);
            }
            return ds;
        }
    }
}
