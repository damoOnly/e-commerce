using EcShop.ControlPanel.Store;
using EcShop.Entities.AliOH;
using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.SaleSystem.Comments;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohBannerEdit)]
	public class EditBanner : AdminPage
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
                    this.ddlType.BindEnum<EcShop.Entities.AliOH.LocationType>("");//修改1
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
			case EcShop.Entities.VShop.LocationType.Topic:
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
			case (EcShop.Entities.VShop.LocationType)1:
			case EcShop.Entities.VShop.LocationType.Home:
			case EcShop.Entities.VShop.LocationType.Category:
			case EcShop.Entities.VShop.LocationType.ShoppingCart:
			case EcShop.Entities.VShop.LocationType.OrderCenter:
			case EcShop.Entities.VShop.LocationType.VipCard:
			case EcShop.Entities.VShop.LocationType.GroupBuy:
			case EcShop.Entities.VShop.LocationType.Brand:
				break;
			case EcShop.Entities.VShop.LocationType.Activity:
			{
				this.ddlSubType.Attributes.CssStyle.Remove("display");
                this.ddlSubType.BindEnum<EcShop.Entities.AliOH.LocationType>("");//修改1
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
			case EcShop.Entities.VShop.LocationType.Article:
			{
				this.ddlSubType.Attributes.CssStyle.Remove("display");
				System.Collections.Generic.IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
				this.ddlSubType.Items.Clear();
				int num = 0;
				int num2 = 0;
				if (!string.IsNullOrEmpty(tplCfgById.Url))
				{
					int num3 = tplCfgById.Url.LastIndexOf('=');
					int.TryParse(tplCfgById.Url.Substring(num3 + 1), out num);
				}
				if (num > 0)
				{
					ArticleInfo article = CommentBrowser.GetArticle(num);
					if (article != null)
					{
						num2 = article.CategoryId;
					}
				}
				if (articleMainCategories != null && articleMainCategories.Count > 0)
				{
					foreach (ArticleCategoryInfo current in articleMainCategories)
					{
						this.ddlSubType.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.CategoryId.ToString()));
					}
					if (num2 > 0)
					{
						this.ddlSubType.SelectedValue = num2.ToString();
					}
				}
				if (num > 0)
				{
					this.ddlThridType.Attributes.CssStyle.Remove("display");
					System.Collections.Generic.IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(num2, 1000);
					foreach (ArticleInfo current2 in articleList)
					{
						this.ddlThridType.Items.Add(new System.Web.UI.WebControls.ListItem(current2.Title, current2.ArticleId.ToString()));
					}
					this.ddlThridType.SelectedValue = num.ToString();
					return;
				}
				break;
			}
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
			tplCfgById.LocationType = (EcShop.Entities.VShop.LocationType)System.Enum.Parse(typeof(EcShop.Entities.VShop.LocationType), this.ddlType.SelectedValue);
			tplCfgById.Url = this.locationUrl.Value;
			tplCfgById.Client = 4;
			if (string.IsNullOrWhiteSpace(tplCfgById.ImageUrl))
			{
				this.ShowMsg("请上传轮播图！", false);
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
