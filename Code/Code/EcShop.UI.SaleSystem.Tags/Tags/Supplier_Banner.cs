using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Supplier_Banner : WebControl
	{
        public int supplierId;
		protected override void Render(HtmlTextWriter writer)
		{
            int.TryParse(this.Page.Request.QueryString["supplierid"], out supplierId);
			writer.Write(this.RendHtml());
		}
		public string RendHtml()
		{
            IList<BannerInfo> banners = VShopHelper.GetAllBanners(ClientType.PC, supplierId);
			StringBuilder str = new StringBuilder();
            if(banners != null && banners.Count > 0 )
            {
                foreach(BannerInfo b in banners)
                {
                    var url = b.Url.ToString().Length > 0 ? b.Url : "javascript:void(0);";
                    str.AppendFormat("<li><a href='{0}' style='background:url({1})  no-repeat center top;'></a></li>", url, b.ImageUrl);
                }
            }
			return str.ToString();  
		}
	}
}
