using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.App
{
      [PrivilegeCheck(Privilege.AppAdImageAdd)]
    public class AddAppIcon : AdminPage
    {
        private int id;
        protected System.Web.UI.WebControls.TextBox txtBannerDesc;
        protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
        protected System.Web.UI.WebControls.Button btnAddBanner;
        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
        protected System.Web.UI.HtmlControls.HtmlImage littlepic;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (int.TryParse(base.Request.QueryString["Id"], out this.id))
            {
                if (!this.Page.IsPostBack)
                {

                    if (id != 0)
                    {
                        this.Restore();
                    }
                    return;
                }
            }
            else
            {
                base.Response.Redirect("ManageAppIcon.aspx");
            }
        }
        protected void btnAddBanner_Click(object sender, System.EventArgs e)
        {
            if (this.id == 0)
            {
                TplCfgInfo tplCfgInfo = new IconInfo();
                tplCfgInfo.IsDisable = false;
                tplCfgInfo.ImageUrl = this.fmSrc.Value;
                tplCfgInfo.ShortDesc = this.txtBannerDesc.Text;
                tplCfgInfo.Client = 3;
                tplCfgInfo.LocationType = LocationType.Link;
                tplCfgInfo.Url = "";
                if (string.IsNullOrWhiteSpace(tplCfgInfo.ImageUrl))
                {
                    this.ShowMsg("请上传图片！", false);
                    return;
                }
                if (VShopHelper.SaveTplCfg(tplCfgInfo))
                {
                    this.CloseWindow();
                    return;
                }
                this.ShowMsg("添加错误！", false);
            }

            else
            {
                TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
                tplCfgById.IsDisable = false;
                tplCfgById.ImageUrl = this.fmSrc.Value;
                tplCfgById.ShortDesc = this.txtBannerDesc.Text;
                tplCfgById.Client = 3;
                tplCfgById.LocationType = LocationType.Link;
                tplCfgById.Url = "";
                if (VShopHelper.UpdateTplCfg(tplCfgById))
                {
                    this.CloseWindow();
                    return;
                }
                this.ShowMsg("修改失败！", false);
            }


        }


        protected void Restore()
        {
            TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
            this.txtBannerDesc.Text = tplCfgById.ShortDesc;
            this.littlepic.Src = tplCfgById.ImageUrl;
            this.fmSrc.Value = tplCfgById.ImageUrl;
        }
    }
}
