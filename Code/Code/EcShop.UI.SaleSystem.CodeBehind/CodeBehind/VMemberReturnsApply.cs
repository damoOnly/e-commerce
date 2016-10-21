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
    public class VMemberReturnsApply : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptApply;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberReturnsApply.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("退货单");
            this.rptApply = (VshopTemplatedRepeater)this.FindControl("rptApply");
            BindReplace();
        }
        private void BindReplace()
        {
            ReturnsApplyQuery returnsQuery = this.ReturnsApplyQuery();
            returnsQuery.UserId = new int?(HiContext.Current.User.UserId);
            DbQueryResult returnsApplys = TradeHelper.GetReturnsApplys(returnsQuery);
            this.rptApply.DataSource = returnsApplys.Data;
            this.rptApply.DataBind();
            //this.pager.TotalRecords = refundApplys.TotalRecords;
            //this.txtOrderId.Value = refundQuery.OrderId;
            //this.handleStatus.SelectedIndex = 0;
            //if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
            //{
            //    this.handleStatus.Value = refundQuery.HandleStatus.Value.ToString();
            //}
        }
        private ReturnsApplyQuery ReturnsApplyQuery()
        {
            ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
                {
                    returnsApplyQuery.HandleStatus = new int?(num);
                }
            }
            returnsApplyQuery.PageIndex = 1;
            returnsApplyQuery.PageSize = 100;
            returnsApplyQuery.SortBy = "ApplyForTime";
            returnsApplyQuery.SortOrder = SortAction.Desc;
            return returnsApplyQuery;
        }
    }
}
