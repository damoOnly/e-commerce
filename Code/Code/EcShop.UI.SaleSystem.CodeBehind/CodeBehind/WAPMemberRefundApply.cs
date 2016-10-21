using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class WAPMemberRefundApply : WAPMemberTemplatedWebControl
    {
        private WapTemplatedRepeater rptApply;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberRefundApply.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("退款单");
            this.rptApply = (WapTemplatedRepeater)this.FindControl("rptApply");
            BindRefund();
        }
        private void BindRefund()
        {
            RefundApplyQuery refundQuery = this.GetRefundQuery();
            refundQuery.UserId = new int?(HiContext.Current.User.UserId);
            //DbQueryResult refundApplys = TradeHelper.GetRefundApplys(refundQuery);
            //this.rptApply.DataSource = refundApplys.Data;

            TradeHelper.GetRefundApplys(refundQuery);
            this.rptApply.DataSource = TradeHelper.NewGetRefundApplys(refundQuery);
            this.rptApply.DataBind();
            
            //this.pager.TotalRecords = refundApplys.TotalRecords;
            //this.txtOrderId.Value = refundQuery.OrderId;
            //this.handleStatus.SelectedIndex = 0;
            //if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
            //{
            //    this.handleStatus.Value = refundQuery.HandleStatus.Value.ToString();
            //}
        }
        private RefundApplyQuery GetRefundQuery()
        {
            RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
                {
                    refundApplyQuery.HandleStatus = new int?(num);
                }
            }
            refundApplyQuery.PageIndex = 1;
            refundApplyQuery.PageSize = 100;
            refundApplyQuery.SortBy = "ApplyForTime";
            refundApplyQuery.SortOrder = SortAction.Desc;
            return refundApplyQuery;
        }
    }
}

