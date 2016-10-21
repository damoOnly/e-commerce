using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.ControlPanel.Utility;
using Promotions;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SupplierProductPromotions)]
    public class SupplierProductPromotions : AdminPage
    {
        public bool isWholesale;
        protected System.Web.UI.WebControls.Literal litTite;
        protected System.Web.UI.WebControls.Literal litDec;
        protected System.Web.UI.WebControls.HyperLink hlinkAddPromotion;
        protected Grid grdPromoteSales;

        protected PageSize hrefPageSize;
        protected Pager pager;
        /// <summary>
        /// 开始时间
        /// </summary>
        protected WebCalendar calendarStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        protected WebCalendar calendarEnd;
        protected Button btnQueryBalanceDetails;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
            this.LoadParameters();
            if (this.isWholesale)
            {
                this.litTite.Text = "批发规则";
                this.litDec.Text = "针对部分商品满足一定数量时的打折促销，您可以添加新的批发促销活动或管理当前的批发促销活动";
                this.hlinkAddPromotion.Text = "添加新的批发规则";
                this.hlinkAddPromotion.NavigateUrl = "AddProductPromotion.aspx?isWholesale=true";
            }
            this.grdPromoteSales.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdPromoteSales_RowDataBound);
            this.grdPromoteSales.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPromoteSales_RowDeleting);
            this.btnQueryBalanceDetails.Click += btnQueryBalanceDetails_Click;
            if (!this.Page.IsPostBack)
            {
                this.BindProductPromotions();
            }
        }


        private void grdPromoteSales_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int activityId = (int)this.grdPromoteSales.DataKeys[e.RowIndex].Value;
            if (PromoteHelper.DeletePromotion(activityId))
            {
                this.ShowMsg("成功删除了选择的促销活动", true);
                this.BindProductPromotions();
                return;
            }
            this.ShowMsg("删除失败", false);
        }
        private void BindProductPromotions()
        {
            ProductPromotionsQuery giftQuery = new ProductPromotionsQuery();
            giftQuery.Page.PageSize = this.pager.PageSize;
            giftQuery.Page.PageIndex = this.pager.PageIndex;
            giftQuery.Page.SortOrder = SortAction.Desc;
            giftQuery.IsPromotion = true;
            giftQuery.IsWholesale = this.isWholesale;
            giftQuery.BeginTime = this.calendarStart.Text;
            giftQuery.EndTime = this.calendarEnd.Text;
            giftQuery.SupplierId = UserHelper.GetAssociatedSupplierId(HiContext.Current.User.UserId);
            int count;
            if (!string.IsNullOrWhiteSpace(giftQuery.EndTime))
            {
                giftQuery.EndTime += " 23:59:59";
            }
            this.grdPromoteSales.DataSource = PromoteHelper.GetPromotions(giftQuery, out count);
            this.grdPromoteSales.DataBind();
            this.pager.TotalRecords = count;
        }
        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            this.ReloadGiftsList(true);
        }
        private void ReloadGiftsList(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("endTime", this.calendarEnd.Text.Trim());
            nameValueCollection.Add("beginTime", this.calendarStart.Text);
            nameValueCollection.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
            nameValueCollection.Add("isWholesale", this.isWholesale.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(nameValueCollection);
        }
        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                bool bol;
                if (bool.TryParse(this.Request.QueryString["isWholesale"], out bol))
                {
                    this.isWholesale = bol;
                }
                if (!string.IsNullOrWhiteSpace(this.Request.QueryString["beginTime"]))
                {
                    this.calendarStart.Text = this.Request.QueryString["beginTime"];
                }
                if (!string.IsNullOrWhiteSpace(this.Request.QueryString["endTime"]))
                {
                    this.calendarEnd.Text = this.Request.QueryString["endTime"];
                }
                //int pagesize;
                //if (int.TryParse(this.Request.QueryString["pageSize"],out pagesize))
                //{
                //    this.hrefPageSize. = pagesize;
                //}
                return;
            }
            //this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
            //this.isPromotion = this.chkPromotion.Checked;
        }
        private void grdPromoteSales_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                int activityId = int.Parse(this.grdPromoteSales.DataKeys[e.Row.RowIndex].Value.ToString());
                System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblmemberGrades");
                System.Web.UI.WebControls.Label label2 = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblPromoteType");
                System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("ltrPromotionInfo");
                System.Collections.Generic.IList<MemberGradeInfo> promoteMemberGrades = PromoteHelper.GetPromoteMemberGrades(activityId);
                string text = string.Empty;
                foreach (MemberGradeInfo current in promoteMemberGrades)
                {
                    text = text + current.Name + ",";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.Remove(text.Length - 1);
                }
                label.Text = text;
                switch ((int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PromoteType"))
                {
                    case 1:
                        label2.Text = "直接打折";
                        literal.Text = string.Format("折扣值：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;
                    case 2:
                        label2.Text = "固定金额出售";
                        literal.Text = string.Format("固定金额值：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;
                    case 3:
                        label2.Text = "优惠金额出售";
                        literal.Text = string.Format("优惠金额值：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;
                    case 4:
                        label2.Text = "按批发数量打折";
                        literal.Text = string.Format("购买数量：{0} 折扣值：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        return;
                    case 5:
                        label2.Text = "买商品送礼品";
                        literal.Text = "<a href=\"javascript:DialogFrame('promotion/gifts.aspx?isPromotion=true','查看促销礼品',null,null)\">查看促销礼品</a>";
                        return;
                    case 6:
                        label2.Text = "有买有送";
                        literal.Text = string.Format("购买数量：{0} 赠送数量：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f0}"));
                        break;

                    case 7:
                        label2.Text = "单品满减";
                        literal.Text = string.Format("满足金额：{0} 减少金额：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        break;

                    case 8:
                        label2.Text = "第二件减价";
                        literal.Text = string.Format("减少金额：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        break;

                    case 9:
                        label2.Text = "第二件打折";
                        literal.Text = string.Format("折扣值：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
