using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Entities.Wap;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppAdImageEdit)]
	public class EditAppNavigate : AdminPage
	{
		private int id;
		protected System.Web.UI.WebControls.TextBox txtNavigateDesc;
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
                    this.ddlType.BindEnum<EcShop.Entities.AppLocationType>("");//修改1
					this.Restore();
					return;
				}
			}
			else
			{
				base.Response.Redirect("ManageNavigate.aspx");
			}
		}
		protected void Restore()
		{
			TplCfgInfo tplCfgById = VShopHelper.GetTplCfgById(this.id);
			this.txtNavigateDesc.Text = tplCfgById.ShortDesc;
			this.ddlType.SelectedValue = tplCfgById.LocationType.ToString();
			this.littlepic.Src = tplCfgById.ImageUrl;
			this.fmSrc.Value = tplCfgById.ImageUrl;
			switch (tplCfgById.LocationType)
			{
			case EcShop.Entities.VShop.LocationType.Topic:
				this.ddlSubType.Attributes.CssStyle.Remove("display");
				this.ddlSubType.DataSource = VShopHelper.Gettopics();
				this.ddlSubType.DataTextField = "Title";
				this.ddlSubType.DataValueField = "TopicId";
				this.ddlSubType.DataBind();
				this.ddlSubType.SelectedValue = tplCfgById.Url;
				break;
			case (EcShop.Entities.VShop.LocationType)1:
			case EcShop.Entities.VShop.LocationType.Home:
			case EcShop.Entities.VShop.LocationType.Category:
			case EcShop.Entities.VShop.LocationType.ShoppingCart:
			case EcShop.Entities.VShop.LocationType.OrderCenter:
			case EcShop.Entities.VShop.LocationType.VipCard:
				break;
			case EcShop.Entities.VShop.LocationType.Activity:
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
			case EcShop.Entities.VShop.LocationType.Link:
				this.Tburl.Text = tplCfgById.Url;
				this.Tburl.Attributes.CssStyle.Remove("display");
				return;
			case EcShop.Entities.VShop.LocationType.Phone:
				this.Tburl.Text = tplCfgById.Url;
				this.Tburl.Attributes.CssStyle.Remove("display");
				return;
			case EcShop.Entities.VShop.LocationType.Address:
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
			tplCfgById.Client = 3;
			tplCfgById.IsDisable = false;
			tplCfgById.ImageUrl = this.fmSrc.Value;
			tplCfgById.ShortDesc = this.txtNavigateDesc.Text;
			tplCfgById.LocationType = (EcShop.Entities.VShop.LocationType)System.Enum.Parse(typeof(EcShop.Entities.VShop.LocationType), this.ddlType.SelectedValue);
			tplCfgById.Url = this.locationUrl.Value;
			if (string.IsNullOrWhiteSpace(tplCfgById.ImageUrl))
			{
				this.ShowMsg("请选择图标或上传图片！", false);
				return;
			}
			if (string.IsNullOrWhiteSpace(this.locationUrl.Value))
			{
				this.ShowMsg("跳转页不能为空！", false);
				return;
			}
			if (VShopHelper.UpdateTplCfg(tplCfgById))
			{
				this.CloseWindow();
				return;
			}
			this.ShowMsg("修改失败！", false);
		}
	}
}
