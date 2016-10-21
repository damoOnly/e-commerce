using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPTopics : WAPTemplatedWebControl
	{
		private int topicId;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
        private System.Web.UI.HtmlControls.HtmlInputText txtSearch;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VTopics.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["TopicId"], out this.topicId))
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
				this.Page.Response.Redirect(text);
				return;
			}
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			TopicInfo topic = VshopBrowser.GetTopic(this.topicId);

            if (topic == null)
            {
                base.GotoResourceNotFound("");
            }

            //主题大图和标题
            this.imgUrl.ImageUrl = topic.IconUrl;
            this.litContent.Text = topic.Content;
            PageTitle.AddSiteNameTitle(topic.Content);
		}
	}
}
