using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public class ReturnsDetails : AdminPage
    {
        private int returnsId;
        private ReturnsInfo returnsInfo;
        protected string orderId;
        protected System.Web.UI.WebControls.Literal litReturnsId;//退回单Id
        protected System.Web.UI.WebControls.Literal litReturnsHandleStatus;//退回单处理状态
        protected System.Web.UI.WebControls.Literal litReturnsHandleStatus1;
        protected System.Web.UI.WebControls.Literal litUserName;//会员名
        protected System.Web.UI.WebControls.Literal litRealName;//会员真实名称
        protected System.Web.UI.WebControls.Literal litUserTel;//会员电话
        protected System.Web.UI.WebControls.Literal litUserEmail;//会员邮箱
        protected System.Web.UI.WebControls.Literal litApplyTime;//申请时间
        protected System.Web.UI.WebControls.Literal litReturnsMoney;//退款金额
        protected System.Web.UI.WebControls.Literal litComments;//备注

        protected System.Web.UI.WebControls.Literal litReceiveTime;
        protected System.Web.UI.WebControls.Repeater rptItmesList;
        protected System.Web.UI.WebControls.Literal litRefundType;
        protected System.Web.UI.WebControls.Literal litSourceOrder;//关联的订单号
        protected System.Web.UI.WebControls.Literal litLogisticsCompany;//物流公司
        protected System.Web.UI.WebControls.Literal litLogisticsId;//物流单号
        protected System.Web.UI.WebControls.Literal litExpressFee;//收取快递费
        protected System.Web.UI.WebControls.Literal litFeeAffiliation;//快递费用归属
        protected System.Web.UI.WebControls.Literal litCustomsClearanceFee;//收取清关费
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReturnsId"]))
            {
                base.GotoResourceNotFound();
                return;
            }
            string key = "orderdetails-frame";
            string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", Globals.ApplicationPath + "/admin/default.html");
            ClientScriptManager clientScript = this.Page.ClientScript;
            if (!clientScript.IsClientScriptBlockRegistered(key))
            {
                clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
            }
            int.TryParse(this.Page.Request.QueryString["returnsId"], out returnsId);

            this.returnsInfo = OrderHelper.GetReturnsInfo(this.returnsId);
            this.orderId = returnsInfo.OrderId;
            if (this.returnsInfo == null)
            {
                base.Response.Write("<h3 style=\"color:red;\">退货单不存在，或者已被删除。</h3>");
                base.Response.End();
            }
            else
            {

            }
            if (!this.Page.IsPostBack)
            {
                BindReturnsData();
            }
        }
        private void BindReturnsData()
        {
            string strHandleStatus = string.Empty;
            if (this.returnsInfo.HandleStatus == 0)
            {
                strHandleStatus = "待处理";
            }
            else if (this.returnsInfo.HandleStatus == 1)
            {
                strHandleStatus = "已完成";
            }
            else if (this.returnsInfo.HandleStatus == 3)
            {
                strHandleStatus = "已受理";
            }
            if (this.returnsInfo.HandleStatus == 2)
            {
                strHandleStatus = "已拒绝";
            }
            if (this.returnsInfo.RefundType == 3)
            {
                litRefundType.Text = "原路返回";
            }
            litReturnsHandleStatus.Text = strHandleStatus;
            litApplyTime.Text = "申请时间："+this.returnsInfo.ApplyForTime.ToString();
            litSourceOrder.Text =this.returnsInfo.OrderId;
            this.rptItmesList.DataSource = this.returnsInfo.ReturnsLineItem;
            this.rptItmesList.DataBind();
            litReturnsMoney.Text = this.returnsInfo.RefundMoney.ToString("0.00");
            litReturnsHandleStatus1.Text = strHandleStatus;
            litComments.Text = this.returnsInfo.Comments;
            litReceiveTime.Text = this.returnsInfo.ReceiveTime.ToString();
            litReturnsId.Text = this.returnsInfo.ReturnsId.ToString();
            litLogisticsCompany.Text = this.returnsInfo.LogisticsCompany;
            litLogisticsId.Text = this.returnsInfo.LogisticsId;
            litExpressFee.Text=this.returnsInfo.ExpressFee.ToString("F2");
            litFeeAffiliation.Text=this.returnsInfo.FeeAffiliation;
            litCustomsClearanceFee.Text = this.returnsInfo.CustomsClearanceFee.ToString("F2");

            litUserName.Text = this.returnsInfo.Username;
            litRealName.Text = this.returnsInfo.RealName;
            litUserTel.Text = this.returnsInfo.CellPhone;
            litUserEmail.Text = this.returnsInfo.EmailAddress;

        }
    }
}
