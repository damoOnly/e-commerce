using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class SourceOrderLable : Literal
	{
		public object SourceOrder
		{
			get
			{
				return this.ViewState["SourceOrder"];
			}
			set
			{
				this.ViewState["SourceOrder"] = value;
			}
		}

        public object StoreId
        {
            get
            {
                return this.ViewState["StoreId"];
            }
            set
            {
                this.ViewState["StoreId"] = value;
            }
        }


		protected override void Render(HtmlTextWriter writer)
		{
            base.Text = this.GetOrderSource((this.SourceOrder != null && !string.IsNullOrEmpty(this.SourceOrder.ToString())) ? int.Parse(this.SourceOrder.ToString()) : 0, (this.StoreId != null && !string.IsNullOrEmpty(this.StoreId.ToString())) ? int.Parse(this.StoreId.ToString()) : 0);
			base.Render(writer);
		}
        private string GetOrderSource(int sourceorder, int storeId)
		{
            string storeName = "";
            StoreInfo storeinfo = new StoreInfo();
            storeinfo = StoreHelper.GetStoreByStoreId(storeId);
            if (storeinfo != null)
            {
                storeName = storeinfo.StoreName;
            }
			string result = "";
			switch (sourceorder)
			{
                case (int)OrderSource.Taobao:
				    result = "<img src=\"" + Utils.ApplicationPath + "/Utility/pics/tao.gif\" title=\"淘宝订单\"/>";
				    break;
                case (int)OrderSource.WeiXin:
				    result = "<img src=\"" + Utils.ApplicationPath + "/Utility/pics/wx.gif\"  title=\"微信订单\"/>";
				    break;
                case (int)OrderSource.Wap:
				    result = "<img src=\"" + Utils.ApplicationPath + "/Utility/pics/wap.gif\" title=\"手机订单\"/>";
				    break;
                case (int)OrderSource.storeAdd:
                    result = string.Format("<img src=\"" + Utils.ApplicationPath + "/Utility/pics/store.gif\" title=\"门店订单\"/><span class='storeName'>{0}</span>", storeName);
                    break;
                case (int)OrderSource.IOS:
                    result = "<span>ios订单</span>";
                    break;
                case (int)OrderSource.Android:
                    result = "<span>Android订单</span>";
                    break;
                case (int)OrderSource.PC:
                    result = "<span>PC端订单</span>";
                    break;
                case 6:
                    result = "<span>原App订单</span>";
                    break;
			}
			return result;
		}
	}
}
