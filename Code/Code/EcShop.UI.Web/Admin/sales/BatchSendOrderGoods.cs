using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using System;
using System.Net;
using System.Web.UI.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;

namespace EcShop.UI.Web.Admin.sales
{
	public class BatchSendOrderGoods : AdminPage
	{
		private string strIds;
		protected ShippingModeDropDownList dropShippingMode;
		protected System.Web.UI.WebControls.Button btnSetShippingMode;
		protected System.Web.UI.WebControls.DropDownList dropExpressComputerpe;
		protected System.Web.UI.WebControls.TextBox txtStartShipOrderNumber;
		protected System.Web.UI.WebControls.Button btnSetShipOrderNumber;
		protected Grid grdOrderGoods;
		protected DropdownColumn dropShippId;
		protected DropdownColumn dropExpress;
		protected System.Web.UI.WebControls.Button btnBatchSendGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.strIds = base.Request.QueryString["OrderIds"];
			this.btnSetShippingMode.Click += new System.EventHandler(this.btnSetShippingMode_Click);
			this.btnSetShipOrderNumber.Click += new System.EventHandler(this.btnSetShipOrderNumber_Click);
			this.grdOrderGoods.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdOrderGoods_RowDataBound);
			this.btnBatchSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropShippingMode.DataBind();
				this.dropExpressComputerpe.DataSource = ExpressHelper.GetAllExpress();
				this.dropExpressComputerpe.DataTextField = "Name";
				this.dropExpressComputerpe.DataValueField = "Name";
				this.dropExpressComputerpe.DataBind();
				this.dropExpressComputerpe.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
				this.BindData();
			}
		}
		private void grdOrderGoods_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				string orderId = (string)this.grdOrderGoods.DataKeys[e.Row.RowIndex].Value;
				System.Web.UI.WebControls.DropDownList dropDownList = e.Row.FindControl("dropExpress") as System.Web.UI.WebControls.DropDownList;
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo != null && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(orderInfo.ExpressCompanyName);
					if (expressCompanyInfo != null)
					{
						dropDownList.SelectedValue = expressCompanyInfo.Kuaidi100Code;
					}
				}
			}
		}
		protected string GetOrderIds()
		{
			string text = string.Empty;
			for (int i = 0; i < this.grdOrderGoods.Rows.Count; i++)
			{
				System.Web.UI.WebControls.GridViewRow arg_1B_0 = this.grdOrderGoods.Rows[i];
				text = text + this.grdOrderGoods.DataKeys[i].Value.ToString() + ",";
			}
			return text.TrimEnd(new char[]
			{
				','
			});
		}
		private void btnSetShipOrderNumber_Click(object sender, System.EventArgs e)
		{
			string orderIds = this.GetOrderIds();
			string[] orderIds2 = orderIds.Split(new char[]
			{
				','
			});
			long num = 0L;
			string orderIds3 = "'" + orderIds.Replace(",", "','") + "'";
			if (!string.IsNullOrEmpty(this.dropExpressComputerpe.SelectedValue))
			{
				OrderHelper.SetOrderExpressComputerpe(orderIds3, this.dropExpressComputerpe.SelectedItem.Text, this.dropExpressComputerpe.SelectedValue);
			}
			if (!string.IsNullOrEmpty(this.txtStartShipOrderNumber.Text.Trim()) && long.TryParse(this.txtStartShipOrderNumber.Text.Trim(), out num))
			{
				try
				{
					OrderHelper.SetOrderShipNumber(orderIds2, this.txtStartShipOrderNumber.Text.Trim(), this.dropExpressComputerpe.SelectedValue);
					this.BindData();
					return;
				}
				catch (System.Exception)
				{
					this.ShowMsg("你输入的起始单号不正确", false);
					return;
				}
			}
			this.ShowMsg("起始发货单号不允许为空且必须为正整数", false);
		}
		private void btnSetShippingMode_Click(object sender, System.EventArgs e)
		{
			string orderIds = this.GetOrderIds();
			string orderIds2 = "'" + orderIds.Replace(",", "','") + "'";
			if (this.dropShippingMode.SelectedValue.HasValue)
			{
				OrderHelper.SetOrderShippingMode(orderIds2, this.dropShippingMode.SelectedValue.Value, this.dropShippingMode.SelectedItem.Text);
			}
			this.BindData();
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			if (this.grdOrderGoods.Rows.Count <= 0)
			{
				this.ShowMsg("没有要进行发货的订单。", false);
				return;
			}
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdOrderGoods.Columns[4];
			System.Web.UI.WebControls.ListItemCollection selectedItems = dropdownColumn.SelectedItems;
			DropdownColumn dropdownColumn2 = (DropdownColumn)this.grdOrderGoods.Columns[5];
			System.Web.UI.WebControls.ListItemCollection selectedItems2 = dropdownColumn2.SelectedItems;
			int num = 0;
			for (int i = 0; i < selectedItems.Count; i++)
			{
				string orderId = (string)this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
				System.Web.UI.WebControls.ListItem listItem = selectedItems[i];
				System.Web.UI.WebControls.ListItem listItem2 = selectedItems2[i];
				int num2 = 0;
				int.TryParse(listItem.Value, out num2);
				if (!string.IsNullOrEmpty(textBox.Text.Trim()) && !string.IsNullOrEmpty(listItem.Value) && int.Parse(listItem.Value) > 0 && !string.IsNullOrEmpty(listItem2.Value))
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
					if ((orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "Ecdev.plugins.payment.podrequest") || orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && num2 > 0 && !string.IsNullOrEmpty(textBox.Text.Trim()) && textBox.Text.Trim().Length <= 20)
					{
						ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(num2, true);
						orderInfo.RealShippingModeId = shippingMode.ModeId;
						orderInfo.RealModeName = shippingMode.Name;
						ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(listItem2.Value);
						orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
						orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
						orderInfo.ShipOrderNumber = textBox.Text;
						if (OrderHelper.SendGoods(orderInfo))
						{
							SendNoteInfo sendNoteInfo = new SendNoteInfo();
							sendNoteInfo.NoteId = Globals.GetGenerateId() + num;
							sendNoteInfo.OrderId = orderId;
							sendNoteInfo.Operator = HiContext.Current.User.Username;
							sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
							OrderHelper.SaveSendNote(sendNoteInfo);
							if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
							{
								PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
								if (paymentMode != null)
								{
									PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
									{
										paymentMode.Gateway
									})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
									{
										paymentMode.Gateway
									})), "");
									paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
								}
							}
							if (!string.IsNullOrEmpty(orderInfo.TaobaoOrderId))
							{
								try
								{
									string requestUriString = string.Format("http://vip.ecdev.cn/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}&Host={3}", new object[]
									{
										orderInfo.TaobaoOrderId,
										expressCompanyInfo.TaobaoCode,
										orderInfo.ShipOrderNumber,
										HiContext.Current.SiteUrl
									});
									System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
									webRequest.GetResponse();
								}
								catch
								{
								}
							}
							int num3 = orderInfo.UserId;
							if (num3 == 1100)
							{
								num3 = 0;
							}
							IUser user = Users.GetUser(num3);
							Messenger.OrderShipping(orderInfo, user);
							orderInfo.OnDeliver();
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				this.ShowMsg("批量发货失败！", false);
				return;
			}
			if (num > 0)
			{
				this.BindData();
				this.ShowMsg(string.Format("批量发货成功！发货数量{0}个", num), true);
			}
		}
		private void BindData()
		{
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdOrderGoods.Columns[4];
			dropdownColumn.DataSource = SalesHelper.GetShippingModes();
			DropdownColumn dropdownColumn2 = (DropdownColumn)this.grdOrderGoods.Columns[5];
			dropdownColumn2.DataSource = ExpressHelper.GetAllExpress();
			string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
			this.grdOrderGoods.DataSource = OrderHelper.GetSendGoodsOrders(orderIds);
			this.grdOrderGoods.DataBind();
		}
	}
}
