using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_ShippingModeList1 : WebControl   //海美生活配货方式
    {
        protected override void Render(HtmlTextWriter writer)
        {
          IList<ShippingModeInfo> shippingModeInfo=  ShoppingProcessor.GetShippingModes();
          StringBuilder stringBuiler = new StringBuilder();
          stringBuiler.Append("<h2>配货方式：</h2>");
            stringBuiler.Append("<select id=\"drpShipToDate\" class=\"textform\">");
          if (shippingModeInfo != null && shippingModeInfo.Count > 0)
          {
              foreach (ShippingModeInfo model in shippingModeInfo)
              {
                  stringBuiler.AppendFormat("<option value=\"{0}\">{1}</option>", model.ModeId, model.TemplateName);
              }
          }
          stringBuiler.Append("</select>");
          writer.Write(stringBuiler.ToString());
        }
    }
}

