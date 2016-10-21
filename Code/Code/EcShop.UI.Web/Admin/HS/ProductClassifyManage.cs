using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace EcShop.UI.Web.Admin.HS
{
    [PrivilegeCheck(Privilege.ProductClassifyView)]
    public class ProductClassifyManage : AdminPage
    {
        private string sortBy;
        private string sortOrder;
        private string productName;
        //private int? Status;
        private int? recordStatus;
        private int? checkStatus;
        private int? inspectionStatus;
        private System.DateTime? startDate;
        private System.DateTime? endDate;

        private string itemNo;
        private string brandName;
        private string hSProductName;
        private string barCode;
        private string hSCODE;
        private string batchNo;
        private string supplierCode;
        private string productRegistrationNumberS;
        private string lJNoS;

        protected System.Web.UI.WebControls.TextBox txtProductName;
        protected System.Web.UI.WebControls.TextBox txtItemNo;
        protected System.Web.UI.WebControls.TextBox txtBrandName;
        protected System.Web.UI.WebControls.TextBox txtHSProductNameQ;
        protected System.Web.UI.WebControls.TextBox txtBarCode;
        protected System.Web.UI.WebControls.TextBox txtHSCODE;
        protected System.Web.UI.WebControls.TextBox txtBatchNo;
        protected System.Web.UI.WebControls.TextBox txtProductRegistrationNumberS;
        protected System.Web.UI.WebControls.TextBox txtLJNoS;

        //protected System.Web.UI.WebControls.DropDownList ddlStatus;
        protected System.Web.UI.WebControls.DropDownList RecordList;
        protected System.Web.UI.WebControls.DropDownList CheckList;
        protected System.Web.UI.WebControls.DropDownList InspectionList;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnRegistration;
        protected System.Web.UI.WebControls.Button btnAddBatch;
        protected System.Web.UI.WebControls.Button btnExportData;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected Grid grdProductClassify;
        protected Pager pager;

        protected HiddenField hidSortBy;
        protected HiddenField hidSortOrder;

        ////////归类信息////////
        protected System.Web.UI.WebControls.TextBox txtHSProductName;
        protected System.Web.UI.WebControls.TextBox txtHSProductBrand;
        protected System.Web.UI.WebControls.TextBox txtProductItemNo;
        protected System.Web.UI.WebControls.TextBox txtProductRegistrationNumber;
        protected System.Web.UI.WebControls.TextBox txtBatch;
        protected System.Web.UI.WebControls.TextBox txtLJNo;
        
        protected HiddenField hidHSCodeId;
        protected HiddenField hidXYHSCodeId;
        protected HiddenField hidProductId;
        protected HiddenField hidSkuId;
        protected HiddenField hidIsApprovedPrice;


        private int? IsApprovedPrice;

        protected System.Web.UI.WebControls.DropDownList dropIsApprovedPrice;
        /// <summary>
        /// 计量单位
        /// </summary>
        protected UnitDropDownList ddlUnit;
        protected System.Web.UI.WebControls.TextBox txtSupplierCode;

        /// <summary>
        /// 自动报关字段
        /// </summary>
        protected System.Web.UI.WebControls.TextBox coustomSkuType;
        protected System.Web.UI.WebControls.TextBox countrySku;
        protected System.Web.UI.WebControls.DropDownList beLookType;
        protected System.Web.UI.WebControls.TextBox madeOf;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnRegistration.Click += new System.EventHandler(this.btnRegistration_Click);
            this.btnAddBatch.Click += new System.EventHandler(this.btnAddBatch_Click);
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);

            this.grdProductClassify.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProductClassify_RowDataBound);
            if (!this.Page.IsPostBack)
            {
                this.IsApprovedPrice = 1;
                this.ddlUnit.DataBind();
                if (string.IsNullOrWhiteSpace(this.sortBy))
                {
                    this.BindProducts("AddedDate", SortAction.Desc);
                }
                else
                {
                    this.grdProductClassify.SortOrder = this.sortOrder;
                    this.grdProductClassify.SortOrderBy = this.sortBy;
                    this.hidSortOrder.Value = this.sortOrder;
                    this.hidSortBy.Value = this.sortBy;
                    this.BindProducts(this.sortBy, this.sortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                }
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }


        public string ProductImg(string isprove, string productid, string imgpath)
        {
            string resultstr = string.Empty;
            if (isprove == "True")
            {
                resultstr = "<a href='../../ProductDetails.aspx?productId=" + productid + "' target=\"_blank\"><img src='" + imgpath + "' style=\"border-width:0px;\"></a>";
            }
            else
            {
                resultstr = "<img src='" + imgpath + "' style=\"border-width:0px;\">";
            }
            return resultstr;
        }

        public string ProductDetails(string isprove, string productid, string productname)
        {
            string resultstr = string.Empty;
            if (isprove == "True")
            {
                resultstr = "<a href='../../ProductDetails.aspx?productId=" + productid + "' target=\"_blank\">" + productname + "</a>";
            }
            else
            {
                resultstr = "" + productname + "";
            }
            return resultstr;
        }
        #region 绑定数据时更改显示状态
        /// <summary>
        /// 绑定数据时更改显示状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdProductClassify_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                #region 老代码
                //System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litStatus");
                //if (literal.Text == "0")
                //{
                //    literal.Text = "<font color='red'>1.未归类</font>";
                //}
                //else if (literal.Text == "1")
                //{
                //    literal.Text = "<font color='blue'>2.未备案</font>";
                //}
                //else if (literal.Text == "2")
                //{
                //    literal.Text = "<font color='green'>3.已备案</font>";
                //}
                //else if (literal.Text == "3")
                //{
                //    literal.Text = "<font color='green'>4.备案失败</font>";
                //}
                //else if (literal.Text == "4")
                //{
                //    literal.Text = "<font color='green'>5.未校验</font>";
                //}
                //else if (literal.Text == "5")
                //{
                //    literal.Text = "<font color='green'>6.已校验</font>";
                //}
                //else if (literal.Text == "6")
                //{
                //    literal.Text = "<font color='green'>7.校验失败</font>";
                //}
                //else if (literal.Text == "7")
                //{
                //    literal.Text = "<font color='green'>8.未商检</font>";
                //}
                //else if (literal.Text == "8")
                //{
                //    literal.Text = "<font color='green'>9.已商检</font>";
                //}
                //else if (literal.Text == "9")
                //{
                //    literal.Text = "<font color='green'>10.商检失败</font>";
                //}
                //else if (literal.Text == "3")
                //{
                //    literal.Text = "<font color='green'>4.未校验</font>";
                //}
                //else if (literal.Text == "4")
                //{
                //    literal.Text = "<font color='green'>5.已校验</font>";
                //}
                //else if (literal.Text == "5")
                //{
                //    literal.Text = "<font color='green'>6.未商检</font>";
                //}
                //else if (literal.Text == "6")
                //{
                //    literal.Text = "<font color='green'>7.已商检</font>";
                //}
                #endregion

                // edit by lt 2016-3-24  状态分为3类 
                // 备案状态
                System.Web.UI.WebControls.Literal reliteral = (System.Web.UI.WebControls.Literal)e.Row.FindControl("ReStatus");
                switch (reliteral.Text)
                {
                    case "0":
                        reliteral.Text = "<font color='red'>未归类</font>";
                        break;
                    case "1":
                        reliteral.Text = "<font color='blue'>未备案</font>";
                        break;
                    case "2":
                        reliteral.Text = "<font color='green'>已备案</font>";
                        break;
                    case "3":
                        reliteral.Text = "<font color='green'>备案失败</font>";
                        break;
                    default:
                        reliteral.Text = "<font color='green'>已备案</font>";
                        break;
                }

                // 校验状态
                System.Web.UI.WebControls.Literal chliteral = (System.Web.UI.WebControls.Literal)e.Row.FindControl("CheckStatus");
                switch (chliteral.Text)
                {
                    case "0":
                        chliteral.Text = "<font color='green'>未校验</font>";
                        break;
                    case "1":
                        chliteral.Text = "<font color='green'>已校验</font>";
                        break;
                    case "2":
                        chliteral.Text = "<font color='green'>校验失败</font>";
                        break;
                    default:
                        chliteral.Text = "<font color='green'>未校验</font>";
                        break;
                }


                // 商检状态
                System.Web.UI.WebControls.Literal Inliteral = (System.Web.UI.WebControls.Literal)e.Row.FindControl("InspectionStaus");
                switch (Inliteral.Text)
                {
                    case "0":
                        Inliteral.Text = "<font color='green'>未商检</font>";
                        break;
                    case "1":
                        Inliteral.Text = "<font color='green'>已商检</font>";
                        break;
                    case "2":
                        Inliteral.Text = "<font color='green'>商检失败</font>";
                        break;
                    default:
                        Inliteral.Text = "<font color='green'>未商检</font>";
                        break;
                }
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
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = this.productName,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = sortOrder,
                SortBy = sortBy,
                StartDate = this.startDate,
                EndDate = this.endDate,
                //IsApproved = this.Status,
                RecordStatus = this.recordStatus,
                CheckStatus = this.checkStatus,
                InspectionStaus = this.inspectionStatus,
                ItemNo = this.itemNo,
                BrandName = this.brandName,
                HSProductName = this.hSProductName,
                BarCode = this.barCode,
                HSCODE = this.hSCODE,
                BatchNo = this.batchNo,
                IsApprovedPrice=this.IsApprovedPrice,
                SupplierCode = this.supplierCode,
                ProductRegistrationNumber = this.productRegistrationNumberS,
                LJNo = this.lJNoS
            };

            productQuery.SaleType = 1;

            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProductClassifyList(productQuery);
            this.grdProductClassify.DataSource = products.Data;
            this.grdProductClassify.DataBind();

            this.txtProductName.Text = productQuery.Keywords;

            this.txtItemNo.Text = productQuery.ItemNo;
            this.txtBrandName.Text = productQuery.BrandName;
            this.txtHSProductNameQ.Text = productQuery.HSProductName;
            this.txtBarCode.Text = productQuery.BarCode;
            this.txtHSCODE.Text = productQuery.HSCODE;
            this.txtBatchNo.Text = productQuery.BatchNo;
            this.txtSupplierCode.Text = productQuery.SupplierCode;

            this.txtProductRegistrationNumberS.Text = productQuery.ProductRegistrationNumber;
            this.txtLJNoS.Text = productQuery.LJNo;


            //if (productQuery.IsApproved.HasValue)
            //{
            //    this.ddlStatus.SelectedValue = productQuery.IsApproved.Value.ToString();
            //}

            if (productQuery.RecordStatus.HasValue)
            {
                this.RecordList.SelectedValue = productQuery.RecordStatus.Value.ToString();
            }

            if (productQuery.CheckStatus.HasValue)
            {
                this.CheckList.SelectedValue = productQuery.CheckStatus.Value.ToString();
            }

            if (productQuery.InspectionStaus.HasValue)
            {
                this.InspectionList.SelectedValue = productQuery.InspectionStaus.Value.ToString();
            }

            

            if (productQuery.IsApprovedPrice.HasValue)
            {
                this.dropIsApprovedPrice.SelectedValue = productQuery.IsApprovedPrice.Value.ToString();
            }

            this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
            //this.pager.TotalRecords = products.TotalRecords;
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
            nameValueCollection.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));

            nameValueCollection.Add("itemNo", Globals.UrlEncode(this.txtItemNo.Text.Trim()));
            nameValueCollection.Add("brandName", Globals.UrlEncode(this.txtBrandName.Text.Trim()));
            nameValueCollection.Add("hSProductName", Globals.UrlEncode(this.txtHSProductNameQ.Text.Trim()));
            nameValueCollection.Add("barCode", Globals.UrlEncode(this.txtBarCode.Text.Trim()));
            nameValueCollection.Add("hSCODE", Globals.UrlEncode(this.txtHSCODE.Text.Trim()));
            nameValueCollection.Add("batchNo", Globals.UrlEncode(this.txtBatchNo.Text.Trim()));
            nameValueCollection.Add("supplierCode", Globals.UrlEncode(this.txtSupplierCode.Text.Trim()));

            nameValueCollection.Add("productRegistrationNumberS", Globals.UrlEncode(this.txtProductRegistrationNumberS.Text.Trim()));
            nameValueCollection.Add("lJNoS", Globals.UrlEncode(this.txtLJNoS.Text.Trim()));

            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }

            //if (this.ddlStatus.SelectedValue != "-1")
            //{
            //    nameValueCollection.Add("IsApproved", this.ddlStatus.SelectedValue);
            //}

            if (this.RecordList.SelectedValue != "-1")
            {
                nameValueCollection.Add("recordStatus", this.RecordList.SelectedValue);
            }
            if (this.CheckList.SelectedValue != "-1")
            {
                nameValueCollection.Add("checkStatus", this.CheckList.SelectedValue);
            }
            if (this.InspectionList.SelectedValue != "-1")
            {
                nameValueCollection.Add("inspectionStatus", this.InspectionList.SelectedValue);
            }

            if (this.dropIsApprovedPrice.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsApprovedPrice", this.dropIsApprovedPrice.SelectedValue);
            }

            nameValueCollection.Add("sortBy", this.hidSortBy.Value);// this.grdProducts.SortOrderBy);
            nameValueCollection.Add("sortOrder", this.hidSortOrder.Value);// this.grdProducts.SortOrder);
            // 这里好像没有用到这个字段
            nameValueCollection.Add("SaleStatus", this.RecordList.SelectedValue.ToString());
            base.ReloadPage(nameValueCollection, false);
        }
        #endregion

        #region 加载查询参数
        /// <summary>
        /// 加载查询参数
        /// </summary>
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["itemNo"]))
            {
                this.itemNo = Globals.UrlDecode(this.Page.Request.QueryString["itemNo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["brandName"]))
            {
                this.brandName = Globals.UrlDecode(this.Page.Request.QueryString["brandName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["hSProductName"]))
            {
                this.hSProductName = Globals.UrlDecode(this.Page.Request.QueryString["hSProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["barCode"]))
            {
                this.barCode = Globals.UrlDecode(this.Page.Request.QueryString["barCode"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["hSCODE"]))
            {
                this.hSCODE = Globals.UrlDecode(this.Page.Request.QueryString["hSCODE"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["batchNo"]))
            {
                this.batchNo = Globals.UrlDecode(this.Page.Request.QueryString["batchNo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["supplierCode"]))
            {
                this.supplierCode = Globals.UrlDecode(this.Page.Request.QueryString["supplierCode"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productRegistrationNumberS"]))
            {
                this.productRegistrationNumberS = Globals.UrlDecode(this.Page.Request.QueryString["productRegistrationNumberS"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lJNoS"]))
            {
                this.lJNoS = Globals.UrlDecode(this.Page.Request.QueryString["lJNoS"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            //if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApproved"]))//
            //{
            //    int tmpIsApproved = 0;
            //    if (int.TryParse(this.Page.Request.QueryString["IsApproved"], out tmpIsApproved))
            //    {
            //        this.Status = new Int32?(tmpIsApproved);
            //    }
            //}

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["recordStatus"]))//
            {
                int tmpIsApproved = 0;
                if (int.TryParse(this.Page.Request.QueryString["recordStatus"], out tmpIsApproved))
                {
                    this.recordStatus = new Int32?(tmpIsApproved);
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["checkStatus"]))//
            {
                int tmpIsApproved = 0;
                if (int.TryParse(this.Page.Request.QueryString["checkStatus"], out tmpIsApproved))
                {
                    this.checkStatus = new Int32?(tmpIsApproved);
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["inspectionStatus"]))//
            {
                int tmpIsApproved = 0;
                if (int.TryParse(this.Page.Request.QueryString["inspectionStatus"], out tmpIsApproved))
                {
                    this.inspectionStatus = new Int32?(tmpIsApproved);
                }
            }


            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                this.sortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                this.sortOrder = this.Page.Request.QueryString["sortOrder"];
            }


            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApprovedPrice"]))//是否审价过滤
            {
                int tmpIsApprovedPrice = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsApprovedPrice"], out tmpIsApprovedPrice))
                {
                    this.IsApprovedPrice = new Int32?(tmpIsApprovedPrice);
                }
            }

            this.txtProductName.Text = this.productName;
            this.txtItemNo.Text = this.itemNo;
            this.txtBrandName.Text = this.brandName;
            this.txtHSProductNameQ.Text = this.hSProductName;
            this.txtBarCode.Text = this.barCode;
            this.txtHSCODE.Text = this.hSCODE;
            this.txtBatchNo.Text = this.batchNo;
            this.txtSupplierCode.Text = this.supplierCode;

            this.txtProductRegistrationNumberS.Text = this.productRegistrationNumberS;
            this.txtLJNoS.Text = this.lJNoS;
            

            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
            //this.ddlStatus.SelectedValue = this.Status.ToString();
            this.RecordList.SelectedValue = this.recordStatus.ToString();
            this.CheckList.SelectedValue = this.checkStatus.ToString();
            this.InspectionList.SelectedValue = this.inspectionStatus.ToString();
        }
        #endregion

        #region 归类
        /// <summary>
        /// 归类
        /// </summary>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int ProductId;
            int HSCodeId;
            int XYHSCodeId;
            string SkuId = this.hidSkuId.Value;

            if (!int.TryParse(this.hidProductId.Value, out ProductId))
            {
                this.ShowMsg("请选择商品。", false);
                return;
            }

            if (string.IsNullOrEmpty(SkuId))
            {
                this.ShowMsg("请选择商品。", false);
                return;
            }

            if (this.hidIsApprovedPrice.Value.ToString()!="1")
            {
                this.ShowMsg("商品还未审价通过", false);
                return;
            }

            if (!int.TryParse(this.hidHSCodeId.Value, out HSCodeId))
            {
                this.ShowMsg("请输入海关编码。", false);
                return;
            }

            if (!int.TryParse(this.hidXYHSCodeId.Value, out XYHSCodeId))
            {
                this.ShowMsg("请输入行邮编码。", false);
                return;
            }
            this.ClearHiddenField();
            //
            if (this.txtHSProductName.Text.Trim() == "")
            {
                this.ShowMsg("请输入报关名称。", false);
                return;
            }

            if (this.txtHSProductBrand.Text.Trim() == "")
            {
                this.ShowMsg("请输入报关品牌。", false);
                return;
            }
            if (ddlUnit.SelectedItem.Text == ddlUnit.NullToDisplay)
            {
                this.ShowMsg("请选择报关单位。", false);
                return;
            }
            if (this.txtProductItemNo.Text.Trim() == "")
            {
                this.ShowMsg("请输入报关型号。", false);
                return;
            }

            if (txtLJNo.Text.Trim() == "")
            {
                this.ShowMsg("请输入料件号。", false);
                return;
            }

            if (coustomSkuType.Text.Trim() == "")
            {
                this.ShowMsg("请输入海关规格型号。", false);
                return;
            }


            if (countrySku.Text.Trim() == "")
            {
                this.ShowMsg("请输入国检规格型号。", false);
                return;
            }

            if (beLookType.SelectedIndex <-1)
            {
                this.ShowMsg("请输入监管类别。", false);
                return;
            }

            if (madeOf.Text.Trim() == "")
            {
                this.ShowMsg("请输入主要成分。", false);
                return;
            }
            string HSProductName = txtHSProductName.Text.Trim();
            string HSProductBrand = txtHSProductBrand.Text.Trim();
            string ProductItemNo = txtProductItemNo.Text.Trim();
            string Unit = ddlUnit.SelectedItem.Text == ddlUnit.NullToDisplay ? "" : ddlUnit.SelectedItem.Text;
            string UnitCode = ddlUnit.SelectedValue;
            string LJNo = txtLJNo.Text.Trim();

            string coustomSkuTypeValue = coustomSkuType.Text.Trim();
            string countrySkuValue = countrySku.Text.Trim();
            string beLookTypeValue = beLookType.SelectedItem.Text;
            string madeOfValue = madeOf.Text.Trim();
            /*计算海关关税 存储过程中计算
            --进口商品
             1：海关编码临时关税不为空，则取海关临时税率（TEMP_IN_RATE）
             2：海关编码临时关税为空，则查看产品对应的国家是否为最惠国，如果是则取最低税率(LOW_RATE)，否则取最高税率（HIGH_RATE）。
            --出口商品
              1：海关编码临时出口关税不为空，则取临时关税（TEMP_OUT_RATE），如果为空则取（OUT_RATE）
             */

            StringBuilder ElmentsValue = new StringBuilder();
            string ElmentsId = Request["hidElmentsId"];
            //判断ElmentsId是否为空
            if (!string.IsNullOrEmpty(ElmentsId))
            {
                string[] ArrElmentsId = ElmentsId.Split(',');
                foreach (string item in ArrElmentsId)
                {
                    //检查是否存在{@1@}或者{@2@}
                    if (this.Page.Request["elments_" + item].IndexOf("{@1@}") >= 0 || this.Page.Request["elments_" + item].IndexOf("{@2@}") >= 0)
                    {
                        this.ShowMsg("输入申报要素中存在非法字符串‘{@1@}’或‘{@2@}’，请重新输入。", false);
                        return;
                    }
                    ElmentsValue.AppendFormat("{0}^{1}&", item
                        , this.Page.Request["elments_" + item].Replace("^", "{@1@}").Replace("&", "{@2@}").Replace("＾", "{@1@}").Replace("＆", "{@2@}"));
                }
            }
            ProductInfo productInfo = new ProductInfo
            {
                ProductId = ProductId,
                TaxRateId = XYHSCodeId,
                HSCodeId = HSCodeId,
                HSProductName = HSProductName,
                HSBrand = HSProductBrand,
                HSItemNo = ProductItemNo,
                HSUnit = Unit,
                HSUnitCode = UnitCode,
                coustomSkuType = coustomSkuTypeValue,
                countrySku=countrySkuValue,
                beLookType=beLookTypeValue,
                madeOf=madeOfValue
            };
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                return;
            }

            DataSet ds = ProductHelper.ProductClassify(productInfo, ElmentsValue.ToString().TrimEnd('&'), member.UserId, member.Username, LJNo, SkuId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    this.ShowMsg("保存成功。", true);//这里要换成刷新页面的
                else
                    this.ShowMsg("保存失败。", false);
            }
            else
            {
                this.ShowMsg("保存失败。", false);
            }
            this.BindProducts("AddedDate", SortAction.Desc);
        }
        #endregion

        /// <summary>
        /// 备案
        /// </summary>
        private void btnRegistration_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hidSkuId.Value))
            {
                this.ShowMsg("请选择商品。", false);
                return;
            }


            if (this.hidIsApprovedPrice.Value.ToString() != "1")
            {
                this.ShowMsg("商品还未审价通过", false);
                return;
            }
            
            if (ProductHelper.Registration(txtProductRegistrationNumber.Text.Trim(), this.hidSkuId.Value,"2"))
            {
                this.ShowMsg("备案成功！", true);
            }
            else
            {
                ProductHelper.Registration(txtProductRegistrationNumber.Text.Trim(), this.hidSkuId.Value, "3");
                this.ShowMsg("备案失败！", false);
            }
            this.ClearHiddenField();
            this.BindProducts("AddedDate", SortAction.Desc);
        }

        /// <summary>
        /// 备案批次
        /// </summary>
        private void btnAddBatch_Click(object sender, System.EventArgs e)
        {
            string strProductIds = Request["CheckBoxGroup"];
            if (string.IsNullOrEmpty(strProductIds))
            {
                this.ShowMsg("请选择商品。", false);
                return;
            }
            this.ClearHiddenField();

            if (ProductHelper.IsExitNoApprovedPriceProduct(strProductIds))
            {
                this.ShowMsg("存在还未审价的商品。", false);
                return;
            }
            if (ProductHelper.AddBatchNo(txtBatch.Text.Trim(), strProductIds))
            {
                this.ShowMsg("操作成功！", true);
            }
            else
            {
                this.ShowMsg("操作失败！", false);
            }
            this.BindProducts("AddedDate", SortAction.Desc);
        }


        /// <summary>
        /// 清除之前选择数据
        /// </summary>
        private void ClearHiddenField()
        {
            this.hidHSCodeId.Value = "";
            this.hidProductId.Value = "";
            this.hidXYHSCodeId.Value = "";
            this.hidSkuId.Value = "";
            this.hidIsApprovedPrice.Value = "";
        }

        private void btnExportData_Click(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = this.productName,
                PageSize = 99999999,
                PageIndex = 1,
                SortOrder = SortAction.Desc,
                SortBy = "AddedDate",
                StartDate = this.startDate,
                EndDate = this.endDate,
                RecordStatus = this.recordStatus,
                CheckStatus = this.checkStatus,
                InspectionStaus = this.inspectionStatus,
                ItemNo = this.itemNo,
                BrandName = this.brandName,
                HSProductName = this.hSProductName,
                BarCode = this.barCode,
                HSCODE = this.hSCODE,
                BatchNo = this.batchNo,
                IsApprovedPrice = this.IsApprovedPrice,
                SupplierCode = this.supplierCode,
                ProductRegistrationNumber = this.productRegistrationNumberS,
                LJNo = this.lJNoS
            };

            productQuery.SaleType = 1;

            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProductClassifyList(productQuery);

            System.Data.DataTable orderGoods = products.Data as DataTable;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>产品归类备案信息</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>条形码</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>报关名称</td>");
            stringBuilder.AppendLine("<td>商品品牌</td>");
            stringBuilder.AppendLine("<td>报关品牌</td>");
            stringBuilder.AppendLine("<td>商品单位</td>");
            stringBuilder.AppendLine("<td>报关单位</td>");
            stringBuilder.AppendLine("<td>商品型号</td>");
            stringBuilder.AppendLine("<td>报关型号</td>");
            stringBuilder.AppendLine("<td>料件号</td>");
            stringBuilder.AppendLine("<td>备案号</td>");
            stringBuilder.AppendLine("<td>原产地</td>");
            stringBuilder.AppendLine("<td>海关编码</td>");

            stringBuilder.AppendLine("<td>行邮编码</td>");
            stringBuilder.AppendLine("<td>成本价</td>");
            stringBuilder.AppendLine("<td>成分</td>");
            stringBuilder.AppendLine("<td>生产厂家</td>");
            stringBuilder.AppendLine("<td>备案批次</td>");
            stringBuilder.AppendLine("<td>供应商编号</td>");
            stringBuilder.AppendLine("<td>税率</td>");
            stringBuilder.AppendLine("<td>商品添加时间</td>");
            stringBuilder.AppendLine("<td>备案状态</td>");
            stringBuilder.AppendLine("<td>商品规格</td>");
            stringBuilder.AppendLine("<td>审价状态</td>");
            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in orderGoods.Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["BarCode"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["HSProductName"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["BrandName"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HSBrand"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Unit"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HSUnit"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["ItemNo"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HSItemNo"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["LJNo"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["ProductRegistrationNumber"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["EnArea"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HS_CODE"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["PersonalPostalArticlesCode"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["CostPrice"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Ingredient"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Manufacturer"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["BatchNo"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SupplierCode"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["TaxRate"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["AddedDate"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["PStatus"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductStandard"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["IsApprovedPrice"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=产品归类备案信息.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }

        /// <summary>
        /// 把双引号替换为&#34;，单引号替换为&#39;。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ReplaceStr(string str)
        {
            return str.Replace("\"", "&#34;").Replace("'", "&#39;");
        }
    }
}
