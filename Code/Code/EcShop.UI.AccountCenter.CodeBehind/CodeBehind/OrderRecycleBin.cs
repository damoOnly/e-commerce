using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class OrderRecycleBin : MemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputText txtOrderId;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private Pager pager;
        private Common_OrderRecycleBin_OrderList listOrders;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "User/Skin-OrderRecycleBin.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtOrderId = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtOrderId");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
            this.listOrders = (Common_OrderRecycleBin_OrderList)this.FindControl("common_orderrecyclebin_orderlist");
            this.listOrders.ItemDataBound += new Common_OrderRecycleBin_OrderList.DataBindEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.ItemCommand += new Common_OrderRecycleBin_OrderList.CommandEventHandler(this.listOrders_ItemCommand);
            this.pager = (Pager)this.FindControl("pager");

			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			if (!this.Page.IsPostBack)
			{
                this.BindOrders();
			}
		}

        #region *****
        protected void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
                System.DateTime t = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FinishDate") == System.DBNull.Value) ? System.DateTime.Now.AddYears(-1) : ((System.DateTime)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FinishDate"));
                string a = "";
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") != null && !(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
                {
                    a = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
                }
                System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("hplinkorderreview");

                System.Web.UI.HtmlControls.HtmlAnchor linkNowPay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("hlinkPay");

                ImageLinkButton confirmOrder = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
                ImageLinkButton cancelOrder = (ImageLinkButton)e.Item.FindControl("lkbtnCloseOrder");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForRefund = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForRefund");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReturn = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForReturn");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnNotPay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnNotPay");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReplace = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForReplace");

                System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rpProduct");
                //�鿴����
                System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("Logistics");
                if (hyperLink != null)
                {
                    hyperLink.Visible = (orderStatus == OrderStatus.Finished);
                }

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

                //δ����
                lkbtnNotPay.Visible = (orderStatus == OrderStatus.WaitBuyerPay);
                //�˻�
                lkbtnApplyForReturn.Visible = (orderStatus == OrderStatus.Finished && t >= System.DateTime.Now.AddDays((double)(-(double)masterSettings.EndOrderDays)));

                //�˿�
                lkbtnApplyForRefund.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || orderStatus == OrderStatus.SellerAlreadySent);
                //����
                lkbtnApplyForReplace.Visible = (orderStatus == OrderStatus.Finished && t >= System.DateTime.Now.AddDays((double)(-(double)masterSettings.EndOrderDays)));

                /*1.�ȴ���Ҹ��WaitBuyerPay�� ->        δ����/ȡ��     
                  2.�Ѹ���,�ȴ�������BuyerAlreadyPaid�� -> �˿�        
                  3.�ѷ�����SellerAlreadySent��          ->�˿�,ȷ���ջ����鿴����     
                  4.��������ɣ�Finished��  -> �˻�/���� �鿴����
                */

                if (repeater != null)
                {
                    string orderId = ((DataRowView)e.Item.DataItem).Row["OrderId"].ToString();
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        DataTable dt = TradeHelper.GetOrderItemThumbnailsUrl(orderId);
                        repeater.DataSource = dt;
                        repeater.DataBind();
                    }

                }
            }
        }
        protected void listOrders_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            string orderId = e.CommandArgument.ToString();
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (TradeHelper.ConfirmOrderFinish(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMessage("�ɹ�������˸ö���", true);
                    }
                    else
                    {
                        this.ShowMessage("��ɶ���ʧ��", false);
                    }
                }
                if (e.CommandName == "CLOSE_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
                {
                    // 2015-08-19
                    if (TradeHelper.CloseOrder(orderInfo.OrderId))
                    {
                        Messenger.OrderClosed(HiContext.Current.User, orderInfo, "�û��Լ��رն���");
                        this.BindOrders();
                        this.ShowMessage("�ɹ��Ĺر��˸ö���", true);
                        return;
                    }
                    this.ShowMessage("�رն���ʧ��", false);
                }
            }

            //����վ����ɾ������
            if (e.CommandName == "completeDel")
            {
                int result = TradeHelper.LogicDeleteOrder(orderId,(int)UserStatus.CompleteDelete);
                if (result > 0)
                {
                    this.BindOrders();
                    this.ShowMessage("ɾ�������ɹ�", true);
                }
                else
                {
                    this.ShowMessage("ɾ������ʧ��", false);
                }
            }

            if (e.CommandName == "RevertDel")
            {
                int result = TradeHelper.LogicDeleteOrder(orderId, (int)UserStatus.DefaultStatus);
                if (result > 0)
                {
                    this.BindOrders();
                    this.ShowMessage("��ԭ�����ɹ�", true);
                }
                else
                {
                    this.ShowMessage("��ԭ����ʧ��", false);
                }
            }

        }
        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
            this.listOrders.DataSource = userOrder.Data;
            this.listOrders.DataBind();
            this.txtOrderId.Value = orderQuery.OrderId;
            this.pager.TotalRecords = userOrder.TotalRecords;

        }
        private OrderQuery GetOrderQuery()
        {
            OrderQuery orderQuery = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
            {
                int status = 0;
                if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out status))
                {
                    orderQuery.Status = (OrderStatus)status;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                orderQuery.SortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                int sortOrder = 0;
                if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out sortOrder))
                {
                    orderQuery.SortOrder = (SortAction)sortOrder;
                }
            }
            orderQuery.UseStatus = UserStatus.RecycleDelete;
            orderQuery.PageIndex = this.pager.PageIndex;
            orderQuery.PageSize = this.pager.PageSize;
            return orderQuery;
        }
        #endregion
        private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrder(true);
		}

        private void ReloadOrder(bool isSearch)
		{
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("orderId", this.txtOrderId.Value);
            nameValueCollection.Add("sortOrder", ((int)this.listOrders.SortOrder).ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(nameValueCollection);
		}
	}
}
