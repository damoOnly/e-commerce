using ASPNET.WebControls;
using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class ActiveTopicOne : HtmlTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "ActiveTopicOne.html";
            }
            base.OnInit(e);
        }



        protected override void AttachChildControls()
        {
           
        }
    }
}
