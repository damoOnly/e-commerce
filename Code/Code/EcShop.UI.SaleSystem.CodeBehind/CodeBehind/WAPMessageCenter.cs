using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPMessageCenter : WAPMemberTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAPMessageCenter.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("消息中心");

            WAPHeadName.AddHeadName("消息中心");
        }
    }
}
