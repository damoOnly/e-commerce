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
    public class WAPMemberReplaceApply : WAPMemberTemplatedWebControl
    {
        private WapTemplatedRepeater rptApply;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberReplaceApply.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("退货单");
            this.rptApply = (WapTemplatedRepeater)this.FindControl("rptApply");
            BindReplace();
        }
        private void BindReplace()
        {
            ReplaceApplyQuery replaceQuery = this.ReplaceApplyQuery();
            replaceQuery.UserId = new int?(HiContext.Current.User.UserId);
            //DbQueryResult refundApplys = TradeHelper.GetReplaceApplys(replaceQuery);
            //this.rptApply.DataSource = refundApplys.Data;


            this.rptApply.DataSource = TradeHelper.NewGetReplaceApplys(replaceQuery);
            this.rptApply.DataBind();
            
        }
        private ReplaceApplyQuery ReplaceApplyQuery()
        {
            ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
                {
                    replaceApplyQuery.HandleStatus = new int?(num);
                }
            }
            replaceApplyQuery.PageIndex = 1;
            replaceApplyQuery.PageSize = 100;
            replaceApplyQuery.SortBy = "ApplyForTime";
            replaceApplyQuery.SortOrder = SortAction.Desc;
            return replaceApplyQuery;
        }
    }
}
