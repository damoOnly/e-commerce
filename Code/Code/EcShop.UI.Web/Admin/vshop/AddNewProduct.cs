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
namespace EcShop.UI.Web.Admin.vshop
{
    [PrivilegeCheck(Privilege.vBannerAdd)]
    public class AddNewProduct : AdminPage
    {
        private int id;
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
        protected ProductCategoriesDropDownList dropCategories;
        protected ImportSourceTypeDropDownList dropImportSourceType;
        protected System.Web.UI.HtmlControls.HtmlImage littlepic;
        protected System.Web.UI.HtmlControls.HtmlAnchor linkSelectProduct;
        protected System.Web.UI.HtmlControls.HtmlInputHidden productid;
        protected System.Web.UI.HtmlControls.HtmlAnchor productName;
        protected BrandTypeDropDownList dropBrandTypes;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (!this.Page.IsPostBack)
            //{
            //    this.ddlType.BindEnum<EcShop.Entities.VShop.LocationType>("Topic");//修改1

            //    dropCategories.DataBind();
            //    dropImportSourceType.DataBind();
            //}

            if (int.TryParse(base.Request.QueryString["Id"], out this.id))
            {
                if (!this.Page.IsPostBack)
                {
                    this.ddlType.BindEnum<EcShop.Entities.VShop.LocationType>("");//修改1
                    dropCategories.DataBind();
                    dropImportSourceType.DataBind();
                    dropBrandTypes.DataBind();

                    if (id != 0)
                    {
                        this.Restore();
                    }
                    return;
                }
            }
            else
            {
                base.Response.Redirect("ManageNewProduct.aspx");
            }
        }
        protected void btnAddBanner_Click(object sender, System.EventArgs e)
        {
            if (this.id == 0)
            {
                TplCfgInfo tplCfgInfo = new RecommendInfo();
                tplCfgInfo.IsDisable = false;
                tplCfgInfo.ImageUrl = this.fmSrc.Value;
                tplCfgInfo.ShortDesc = this.txtBannerDesc.Text;
                tplCfgInfo.Client = 1;
                tplCfgInfo.LocationType = (LocationType)System.Enum.Parse(typeof(LocationType), this.ddlType.SelectedValue);
                tplCfgInfo.Url = this.locationUrl.Value;
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
                tplCfgById.Client =1;
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
        private void Reset()
        {
            this.txtBannerDesc.Text = string.Empty;
            this.Tburl.Text = string.Empty;
            this.ddlType.SelectedValue = LocationType.Link.ToString();
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
                    this.ddlSubType.Attributes.CssStyle.Remove("display");
                    this.ddlSubType.DataSource = VShopHelper.Gettopics();
                    this.ddlSubType.DataTextField = "Title";
                    this.ddlSubType.DataValueField = "TopicId";
                    this.ddlSubType.DataBind();
                    this.ddlSubType.SelectedValue = tplCfgById.Url;
                    break;
                case (LocationType)1:
                case LocationType.Home:
                case LocationType.ShoppingCart:
                case LocationType.OrderCenter:
                case LocationType.VipCard:
                case LocationType.GroupBuy:
                    break;
                case LocationType.Brand:
                    int BrandId = 0;
                    if (tplCfgById != null && !string.IsNullOrEmpty(tplCfgById.Url))
                    {
                        int.TryParse(tplCfgById.Url, out BrandId);
                    }
                    this.dropBrandTypes.Attributes.CssStyle.Remove("display");
                    this.dropBrandTypes.DataSource = ControlProvider.Instance().GetBrandCategories();
                    this.dropBrandTypes.DataTextField = "BrandName";
                    this.dropBrandTypes.DataValueField = "BrandId";
                    this.dropBrandTypes.DataBind();
                    if (BrandId > 0)
                    {
                        dropBrandTypes.SelectedValue = BrandId;
                    }
                    break;
                case LocationType.Category:
                    int categoryId = 0;
                    if (tplCfgById != null && !string.IsNullOrEmpty(tplCfgById.Url))
                    {
                        //string[] str = tplCfgById.Url.Split('?');
                        //if (str.Length > 0)
                        //{
                        //    string[] str1 = (tplCfgById.Url.Split('?')[1]).Split('=');
                        //    int.TryParse(str1[1].ToString(), out categoryId);

                        //}
                        int.TryParse(tplCfgById.Url, out categoryId);

                    }
                    this.dropCategories.Attributes.CssStyle.Remove("display");
                    this.dropCategories.DataSource = CatalogHelper.GetMainCategories();
                    this.dropCategories.DataTextField = "Name";
                    this.dropCategories.DataValueField = "CategoryId";
                    this.dropCategories.DataBind();
                    if (categoryId > 0)
                    {
                        dropCategories.SelectedValue = categoryId;
                    }
                    break;
                case LocationType.ImportSourceType:
                    int importSourceId = 0;
                    if (tplCfgById != null && !string.IsNullOrEmpty(tplCfgById.Url))
                    {
                        //string[] str = tplCfgById.Url.Split('?');
                        //if (str.Length > 0)
                        //{
                        //    string[] str1 = (tplCfgById.Url.Split('?')[1]).Split('=');
                        //    int.TryParse(str1[1].ToString(), out importSourceId);
                        //}
                        int.TryParse(tplCfgById.Url, out importSourceId);
                    }
                    this.dropImportSourceType.Attributes.CssStyle.Remove("display");
                    this.dropImportSourceType.DataSource = ImportSourceTypeHelper.GetAllImportSourceTypes();
                    this.dropImportSourceType.DataTextField = "CnArea";
                    this.dropImportSourceType.DataValueField = "ImportSourceId";
                    this.dropImportSourceType.DataBind();
                    if (importSourceId > 0)
                    {
                        dropImportSourceType.SelectedValue = importSourceId;
                    }
                    break;
                case LocationType.Activity:
                    {
                        this.ddlSubType.Attributes.CssStyle.Remove("display");
                        this.ddlSubType.BindEnum<EcShop.Entities.VShop.LocationType>("");//修改1
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
                case LocationType.Article:
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

                case LocationType.Product:

                    this.linkSelectProduct.Attributes.CssStyle.Remove("display");
                    this.productName.Attributes.CssStyle.Remove("display");
                    this.productid.Value = tplCfgById.Url;
                    int result;
                    if (int.TryParse(tplCfgById.Url, out result))
                    {
                        this.productName.InnerText = ProductHelper.GetProductDetails(result).ProductName;
                    }


                    break;

                default:
                    return;
            }
        }
    }
}
