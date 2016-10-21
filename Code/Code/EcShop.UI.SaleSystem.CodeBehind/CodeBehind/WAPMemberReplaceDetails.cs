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
    public class WAPMemberReplaceDetails : WAPMemberTemplatedWebControl
    {
        private int replaceId;
        private string orderId;
        private System.Web.UI.WebControls.Literal txtOrderId;
        private System.Web.UI.WebControls.Literal handleStatus;
        private FormatedTimeLabel litAddDate;
        //private Common_OrderManage_OrderItems RefundDetails;
        private System.Web.UI.WebControls.Literal litRemark;
        private System.Web.UI.WebControls.Literal litWeight;
        private System.Web.UI.WebControls.Literal litAdminRemark;
        private System.Web.UI.WebControls.Literal litUsername;
        private System.Web.UI.WebControls.Literal litShippingRegion;
        private System.Web.UI.WebControls.Literal litZipCode;
        private System.Web.UI.WebControls.Literal litEmailAddress;
        private System.Web.UI.WebControls.Literal litCellPhone;
        private System.Web.UI.WebControls.Literal litTelPhone;
        private System.Web.UI.WebControls.Literal litShipToDate;
        private FormatedMoneyLabel litTotalPrice;
        private WapTemplatedRepeater RefundDetails;
        private ReplaceInfo replaceInfo;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberReplaceDetails.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.replaceId = System.Convert.ToInt32(this.Page.Request.QueryString["ReplaceId"]);
            //this.RefundDetails = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
            this.RefundDetails = (WapTemplatedRepeater)this.FindControl("rptOrderProducts");
            this.txtOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("txtOrderId");
            this.handleStatus = (System.Web.UI.WebControls.Literal)this.FindControl("handleStatus");
            this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
            this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
            this.litAdminRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litAdminRemark");
            this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
            this.litUsername = (System.Web.UI.WebControls.Literal)this.FindControl("litUsername");
            this.litShippingRegion = (System.Web.UI.WebControls.Literal)this.FindControl("litShippingRegion");
            this.litZipCode = (System.Web.UI.WebControls.Literal)this.FindControl("litZipCode");
            this.litEmailAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litEmailAddress");
            this.litCellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litCellPhone");
            this.litTelPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litTelPhone");
            this.litShipToDate = (System.Web.UI.WebControls.Literal)this.FindControl("litShipToDate");
            this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
            if (!this.Page.IsPostBack)
            {
                this.BindOrderReplace(this.replaceId);
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                replaceInfo = TradeHelper.GetReplaceInfo(this.replaceId);
                if (orderInfo == null || orderInfo.UserId != HiContext.Current.User.UserId)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
                    return;
                }
                this.BindOrderItems(orderInfo);
                this.BindRefunds(replaceInfo);
            }

            PageTitle.AddSiteNameTitle("换货申请单");

            WAPHeadName.AddHeadName("换货申请单");
        }
        private void BindRefunds(ReplaceInfo replaceInfo)
        {
            this.RefundDetails.DataSource = replaceInfo.ReplaceLineItem;
            this.RefundDetails.DataBind();
        }
        private void BindOrderItems(OrderInfo order)
        {
            this.litTotalPrice.Money = order.GetTotal();
            this.litWeight.Text = order.Weight.ToString("F2");
            this.txtOrderId.Text = order.OrderId;
            this.litUsername.Text = order.ShipTo;
            this.litShippingRegion.Text =order.ShippingRegion+ order.Address;
            this.litZipCode.Text = order.ZipCode;
            this.litEmailAddress.Text = order.EmailAddress;
            this.litCellPhone.Text = order.CellPhone;
            this.litTelPhone.Text = order.TelPhone;
            this.litShipToDate.Text = order.ShipToDate;
        }
        private void BindOrderReplace(int replaceId)
        {
            DataTable replaceApplysTable = TradeHelper.GetReplaceApplysTable(replaceId);
            if (replaceApplysTable.Rows[0]["HandleStatus"] != System.DBNull.Value)
            {
                if ((int)replaceApplysTable.Rows[0]["HandleStatus"] == 0)
                {
                    this.handleStatus.Text = "待处理";
                }
                else
                {
                    if ((int)replaceApplysTable.Rows[0]["HandleStatus"] == 1)
                    {
                        this.handleStatus.Text = "已完成";
                    }
                    else if ((int)replaceApplysTable.Rows[0]["HandleStatus"] == 2)
                    {
                        this.handleStatus.Text = "已拒绝";
                    }
                    else
                    {
                        this.handleStatus.Text = "已受理";
                    }
                }
            }
            if (replaceApplysTable.Rows[0]["ApplyForTime"] != System.DBNull.Value)
            {
                this.litAddDate.Time = (System.DateTime)replaceApplysTable.Rows[0]["ApplyForTime"];
            }
            if (replaceApplysTable.Rows[0]["Comments"] != System.DBNull.Value)
            {
                this.litRemark.Text = replaceApplysTable.Rows[0]["Comments"].ToString();
            }
            if (replaceApplysTable.Rows[0]["AdminRemark"] != System.DBNull.Value)
            {
                this.litAdminRemark.Text = replaceApplysTable.Rows[0]["AdminRemark"].ToString();
            }
            if (replaceApplysTable.Rows[0]["OrderId"] != System.DBNull.Value)
            {
                this.orderId = replaceApplysTable.Rows[0]["OrderId"].ToString();
            }
        }
        private ReplaceApplyQuery GetReplaceQuery()
        {
            ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            replaceApplyQuery.SortBy = "ApplyForTime";
            return replaceApplyQuery;
        }
    }
}
