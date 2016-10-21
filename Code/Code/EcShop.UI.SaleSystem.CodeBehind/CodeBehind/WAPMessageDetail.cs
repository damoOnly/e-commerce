using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPMessageDetail : WAPMemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.Literal MessageContent;
        private long MessageId;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAPMessageDetail.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            this.MessageContent = (System.Web.UI.WebControls.Literal)this.FindControl("MessageContent");

            if (!long.TryParse(this.Page.Request.QueryString["MessageId"], out this.MessageId))
            {
                base.GotoResourceNotFound("");
            }
            else
            {
                this.MessageContent.Text = CommentBrowser.GetMemberMessage(MessageId).Content;

                PageTitle.AddSiteNameTitle("消息详情");

                WAPHeadName.AddHeadName("消息详情");

            }
        }
    }
}

