using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.POManage)]
    public class POItemList : AdminPage
    {
        protected System.Web.UI.WebControls.Repeater rpPOItem;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.TextBox txtProductsName;
        protected System.Web.UI.WebControls.TextBox txtBarCode;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidPOId;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidSupplierId;
        protected System.Web.UI.WebControls.Label lblTotal;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected Pager pager;

        private string productsName;
        private string barCode;
        /// <summary>
        /// POId
        /// </summary>
        public int POId;

        public int SupplierId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            int tempid=0;
            if (!int.TryParse(this.Page.Request.QueryString["Id"], out tempid))
            {
                base.GotoResourceNotFound();
                return;
            }
            int tempSupplierId = 0;
            int.TryParse(this.Page.Request.QueryString["SupplierId"], out tempSupplierId);

            this.POId = tempid;
            this.SupplierId = tempSupplierId;

            hidPOId.Value = POId.ToString();
            hidSupplierId.Value = SupplierId.ToString();

            if (!this.Page.IsPostBack)
            {
                this.BindProducts("CreateTime", SortAction.Desc);
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        #region 绑定数据时更改显示状态
        /// <summary>
        /// 绑定数据时更改显示状态 订单状态（0：创建；1：招商确认；2：关务认领；3：关务申报；4：申报成功；5：申报失败；6：已入库）
        /// </summary>
        protected void rpPOItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblIsSample = (Label)e.Item.FindControl("lblIsSample");
            if (lblIsSample != null)
            {
                lblIsSample.Text = lblIsSample.Text == "True" ? "是" : "否";
            }
        }
        #endregion

        #region 初始化绑定商品数据
        /// <summary>
        /// 初始化绑定商品数据
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        private void BindProducts(string sortBy, SortAction sortOrder)
        {
            this.LoadParameters();
            PurchaseOrderItemQuery purchaseOrderItemQuery = new PurchaseOrderItemQuery
            {
                POId = this.POId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = sortOrder,
                SortBy = sortBy,
                ProductName = this.productsName,
                BarCode = this.barCode,
            };


            Globals.EntityCoding(purchaseOrderItemQuery, true);
            DbQueryResult purchaseOrder = PurchaseOrderHelper.GetPurchaseOrderItemList(purchaseOrderItemQuery);
            this.rpPOItem.DataSource = purchaseOrder.Data;
            this.rpPOItem.DataBind();
            //获取采购单总数量，采购单总金额（最上面或者最底下加整个明细的加总）
            DataSet ds = PurchaseOrderHelper.GetPOTotalQuyAndAmount(this.POId);
            //ExpectQuantity,OriginalCurrencyTotalPrice,TotalSalePrice
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblTotal.Text = string.Format("合计：采购单总数量：{0};    采购单原币总金额：{1};    采购单销售总金额：{2};", ds.Tables[0].Rows[0][0], ds.Tables[0].Rows[0][1], ds.Tables[0].Rows[0][2]);
            }
            this.pager1.TotalRecords = (this.pager.TotalRecords = purchaseOrder.TotalRecords);
        }
        #endregion

        #region 查询商品数据
        /// <summary>
        /// 查询商品数据
        /// </summary>
        /// <param name="isSearch"></param>
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("productsName", Globals.UrlEncode(this.txtProductsName.Text.Trim()));
            nameValueCollection.Add("barCode", Globals.UrlEncode(this.txtBarCode.Text.Trim()));

            nameValueCollection.Add("Id", POId.ToString());
            nameValueCollection.Add("SupplierId", SupplierId.ToString());

            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
           
            base.ReloadPage(nameValueCollection, false);
        }
        #endregion

        #region 加载查询参数
        /// <summary>
        /// 加载查询参数
        /// </summary>
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productsName"]))
            {
                this.productsName = Globals.UrlDecode(this.Page.Request.QueryString["productsName"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["barCode"]))
            {
                this.barCode = Globals.UrlDecode(this.Page.Request.QueryString["barCode"]);
            }

            this.txtProductsName.Text = this.productsName;
            this.txtBarCode.Text = this.barCode;
        }
        #endregion

        protected void rpPOItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (e.CommandName == "del")
            {
                int num = System.Convert.ToInt32(e.CommandArgument);
                if (!PurchaseOrderHelper.DeletetItem(num, member.UserId))
                {
                    this.ShowMsg("该单状态不允许操作或您无权操作该单", false);
                    return;
                }
                this.ReloadProductOnSales(true);
            }
        }
    }
}
