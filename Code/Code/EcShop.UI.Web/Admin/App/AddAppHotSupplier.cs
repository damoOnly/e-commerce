using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.UI.Common.Controls;
using EcShop.Entities.Supplier;
using Entities;
namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.AppAdImageAdd)]
    public class AddAppHotSupplier : AdminPage
    {
        private int id;
        protected System.Web.UI.WebControls.TextBox txtDesc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
        protected System.Web.UI.WebControls.DropDownList ddlSupplier;
        protected System.Web.UI.WebControls.Button btnAddSupplier;
        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
        protected System.Web.UI.HtmlControls.HtmlImage littlepic;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (int.TryParse(base.Request.QueryString["Id"], out this.id))
            {
                if (!this.Page.IsPostBack)
                {
                    BindSupplier();

                    if (id != 0)
                    {
                        SetSupplierCfg(id);
                    }
                    return;
                }
            }
            else
            {
                base.Response.Redirect("ManageAppHotSupplier.aspx");
            }
        }
        protected void btnAddSupplier_Click(object sender, System.EventArgs e)
        {
            if (this.id == 0)
            {
                SupplierConfigInfo info = new SupplierConfigInfo();
                info.IsDisable = false;
                info.ImageUrl = this.fmSrc.Value;
                info.ShortDesc = this.txtDesc.Text;
                info.Client = (int)ClientType.App;
                info.Type = (int)SupplierCfgType.Hot;
                info.SupplierId = int.Parse(this.ddlSupplier.SelectedValue);
                if (string.IsNullOrWhiteSpace(info.ImageUrl))
                {
                    this.ShowMsg("请上传图片！", false);
                    return;
                }
                if (SupplierConfigHelper.SaveSupplierCfg(info) > 0)
                {
                    this.CloseWindow();
                    return;
                }
                this.ShowMsg("添加错误！", false);
            }

            else
            {
                SupplierConfigInfo info = SupplierConfigHelper.GetSupplierCfgById(this.id);
                info.IsDisable = false;
                info.ImageUrl = this.fmSrc.Value;
                info.ShortDesc = this.txtDesc.Text;
                info.SupplierId = int.Parse(this.ddlSupplier.SelectedValue);
                if (SupplierConfigHelper.UpdateSupplierCfg(info))
                {
                    this.CloseWindow();
                    return;
                }
                this.ShowMsg("修改失败！", false);
            }
        }

        private void BindSupplier()
        {
            this.ddlSupplier.DataSource = SupplierHelper.GetSupplier();
            this.ddlSupplier.DataTextField = "SupplierName";
            this.ddlSupplier.DataValueField = "SupplierId";
            this.ddlSupplier.DataBind();
        }

        private void SetSupplierCfg(int id)
        {
            SupplierConfigInfo info = SupplierConfigHelper.GetSupplierCfgById(this.id);
            this.ddlSupplier.SelectedValue = info.SupplierId.ToString();

            this.txtDesc.Text = info.ShortDesc;

            this.littlepic.Src = info.ImageUrl;
            this.fmSrc.Value = info.ImageUrl;
        }







    }
}

