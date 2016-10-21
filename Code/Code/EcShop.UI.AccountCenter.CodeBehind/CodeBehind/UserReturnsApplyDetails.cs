using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class UserReturnsApplyDetails : MemberTemplatedWebControl
    {
        private int returnsId;
        private string orderId;
        private System.Web.UI.WebControls.Literal txtOrderId;
        private System.Web.UI.WebControls.Literal handleStatus;
        private FormatedTimeLabel litAddDate;
        private Common_OrderManage_OrderItems RefundDetails;
        private System.Web.UI.WebControls.Literal litRemark;
        private System.Web.UI.WebControls.Literal litWeight;
        private System.Web.UI.WebControls.Literal litAdminRemark;
        private System.Web.UI.WebControls.Literal litType;
        private FormatedMoneyLabel litRefundTotal;
        private FormatedMoneyLabel litTotalPrice;
        private FormatedMoneyLabel litOrderTotal;
        private System.Web.UI.WebControls.Literal litLogisticsCompany;//物流公司
        private System.Web.UI.WebControls.Literal litLogisticsId;//物流单号
        /// <summary>
        /// 清关费
        /// </summary>
        private FormatedMoneyLabel litCustomsClearanceFee;
        /// <summary>
        /// 快递费
        /// </summary>
        private FormatedMoneyLabel litExpressFee;
        /// <summary>
        /// 费用归属
        /// </summary>
        private System.Web.UI.WebControls.Literal litFeeAffiliation;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserReturnsApplyDetails.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.returnsId = System.Convert.ToInt32(this.Page.Request.QueryString["ReturnsId"]);
            this.RefundDetails = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
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
            this.litCustomsClearanceFee = (FormatedMoneyLabel)this.FindControl("litCustomsClearanceFee");
            this.litExpressFee = (FormatedMoneyLabel)this.FindControl("litExpressFee");
            this.litFeeAffiliation = (System.Web.UI.WebControls.Literal)this.FindControl("litFeeAffiliation");
            this.litLogisticsCompany = (System.Web.UI.WebControls.Literal)this.FindControl("litLogisticsCompany");
            this.litLogisticsId = (System.Web.UI.WebControls.Literal)this.FindControl("litLogisticsId");
            if (!this.Page.IsPostBack)
            {
                this.BindReturnsTable(this.returnsId);
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                if (orderInfo == null || orderInfo.UserId != HiContext.Current.User.UserId)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
                    return;
                }
                this.BindOrderItems(orderInfo);
                this.BindRefunds(orderInfo);
            }
        }
        private void BindRefunds(OrderInfo order)
        {
            Entities.ReturnsInfo inf = OrderHelper.GetReturnsInfo(this.returnsId);
            if (order.OrderStatus == OrderStatus.ApplyForReturns)
            {
                List<LineItemInfo> skus = new List<LineItemInfo>();
                foreach (Entities.OrderAppFormItems v in inf.ReturnsLineItem)
                {
                    if (order.LineItems.ContainsKey(v.SkuId))
                    {
                        order.LineItems[v.SkuId].Quantity = v.Quantity;
                        skus.Add(order.LineItems[v.SkuId]);
                    }
                }
                order.LineItems.Clear();
                decimal monye = 0m;
                foreach (LineItemInfo l in skus)
                {
                    if (!order.LineItems.ContainsKey(l.SkuId))
                    {
                        order.LineItems.Add(l.SkuId, l);
                        monye += l.Quantity * l.ItemListPrice;
                    }
                   
                }
                //计算退货单金额
                this.litTotalPrice.Money = monye;
                this.litOrderTotal.Money = monye;
                this.RefundDetails.DataSource = order.LineItems.Values;
                this.RefundDetails.DataBind();
            }
            else
            {
                this.litCustomsClearanceFee.Text = inf.CustomsClearanceFee.ToString("F2");
                this.litExpressFee.Text = inf.ExpressFee.ToString("F2");
                this.litFeeAffiliation.Text = inf.FeeAffiliation;
                this.RefundDetails.DataSource = order.LineItems.Values;
                this.RefundDetails.DataBind();
            }
        }
        private void BindOrderItems(OrderInfo order)
        {
            if (order.OrderStatus == OrderStatus.Returned && this.handleStatus.Text == "已退款")
            {
                decimal num;
                TradeHelper.GetRefundMoney(order, out num);
                this.litRefundTotal.Money = num;
            }
            ////计算退货单金额
            //this.litTotalPrice.Money = order.GetTotal();
            //this.litOrderTotal.Money = order.GetTotal();
            this.litWeight.Text = order.Weight.ToString("F2");
            this.txtOrderId.Text = order.OrderId;
        }
        private void BindReturnsTable(int returnsId)
        {
            DataTable returnsApplysTable = TradeHelper.GetReturnsApplysTable(returnsId);
            if (returnsApplysTable.Rows[0]["HandleStatus"] != System.DBNull.Value)
            {
                if ((int)returnsApplysTable.Rows[0]["HandleStatus"] == 0)
                {
                    this.handleStatus.Text = "未处理";
                }
                else
                {
                    if ((int)returnsApplysTable.Rows[0]["HandleStatus"] == 1)
                    {
                        this.handleStatus.Text = "已退款";
                    }
                    else
                    {
                        this.handleStatus.Text = "已拒绝";
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
                this.litType.Text = (returnsApplysTable.Rows[0]["RefundType"].ToString().Equals("1") ? "预付款" : returnsApplysTable.Rows[0]["RefundType"].ToString().Equals("2") ? "银行转账" : "原路返回");
            }
            if (returnsApplysTable.Rows[0]["OrderId"] != System.DBNull.Value)
            {
                this.orderId = returnsApplysTable.Rows[0]["OrderId"].ToString();
            }
            if (returnsApplysTable.Rows[0]["LogisticsCompany"] != System.DBNull.Value)
            {
                litLogisticsCompany.Text = returnsApplysTable.Rows[0]["LogisticsCompany"].ToString();
            }
            if (returnsApplysTable.Rows[0]["LogisticsId"] != System.DBNull.Value)
            {
                litLogisticsId.Text = returnsApplysTable.Rows[0]["LogisticsId"].ToString();
            }
            // this.litLogisticsId.Text=order
        }
    }
}
