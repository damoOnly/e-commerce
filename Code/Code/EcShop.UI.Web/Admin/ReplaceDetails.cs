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
    public class ReplaceDetails : AdminPage
    {
        private int returnsId;
        private ReplaceInfo replaceInfo;
        protected string orderId;
        protected System.Web.UI.WebControls.Literal litReplaceId;//退回单Id
        protected System.Web.UI.WebControls.Literal litReplaceHandleStatus;//退回单处理状态
        protected System.Web.UI.WebControls.Literal litReplaceHandleStatus1;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReplaceId"]))
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
            int.TryParse(this.Page.Request.QueryString["ReplaceId"], out returnsId);

            this.replaceInfo = OrderHelper.GetReplaceInfo(this.returnsId);
            this.orderId = replaceInfo.OrderId;
            if (this.replaceInfo == null)
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
            if (this.replaceInfo.HandleStatus == 0)
            {
                strHandleStatus = "待处理";
            }
            else if (this.replaceInfo.HandleStatus == 1)
            {
                strHandleStatus = "已完成";
            }
            else if (this.replaceInfo.HandleStatus == 3)
            {
                strHandleStatus = "已受理";
            }
            if (this.replaceInfo.HandleStatus == 2)
            {
                strHandleStatus = "已拒绝";
            }
            litReplaceHandleStatus.Text = strHandleStatus;
            litApplyTime.Text = "申请时间：" + this.replaceInfo.ApplyForTime.ToString();
            litSourceOrder.Text = this.replaceInfo.OrderId;
            this.rptItmesList.DataSource = this.replaceInfo.ReplaceLineItem;
            this.rptItmesList.DataBind();
            //litReturnsMoney.Text = this.replaceInfo.RefundMoney.ToString("0.00");
            litReplaceHandleStatus1.Text = strHandleStatus;
            litComments.Text = this.replaceInfo.Comments;
            litReceiveTime.Text = this.replaceInfo.ReceiveTime.ToString();
            litReplaceId.Text = this.replaceInfo.ReplaceId.ToString();

            litUserName.Text = this.replaceInfo.Username;
            litRealName.Text = this.replaceInfo.RealName;
            litUserTel.Text = this.replaceInfo.CellPhone;
            litUserEmail.Text = this.replaceInfo.EmailAddress;
        }
    }
}
