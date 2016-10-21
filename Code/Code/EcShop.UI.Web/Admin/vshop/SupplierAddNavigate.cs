using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.vshop
{
     public class SupplierAddNavigate : AdminPage
    {
        protected string tplpath = HiContext.Current.GetVshopSkinPath(null) + "/images/deskicon/";
        protected System.Web.UI.WebControls.TextBox txtNavigateDesc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
        protected System.Web.UI.HtmlControls.HtmlImage littlepic;
        protected System.Web.UI.WebControls.Repeater FontIcon;
        protected System.Web.UI.WebControls.DropDownList ddlType;
        protected System.Web.UI.WebControls.DropDownList ddlSubType;
        protected System.Web.UI.WebControls.DropDownList ddlThridType;
        protected System.Web.UI.WebControls.TextBox Tburl;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navigateDesc;
        protected System.Web.UI.WebControls.Button btnAddBanner;
        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
        protected System.Web.UI.HtmlControls.HtmlInputHidden locationUrl;
        protected ProductCategoriesDropDownList dropCategories;
        protected ImportSourceTypeDropDownList dropImportSourceType;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ddlType.BindEnum<EcShop.Entities.VShop.LocationType>("VipCard");//修改1
                this.BindIcons();

                dropCategories.DataBind();
                dropImportSourceType.DataBind();
            }
        }
        protected void btnAddBanner_Click(object sender, System.EventArgs e)
        {
            TplCfgInfo tplCfgInfo = new NavigateInfo();
            tplCfgInfo.IsDisable = false;
            tplCfgInfo.ImageUrl = this.fmSrc.Value;
            tplCfgInfo.ShortDesc = this.txtNavigateDesc.Text;
            tplCfgInfo.LocationType = (LocationType)System.Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue);
            tplCfgInfo.Url = this.locationUrl.Value;
            tplCfgInfo.Client = 1;
            tplCfgInfo.SupplierId = CheckSupplierRole();
            if (string.IsNullOrWhiteSpace(this.locationUrl.Value))
            {
                this.ShowMsg("跳转页不能为空！", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(tplCfgInfo.ImageUrl))
            {
                this.ShowMsg("请选择图标或上传图片！", false);
                return;
            }
            if (VShopHelper.SaveTplCfg(tplCfgInfo))
            {
                this.CloseWindow();
                return;
            }
            this.ShowMsg("添加错误！", false);
        }
        private void Reset()
        {
            this.txtNavigateDesc.Text = string.Empty;
            this.Tburl.Text = string.Empty;
            this.ddlType.SelectedValue = LocationType.Link.ToString();
        }
        protected void BindIcons()
        {
            string path = base.Server.MapPath("/Utility/icomoon") + "/icomoon.font";
            string text;
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path))
            {
                text = streamReader.ReadToEnd();
            }
            this.FontIcon.DataSource = text.Split(new char[]
			{
				','
			});
            this.FontIcon.DataBind();
        }
    }
}
