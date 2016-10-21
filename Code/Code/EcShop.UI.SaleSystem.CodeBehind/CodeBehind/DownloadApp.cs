using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class DownloadApp : HtmlTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DownloadApp.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {

        }
    }
}
