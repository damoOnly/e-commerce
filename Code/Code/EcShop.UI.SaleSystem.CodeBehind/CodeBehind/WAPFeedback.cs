using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPFeedback:WAPTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WAPFeedback.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("反馈");

            WAPHeadName.AddHeadName("反馈");
        }
    }
}