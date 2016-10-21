using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.vshop
{
    public class SupplierAddBanner : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtBannerDesc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
        protected System.Web.UI.WebControls.DropDownList ddlType;
        protected System.Web.UI.WebControls.DropDownList ddlSubType;
        protected System.Web.UI.WebControls.DropDownList ddlThridType;
        protected System.Web.UI.WebControls.TextBox Tburl;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navigateDesc;
        protected System.Web.UI.WebControls.Button btnAddBanner;
        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
        protected System.Web.UI.HtmlControls.HtmlInputHidden locationUrl;

        private int supplierId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //根据登录用户获取供应商id
            supplierId = CheckSupplierRole();
            if (!this.Page.IsPostBack)
            {
                this.ddlType.BindEnum<EcShop.Entities.VShop.LocationType>("VipCard");//修改1
            }
        }
        protected void btnAddBanner_Click(object sender, System.EventArgs e)
        {
            
            TplCfgInfo tplCfgInfo = new BannerInfo();
            tplCfgInfo.IsDisable = false;
            tplCfgInfo.ImageUrl = this.fmSrc.Value;
            tplCfgInfo.ShortDesc = this.txtBannerDesc.Text;
            tplCfgInfo.LocationType = (LocationType)System.Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue);
            tplCfgInfo.Client = 1;
            tplCfgInfo.SupplierId = supplierId;
            if (string.IsNullOrWhiteSpace(tplCfgInfo.ImageUrl))
            {
                this.ShowMsg("请上传轮播图！", false);
                return;
            }
            tplCfgInfo.Url = this.locationUrl.Value;
            if (VShopHelper.SaveTplCfg(tplCfgInfo))
            {
                this.CloseWindow();
                return;
            }
            this.ShowMsg("添加错误！", false);
        }
        private void Reset()
        {
            this.txtBannerDesc.Text = string.Empty;
            this.Tburl.Text = string.Empty;
            this.ddlType.SelectedValue = LocationType.Link.ToString();
        }
    }
}
