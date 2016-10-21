using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPMemberReturnsDetails : WAPMemberTemplatedWebControl
    {
        private int returnsId;
        private string orderId;
        private System.Web.UI.WebControls.Literal txtOrderId;
        private System.Web.UI.WebControls.Literal handleStatus;
        private FormatedTimeLabel litAddDate;
        //private Common_OrderManage_OrderItems RefundDetails;
        private System.Web.UI.WebControls.Literal litRemark;
        private System.Web.UI.WebControls.Literal litWeight;
        private System.Web.UI.WebControls.Literal litAdminRemark;
        private System.Web.UI.WebControls.Literal litType;
        private System.Web.UI.WebControls.Literal litLogisticsCompany;
        private System.Web.UI.WebControls.Literal litLogisticsId;
        private FormatedMoneyLabel litRefundTotal;
        private FormatedMoneyLabel litTotalPrice;
        private FormatedMoneyLabel litOrderTotal;
        private WapTemplatedRepeater RefundDetails;
        protected ReturnsInfo returnsInfo;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberReturnsDetails.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.returnsId = System.Convert.ToInt32(this.Page.Request.QueryString["ReturnsId"]);
            //this.RefundDetails = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
            this.RefundDetails = (WapTemplatedRepeater)this.FindControl("rptOrderProducts");
            this.txtOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("txtOrderId");
            this.handleStatus = (System.Web.UI.WebControls.Literal)this.FindControl("handleStatus");
            this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
            this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
            this.litAdminRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litAdminRemark");
            this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
            this.litType = (System.Web.UI.WebControls.Literal)this.FindControl("litType");
            this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
            this.litRefundTotal = (FormatedMoneyLabel)this.FindControl("litRefundTotal");
            this.litOrderTotal = (FormatedMoneyLabel)this.FindControl("litOrderTotal");
            this.litLogisticsCompany = (System.Web.UI.WebControls.Literal)this.FindControl("litLogisticsCompany");
            this.litLogisticsId = (System.Web.UI.WebControls.Literal)this.FindControl("litLogisticsId");

            if (!this.Page.IsPostBack)
            {
                this.BindReturnsTable(this.returnsId);
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                returnsInfo = TradeHelper.GetReturnsInfo(this.returnsId);
                if (returnsInfo == null || returnsInfo.Username != HiContext.Current.User.Username)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
                    return;
                }
                this.BindOrderItems(orderInfo);
                this.BindRefunds(returnsInfo);
            }

            PageTitle.AddSiteNameTitle("退货申请单");

            WAPHeadName.AddHeadName("退货申请单");
        }
        private void BindRefunds(ReturnsInfo returnsInfo)
        {
            this.RefundDetails.DataSource = returnsInfo.ReturnsLineItem;
            this.RefundDetails.DataBind();
        }
        private void BindOrderItems(OrderInfo order)
        {
            if (order.OrderStatus == OrderStatus.Returned && this.handleStatus.Text == "已完成")
            {
                decimal num;
                TradeHelper.GetRefundMoney(order, out num);
                this.litRefundTotal.Money = num;
            }
            this.litTotalPrice.Money = returnsInfo.GetAmount();
            this.litOrderTotal.Money = order.GetTotal();
            this.litWeight.Text = order.Weight.ToString("F2");
            this.txtOrderId.Text = order.OrderId;
            this.litLogisticsCompany.Text = returnsInfo.LogisticsCompany;
            this.litLogisticsId.Text = returnsInfo.LogisticsId;
        }
        private void BindReturnsTable(int returnsId)
        {
            DataTable returnsApplysTable = TradeHelper.GetReturnsApplysTable(returnsId);
            if (returnsApplysTable.Rows[0]["HandleStatus"] != System.DBNull.Value)
            {
                if ((int)returnsApplysTable.Rows[0]["HandleStatus"] == 0)
                {
                    this.handleStatus.Text = "待处理";
                }
                else
                {
                    if ((int)returnsApplysTable.Rows[0]["HandleStatus"] == 1)
                    {
                        this.handleStatus.Text = "已完成";
                    }
                    else if ((int)returnsApplysTable.Rows[0]["HandleStatus"] == 2)
                    {
                        this.handleStatus.Text = "已拒绝";
                    }
                    else
                    {
                        this.handleStatus.Text = "已受理";
                    }
                }
            }
            if (returnsApplysTable.Rows[0]["ApplyForTime"] != System.DBNull.Value)
            {
                this.litAddDate.Time = (System.DateTime)returnsApplysTable.Rows[0]["ApplyForTime"];
            }
            if (returnsApplysTable.Rows[0]["AdminRemark"] != System.DBNull.Value)
            {
                this.litAdminRemark.Text = returnsApplysTable.Rows[0]["AdminRemark"].ToString();
            }
            if (returnsApplysTable.Rows[0]["Comments"] != System.DBNull.Value)
            {
                this.litRemark.Text = returnsApplysTable.Rows[0]["Comments"].ToString();
            }
            if (returnsApplysTable.Rows[0]["RefundType"] != System.DBNull.Value)
            {
                string refundType = returnsApplysTable.Rows[0]["RefundType"].ToString();
                if (refundType.Equals("1"))
                {
                    this.litType.Text = "预付款";
                }
                else if (refundType.Equals("2"))
                {
                    this.litType.Text = "银行转账";
                }
                else if (refundType.Equals("3"))
                {
                    this.litType.Text = "原路返回";
                }
            }
            if (returnsApplysTable.Rows[0]["OrderId"] != System.DBNull.Value)
            {
                this.orderId = returnsApplysTable.Rows[0]["OrderId"].ToString();
            }
        }
    }
}
