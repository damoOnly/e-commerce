using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_WapHeader2 : WAPTemplatedWebControl
    {
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Header2.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {

        }
    }
}