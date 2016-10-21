using Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VMyUserPoints : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptUserPointList;

        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

        private System.Web.UI.WebControls.Literal litSumPoint;
        private System.Web.UI.WebControls.Literal litPoint;
        

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vmyuserPoints.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("积分记录");
            this.rptUserPointList = (VshopTemplatedRepeater)this.FindControl("rptUserPointList");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litSumPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litSumPoint");
            this.litPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litPoint");

            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.Page.Response.Redirect("/Vshop/Login.aspx");
            }

            int pageIndex;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
            {
                pageIndex = 1;
            }
            int pageSize;
            if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
            {
                pageSize = 8;
            }

            DbQueryResult dr = TradeHelper.GetUserPoints(member.UserId, pageIndex, pageSize);
            this.rptUserPointList.DataSource = dr.Data;
            this.rptUserPointList.DataBind();
            this.txtTotal.SetWhenIsNotNull(dr.TotalRecords.ToString());
            //points = member.Points;
            //availablePoints = member.Points;
            this.litSumPoint.SetWhenIsNotNull(member.Points.ToString());
            this.litPoint.SetWhenIsNotNull(member.Points.ToString());
        }
    }
}
