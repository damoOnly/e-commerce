using EcShop.ControlPanel.Sales;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class ExpressPrintButton : WebControl
	{
		public int ShipperId
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
			if (isUserExpressTemplates != null && isUserExpressTemplates.Rows.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<div>");
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
				{
					IEnumerator enumerator = isUserExpressTemplates.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							stringBuilder.AppendFormat("<a href=\"flex/print.html?ShipperId={0}&OrderId={1}&XmlFile={2}\" style=\"margin-right:10px;\" >{3}</a> ", new object[]
							{
								this.ShipperId,
								this.Page.Request.QueryString["OrderId"],
								dataRow["XmlFile"],
								dataRow["ExpressName"]
							});
						}
						goto IL_1B4;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
				{
					foreach (DataRow dataRow2 in isUserExpressTemplates.Rows)
					{
						stringBuilder.AppendFormat("<a href=\"flex/print.html?ShipperId={0}&PurchaseOrderId={1}&XmlFile={2}\" style=\"margin-right:10px;\" >{3}</a> ", new object[]
						{
							this.ShipperId,
							this.Page.Request.QueryString["PurchaseOrderId"],
							dataRow2["XmlFile"],
							dataRow2["ExpressName"]
						});
					}
				}
				IL_1B4:
				stringBuilder.Append("</div>");
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
