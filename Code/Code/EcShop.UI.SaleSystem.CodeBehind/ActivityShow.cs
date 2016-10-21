using EcShop.ControlPanel.Commodities;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EcShop.UI.SaleSystem.CodeBehind
{
   [System.Web.UI.ParseChildren(true)]
    public class ActivityShow : HtmlTemplatedWebControl
    {
       
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ActivityShow1.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            if (!string.IsNullOrWhiteSpace(TitilName))
            {
                PageTitle.AddSiteNameTitle(TitilName);
            }
        }

       
    }
}
