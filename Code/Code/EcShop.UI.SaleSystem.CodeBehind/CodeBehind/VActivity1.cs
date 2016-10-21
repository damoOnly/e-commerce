using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VActivity1 : VshopTemplatedWebControl
    {
        private int topicId;
        private HiImage imgUrl;
        private System.Web.UI.WebControls.Literal litContent;
        //private System.Web.UI.WebControls.Literal litPager;
       // private VshopTemplatedRepeater rptProducts;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VActivity1.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["TopicId"], out this.topicId))
            {
                base.GotoResourceNotFound("");
            }
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
            //this.litPager = (System.Web.UI.WebControls.Literal)this.FindControl("litPager");
            //this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            TopicInfo topic = VshopBrowser.GetTopic(this.topicId);
            if (topic == null)
            {
                base.GotoResourceNotFound("");
            }
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
                base.RegisterShareScript(topic.IconUrl, text, topic.Content, topic.Title);
            }
            this.imgUrl.ImageUrl = topic.MobileBannerImageUrl;
            this.litContent.Text = topic.Content;

            //this.rptProducts.DataSource = ProductBrowser.GetTopicProducts(this.topicId, new VTemplateHelper().GetTopicProductMaxNum());           
            //int total = 0;
            //int pageNumber = 0;
            //int pageSize = new VTemplateHelper().GetTopicProductMaxNum();
            //if (!int.TryParse(this.Page.Request.QueryString["pageNumber"], out pageNumber))
            //{
            //    pageNumber = 1;
            //}
            //this.rptProducts.DataSource = ProductBrowser.GetProducts(this.topicId, null, "", pageNumber, pageSize, out total, " sort2 ",true,"asc");//分页查询
            //int totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            //this.litPager.Text = string.Format("<li class=\"previous {0}\"><a  href=\"/Vshop/Activity1.aspx?TopicId={1}&pageNumber={2}\">上一页</a></li><li class=\"next {3}\"><a href=\"/Vshop/Activity1.aspx?TopicId={1}&pageNumber={4}\">下一页</a></li>", pageNumber > 1 ? "" : "disabled", topicId, pageNumber - 1, totalPage - pageNumber > 0 ? "" : "disabled", pageNumber + 1);

            //this.rptProducts.DataBind();
            PageTitle.AddSiteNameTitle(topic.Title);
        }
    }
}
