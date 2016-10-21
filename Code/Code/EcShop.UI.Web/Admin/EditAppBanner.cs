using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppAdImageEdit)]
	public class EditAppBanner : AdminPage
	{
		private int id;
		protected System.Web.UI.WebControls.TextBox txtBannerDesc;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
		protected System.Web.UI.HtmlControls.HtmlImage littlepic;
		protected System.Web.UI.WebControls.DropDownList ddlType;
		protected System.Web.UI.WebControls.DropDownList ddlSubType;
		protected System.Web.UI.WebControls.DropDownList ddlThridType;
		protected System.Web.UI.WebControls.TextBox Tburl;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navigateDesc;
		protected System.Web.UI.WebControls.Button btnEditBanner;
		protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc;
		protected System.Web.UI.HtmlControls.HtmlInputHidden locationUrl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["Id"], out this.id))
			{
				if (!this.Page.IsPostBack)
				{
                    this.ddlType.BindEnum<EcShop.Entities.AppLocationType>("Topic");//修改1
					this.Restore();
					return;
				}
			}
			else
			{
				base.Response.Redirect("ManageBanner.aspx");
			}
		}
		protected void Restore()
		{
			TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
			this.txtBannerDesc.Text = tplCfgById.ShortDesc;
			this.ddlType.SelectedValue = tplCfgById.LocationType.ToString();
			this.littlepic.Src = tplCfgById.ImageUrl;
			this.fmSrc.Value = tplCfgById.ImageUrl;
			switch (tplCfgById.LocationType)
			{
			case LocationType.Topic:
			{
				this.ddlSubType.Attributes.CssStyle.Remove("display");
				System.Collections.Generic.IList<TopicInfo> list = VShopHelper.Gettopics();
				if (list != null && list.Count > 0)
				{
					this.ddlSubType.DataSource = list;
					this.ddlSubType.DataTextField = "title";
					this.ddlSubType.DataValueField = "TopicId";
					this.ddlSubType.DataBind();
					this.ddlSubType.SelectedValue = tplCfgById.Url;
				}
				break;
			}
			case (LocationType)1:
			case LocationType.Home:
			case LocationType.Category:
			case LocationType.ShoppingCart:
			case LocationType.OrderCenter:
			case LocationType.VipCard:
				break;
			case LocationType.Activity:
			{
				this.ddlSubType.Attributes.CssStyle.Remove("display");
                this.ddlSubType.BindEnum<EcShop.Entities.AppLocationType>("");//修改1
				this.ddlSubType.SelectedValue = tplCfgById.Url.Split(new char[]
				{
					','
				})[0];
				this.ddlThridType.Attributes.CssStyle.Remove("display");
				LotteryActivityType lotteryActivityType = (LotteryActivityType)System.Enum.Parse(typeof(LotteryActivityType), tplCfgById.Url.Split(new char[]
				{
					','
				})[0]);
				if (lotteryActivityType == LotteryActivityType.SignUp)
				{
					this.ddlThridType.DataSource = 
						from item in VShopHelper.GetAllActivity()
						select new
						{
							ActivityId = item.ActivityId,
							ActivityName = item.Name
						};
				}
				else
				{
					this.ddlThridType.DataSource = VShopHelper.GetLotteryActivityByType(lotteryActivityType);
				}
				this.ddlThridType.DataTextField = "ActivityName";
				this.ddlThridType.DataValueField = "Activityid";
				this.ddlThridType.DataBind();
				this.ddlThridType.SelectedValue = tplCfgById.Url.Split(new char[]
				{
					','
				})[1];
				return;
			}
			case LocationType.Link:
				this.Tburl.Text = tplCfgById.Url;
				this.Tburl.Attributes.CssStyle.Remove("display");
				return;
			case LocationType.Phone:
				this.Tburl.Text = tplCfgById.Url;
				this.Tburl.Attributes.CssStyle.Remove("display");
				return;
			case LocationType.Address:
				this.Tburl.Attributes.CssStyle.Remove("display");
				this.navigateDesc.Attributes.CssStyle.Remove("display");
				this.Tburl.Text = tplCfgById.Url;
				return;
			default:
				return;
			}
		}
		protected void btnEditBanner_Click(object sender, System.EventArgs e)
		{
			TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
			tplCfgById.IsDisable = false;
			tplCfgById.ImageUrl = this.fmSrc.Value;
			tplCfgById.ShortDesc = this.txtBannerDesc.Text;
			tplCfgById.Client = 3;
			tplCfgById.LocationType = (LocationType)System.Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue);
			tplCfgById.Url = this.locationUrl.Value;
			if (VShopHelper.UpdateTplCfg(tplCfgById))
			{
				this.CloseWindow();
				return;
			}
			this.ShowMsg("修改失败！", false);
		}
	}
}
