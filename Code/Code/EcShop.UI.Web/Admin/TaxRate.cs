using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.TaxRateView)]
    public class TaxRate : AdminPage
	{
		protected System.Web.UI.WebControls.Repeater rp_prducttag;
		protected System.Web.UI.WebControls.TextBox txttagname;
		protected System.Web.UI.WebControls.TextBox txtaddtagname;
        protected System.Web.UI.WebControls.TextBox txtcode;
        protected System.Web.UI.WebControls.TextBox txtaddcode;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtagId;
		protected System.Web.UI.WebControls.Button btnaddtag;
		protected System.Web.UI.WebControls.Button btnupdatetag;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.TextBox txtAddCodeDescription;
        protected System.Web.UI.WebControls.TextBox txtCodeDescription;
        
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{   
            //*****************************************************************
            //不知有啥用，等新功能开发完成请教
            //if (!string.IsNullOrEmpty(base.Request["isAjax"]) && base.Request["isAjax"] == "true")
            //{
            //    string text = base.Request["Mode"].ToString();
            //    string text2 = "false";
            //    string a;
            //    if ((a = text) != null && a == "Add")
            //    {
            //        ManagerHelper.CheckPrivilege(Privilege.TaxRateAdd);
            //        string text3 = "不允许为空";
            //        if (!string.IsNullOrEmpty(base.Request["TagValue"].Trim()))
            //        {
            //            text3 = "添加税率失败，请确认税率是否已存在";
            //            decimal tagName = decimal.Parse(Globals.HtmlEncode(base.Request["TagValue"].ToString()));
            //            string tagCode = Globals.HtmlEncode(base.Request["TagValue"].ToString());
            //            int num = CatalogHelper.AddTaxRate(tagName, tagCode);
            //            if (num > 0)
            //            {
            //                text2 = "true";
            //                text3 = num.ToString();
            //            }
            //        }
            //        base.Response.Clear();
            //        base.Response.ContentType = "application/json";
            //        base.Response.Write(string.Concat(new string[]
            //        {
            //            "{\"Status\":\"",
            //            text2,
            //            "\",\"msg\":\"",
            //            text3,
            //            "\"}"
            //        }));
            //        base.Response.End();
            //    }
            //}
            //***************************************************************
			this.btnaddtag.Click += new System.EventHandler(this.btnaddtag_Click);
			this.btnupdatetag.Click += new System.EventHandler(this.btnupdatetag_Click);
			this.rp_prducttag.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rp_prducttag_ItemCommand);
			if (!base.IsPostBack)
			{
                this.TaxRateBind();
			}
		}
        protected void TaxRateBind()
		{
			this.rp_prducttag.DataSource = CatalogHelper.GetTaxRate();
			this.rp_prducttag.DataBind();
		}
		protected void btnaddtag_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.TaxRateAdd);
			string text = Globals.HtmlEncode(this.txtaddtagname.Text.Trim());
            string code = Globals.HtmlEncode(this.txtaddcode.Text.Trim());
            string CodeDescription = Globals.HtmlEncode(this.txtAddCodeDescription.Text.Trim());
			if (string.IsNullOrEmpty(text))
			{
                this.ShowMsg("税率不允许为空！", false);
				return;
			}
            if (string.IsNullOrEmpty(code))
            {
                this.ShowMsg("行邮编码不允许为空", false);
                return;
            }
            decimal taxRate = decimal.Parse(text);
            if (CatalogHelper.AddTaxRate(taxRate, code, CodeDescription) > 0)
            {
                this.ShowMsg("添加成功！", true);
                this.TaxRateBind();
                return;
            }
            else
            {
                this.ShowMsg("添加失败，请确认是否存在相同数值", false);
            }

            

		}
		protected void btnupdatetag_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.TaxRateEdit);
			string value = this.hdtagId.Value.Trim();
			string text = Globals.HtmlEncode(this.txttagname.Text.Trim());
            string code = Globals.HtmlEncode(this.txtcode.Text.Trim());
            string CodeDescription = Globals.HtmlEncode(this.txtCodeDescription.Text.Trim());

			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text))
			{
                this.ShowMsg("请选择要修改的税率或输入税率", false);
				return;
			}
            if (string.IsNullOrEmpty(code))
            {
                this.ShowMsg("行邮编码不允许为空", false);
                return;
            }
			if (System.Convert.ToInt32(value) <= 0)
			{
                this.ShowMsg("选择的税率有误", false);
				return;
			}
            decimal taxRate = decimal.Parse(text);
            if (CatalogHelper.UpdateTaxRate(System.Convert.ToInt32(value), taxRate, code,CodeDescription))
			{
                this.ShowMsg("修改成功", true);
                this.TaxRateBind();
				return;
			}
            this.ShowMsg("添加失败，请确认是否存在相同数值", false);
		}
		protected void rp_prducttag_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.TaxRateDelete);
			if (e.CommandName.Equals("delete"))
			{
				string value = e.CommandArgument.ToString();
				if (!string.IsNullOrEmpty(value) && System.Convert.ToInt32(value) > 0)
				{
					if (CatalogHelper.DeleteTaxRate(System.Convert.ToInt32(value)))
					{
                        this.ShowMsg("删除成功", true);
                        this.TaxRateBind();
						return;
					}
                    this.ShowMsg("删除失败", false);
				}
			}
		}

        protected void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            string taxRate = this.txtSearchText.Text.Trim();
            this.rp_prducttag.DataSource = CatalogHelper.GetTaxRate(taxRate);
            this.rp_prducttag.DataBind();
        }
	}
}
