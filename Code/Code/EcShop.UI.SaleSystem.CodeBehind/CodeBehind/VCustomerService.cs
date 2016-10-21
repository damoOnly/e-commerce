using EcShop.UI.Common.Controls;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.SaleSystem.Shopping;
using EcShop.Entities.Orders;
using EcShop.ControlPanel.Sales;
using EcShop.Entities.Sales;
using EcShop.Entities;
using EcShop.SaleSystem.Member;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VCustomerService : VshopTemplatedWebControl
    {
        private System.Web.UI.WebControls.TextBox txtRefundRemark;
        private System.Web.UI.WebControls.TextBox txtReturnRemark;
        private System.Web.UI.WebControls.TextBox txtReplaceRemark;
        private System.Web.UI.WebControls.DropDownList dropLogisticsCompany;
        private System.Web.UI.WebControls.DropDownList dropRefundReason;
        private System.Web.UI.WebControls.DropDownList dropReturnReason;
        private VshopTemplatedRepeater rptOrderProducts;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCustomerService.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.txtRefundRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRefundRemark");
            this.txtReturnRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReturnRemark");
            this.txtReplaceRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplaceRemark");
            this.rptOrderProducts = (VshopTemplatedRepeater)this.FindControl("rptOrderProducts");
            this.dropLogisticsCompany = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropLogisticsCompany");
            this.dropRefundReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundReason");
            this.dropReturnReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnReason");
            string action = this.Page.Request.QueryString["Action"];
            string orderId = this.Page.Request.QueryString["OrderId"];

            if (action == "Refund")
            {
                PageTitle.AddSiteNameTitle("退款申请");
            }
            else if (action == "Return")
            {
                PageTitle.AddSiteNameTitle("退货申请");
            }
            else if (action == "Replace")
            {
                PageTitle.AddSiteNameTitle("退换申请");
            }
            //OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            IList<string> list = ExpressHelper.GetAllExpressName();
            List<ListItem> item = new List<ListItem>();
            foreach (string s in list)
            {
                item.Add(new ListItem(s, s));
            }
            dropLogisticsCompany.Items.AddRange(item.ToArray());
            //绑定物流公司选择
            //IList<ShippingModeInfo> shippingModes = ControlProvider.Instance().GetShippingModes();
            //dropLogisticsCompany.DataSource = shippingModes;
            //dropLogisticsCompany.DataValueField = "Name";
            //dropLogisticsCompany.DataMember = "Name";
            //dropLogisticsCompany.DataBind();
            this.rptOrderProducts.DataSource = ShoppingProcessor.GetOrderItems(orderId);
            this.rptOrderProducts.DataBind();

            BindRefundReason();
            BindReturnReason();
        }

        private void BindRefundReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Refund);
            this.dropRefundReason.DataSource = reasons;
            this.dropRefundReason.DataBind();

            this.dropRefundReason.Items.Insert(this.dropRefundReason.Items.Count, "其他原因...");
        }
        private void BindReturnReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Return);
            this.dropReturnReason.DataSource = reasons;
            this.dropReturnReason.DataBind();

            this.dropReturnReason.Items.Insert(this.dropReturnReason.Items.Count, "其他原因...");
        }
    }
}
