using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class OrderReviews : MemberTemplatedWebControl
	{
		private string orderId;
		private Common_OrderManage_ReviewsOrderItems orderItems;
		private System.Web.UI.WebControls.Literal litWeight;
		private System.Web.UI.WebControls.Literal litOrderId;
		private FormatedTimeLabel litAddDate;
		private FormatedMoneyLabel lbltotalPrice;
        private OrderStatusLabel lblOrderStatus;
		private System.Web.UI.WebControls.Literal litCloseReason;
		private IButton btnRefer;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-OrderReviews.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				base.GotoResourceNotFound();
			}
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.orderItems = (Common_OrderManage_ReviewsOrderItems)this.FindControl("Common_OrderManage_ReviewsOrderItems");
			this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.lbltotalPrice = (FormatedMoneyLabel)this.FindControl("lbltotalPrice");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
            this.lblOrderStatus = (OrderStatusLabel)this.FindControl("lblOrderStatus");
			this.litCloseReason = (System.Web.UI.WebControls.Literal)this.FindControl("litCloseReason");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack && HiContext.Current.User.UserRole == UserRole.Member)
			{
				this.btnRefer.Text = "提交评论";
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo.OrderStatus != OrderStatus.Finished)
				{
					this.ShowMessage("订单还未完成，不能进行评价", false);
					this.btnRefer.Visible = false;
				}
				this.BindOrderItems(orderInfo);
				this.BindOrderBase(orderInfo);
			}
		}
		private void BindOrderBase(OrderInfo order)
		{
			this.litOrderId.Text = order.OrderId;
			this.lbltotalPrice.Money = order.GetTotal();
			this.litAddDate.Time = order.OrderDate;
			this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
			if (order.OrderStatus == OrderStatus.Closed)
			{
				this.litCloseReason.Text = order.CloseReason;
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			DataTable productReviewAll = ProductBrowser.GetProductReviewAll(this.orderId);
			System.Collections.Generic.Dictionary<string, LineItemInfo> dictionary = new System.Collections.Generic.Dictionary<string, LineItemInfo>();
			LineItemInfo lineItemInfo = new LineItemInfo();
			foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in order.LineItems)
			{
				lineItemInfo = current.Value;
				for (int i = 0; i < productReviewAll.Rows.Count; i++)
				{
					if (lineItemInfo.ProductId.ToString() == productReviewAll.Rows[i][0].ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll.Rows[i][1].ToString().Trim())
					{
						dictionary.Add(current.Key, lineItemInfo);
						break;
					}
				}
			}
			this.orderItems.DataSource = dictionary.Values;
			this.orderItems.DataBind();
			if (dictionary.Count == 0)
			{
				this.btnRefer.Visible = false;
			}
			this.litWeight.Text = order.Weight.ToString();
		}
		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				text += Formatter.FormatErrorMessage("请填写用户名和密码");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				return false;
			}
			return true;
		}
		public void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateConvert())
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			foreach (System.Web.UI.WebControls.RepeaterItem repeaterItem in this.orderItems.Items)
			{
				System.Web.UI.HtmlControls.HtmlTextArea htmlTextArea = repeaterItem.FindControl("txtcontent") as System.Web.UI.HtmlControls.HtmlTextArea;
				System.Web.UI.HtmlControls.HtmlInputHidden htmlInputHidden = repeaterItem.FindControl("hdproductId") as System.Web.UI.HtmlControls.HtmlInputHidden;
				if (!string.IsNullOrEmpty(htmlTextArea.Value.Trim()) && !string.IsNullOrEmpty(htmlInputHidden.Value.Trim()))
				{
					dictionary.Add(htmlInputHidden.Value, htmlTextArea.Value);
				}
			}
			if (dictionary.Count <= 0)
			{
				this.ShowMessage("请输入评价内容呀！", false);
				return;
			}
			string text = "";
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
			{
				int productId = System.Convert.ToInt32(current.Key.Split(new char[]
				{
					'&'
				})[0].ToString());
				string value = current.Value;
				string skuId = current.Key.Split(new char[]
				{
					'&'
				})[2].ToString();
				ProductReviewInfo productReviewInfo = new ProductReviewInfo();
				productReviewInfo.ReviewDate = System.DateTime.Now;
				productReviewInfo.ProductId = productId;
				productReviewInfo.UserId = HiContext.Current.User.UserId;
				productReviewInfo.UserName = HiContext.Current.User.Username;
				productReviewInfo.UserEmail = HiContext.Current.User.Email;
				productReviewInfo.ReviewText = value;
				productReviewInfo.OrderID = this.orderId;
				productReviewInfo.SkuId = skuId;
				ValidationResults validationResults = Validation.Validate<ProductReviewInfo>(productReviewInfo, new string[]
				{
					"Refer"
				});
				text = string.Empty;
				if (!validationResults.IsValid)
				{
					using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator3 = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ValidationResult current2 = enumerator3.Current;
							text += Formatter.FormatErrorMessage(current2.Message);
						}
						break;
					}
				}
				if (ProductBrowser.GetProductSimpleInfo(productId) == null)
				{
					text = "您要评论的商品已经不存在";
					break;
				}
				if (!ProductBrowser.InsertProductReview(productReviewInfo))
				{
					text = "评论失败，请重试";
					break;
				}
			}
			if (text != "")
			{
				this.ShowMessage(text, false);
				return;
			}
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "评论成功", Globals.GetSiteUrls().UrlData.FormatUrl("user_UserProductReviews")));
		}
	}
}
