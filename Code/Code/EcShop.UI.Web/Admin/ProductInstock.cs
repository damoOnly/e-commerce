using EcShop.ControlPanel.Store;
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
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
     [PrivilegeCheck(Privilege.Products)]
    public class ProductInstock : AdminPage
    {
        private string sortBy;
        private string sortOrder;
        private string productName;
        private string productCode;
        private int? categoryId;
        private int? tagId;
        private int? typeId;
        private int? importSourceId;
        private int? supplierId;
        private int? IsApproved;
        private ProductSaleStatus saleStatus = ProductSaleStatus.All;
        private System.DateTime? startDate;
        private System.DateTime? endDate;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected ProductCategoriesDropDownList dropCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductTagsDropDownList dropTagList;
        protected ProductTypeDownList dropType;
        protected System.Web.UI.WebControls.TextBox txtSKU;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected ImageLinkButton btnDelete;
        protected ImageLinkButton btnApprove;//审核按钮
        protected ImageLinkButton btnUnApprove;//弃审核按钮
        protected ImageLinkButton btnGenerationQCode;
        protected ImportSourceTypeDropDownList ddlImportSourceType;
        protected SupplierDropDownList ddlSupplier;

        protected SaleStatusDropDownList dropSaleStatus;
        protected Grid grdProducts;
        protected Pager pager;
        protected ProductTagsLiteral litralProductTag;
        protected System.Web.UI.WebControls.TextBox txtReferralDeduct;
        protected System.Web.UI.WebControls.Literal litReferralDeduct;
        protected System.Web.UI.WebControls.TextBox txtSubMemberDeduct;
        protected System.Web.UI.WebControls.Literal litSubMemberDeduct;
        protected System.Web.UI.WebControls.TextBox txtSubReferralDeduct;
        protected System.Web.UI.WebControls.Literal litSubReferralDeduct;
        protected System.Web.UI.WebControls.Button btnUpdateProductTags;
        protected System.Web.UI.WebControls.Button btnUpdateProductDeducts;
        protected TrimTextBox txtProductTag;
        protected System.Web.UI.WebControls.Button btnInStock;
        protected System.Web.UI.WebControls.Button btnUnSale;
        protected System.Web.UI.WebControls.Button btnUpSale;
        protected System.Web.UI.WebControls.Button btnSetFreeShip;
        protected System.Web.UI.WebControls.Button btnCancelFreeShip;
        protected System.Web.UI.WebControls.DropDownList dropIsApproved;
        protected ShippingTemplatesDropDownList ddlShipping;
        protected System.Web.UI.WebControls.LinkButton btnExcelSupplierProDetatil;
        protected HiddenField hidSortBy;
        protected HiddenField hidSortOrder;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);//审核操作
            this.btnUnApprove.Click += new System.EventHandler(this.btnUnApprove_Click);//
            this.btnExcelSupplierProDetatil.Click += new System.EventHandler(this.btnExcelSupplierProDetatil_Click);

            this.btnGenerationQCode.Click += new System.EventHandler(this.btnGenerationQCode_Click);

            this.btnUpSale.Click += new System.EventHandler(this.btnUpSale_Click);
            this.btnUnSale.Click += new System.EventHandler(this.btnUnSale_Click);
            this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
            this.btnCancelFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
            this.btnSetFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
            this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
            this.btnUpdateProductDeducts.Click += new System.EventHandler(this.btnUpdateProductDeducts_Click);
            this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
            this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
            grdProducts.Sorting += grdProducts_Sorting;
            this.dropSaleStatus.SelectedIndexChanged += new System.EventHandler(this.dropSaleStatus_SelectedIndexChanged);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.dropTagList.DataBind();
                this.dropType.DataBind();
                this.dropSaleStatus.DataBind();
                this.ddlImportSourceType.DataBind();
                this.ddlSupplier.DataBind();
                this.ddlShipping.DataBind();

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
                {
                    this.sortBy = this.Page.Request.QueryString["sortBy"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
                {
                    this.sortOrder = this.Page.Request.QueryString["sortOrder"];
                }
                if (string.IsNullOrWhiteSpace(this.sortBy))
                {
                    this.BindProducts("DisplaySequence", SortAction.Desc);
                }
                else
                {
                    this.grdProducts.SortOrder = this.sortOrder;
                    this.grdProducts.SortOrderBy = this.sortBy;
                    this.hidSortOrder.Value = this.sortOrder;
                    this.hidSortBy.Value = this.sortBy;
                    this.BindProducts(this.sortBy, this.sortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                }
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                this.litReferralDeduct.Text = "（全站统一比例：" + siteSettings.ReferralDeduct.ToString("F2") + " %）";
                this.litSubMemberDeduct.Text = "（全站统一比例：" + siteSettings.SubMemberDeduct.ToString("F2") + " %）";
                this.litSubReferralDeduct.Text = "（全站统一比例：" + siteSettings.SubReferralDeduct.ToString("F2") + " %）";
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        void grdProducts_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sort = e.SortExpression;
            SortAction sortOrder = grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc;
            this.sortBy = sort;
            this.sortOrder = grdProducts.SortOrder;
            this.hidSortOrder.Value = this.sortOrder;
            this.hidSortBy.Value = this.sortBy;
            this.ReloadProductOnSales(true);
            //绑定排序数据
            //this.BindProducts(sort, sortOrder);
        }
        private void btnUpdateProductDeducts_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要更新推广分佣的商品", false);
                return;
            }
            decimal referralDeduct = 0m;
            decimal subMemberDeduct = 0m;
            decimal subReferralDeduct = 0m;
            if (string.IsNullOrEmpty(this.txtReferralDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入直接推广佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtReferralDeduct.Text.Trim(), out referralDeduct))
            {
                this.ShowMsg("您输入的直接推广佣金格式不对！", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtSubMemberDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入下级会员佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtSubMemberDeduct.Text, out subMemberDeduct))
            {
                this.ShowMsg("您输入的下级会员佣金格式不正确！", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtSubReferralDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入下级推广员佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtSubReferralDeduct.Text, out subReferralDeduct))
            {
                this.ShowMsg("您输入的下级推广员佣金格式不正确！", false);
                return;
            }
            if (ProductHelper.UpdateProductReferralDeduct(text, referralDeduct, subMemberDeduct, subReferralDeduct))
            {
                this.ShowMsg("成功更新了商品的推广分佣！", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("更新商品的推广分佣失败！", false);
        }
        private void btnSetFreeShip_Click(object sender, System.EventArgs e)
        {
            bool flag = ((System.Web.UI.WebControls.Button)sender).ID == "btnSetFreeShip";
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要设置为包邮的商品", false);
                return;
            }
            int num = ProductHelper.SetFreeShip(text, flag);
            if (num > 0)
            {
                this.ShowMsg("成功" + (flag ? "设置" : "取消") + "了商品包邮状态", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg((flag ? "设置" : "取消") + "商品包邮状态失败，未知错误", false);
        }
        private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }
        private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
                System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
                if (literal.Text == "1")
                {
                    literal.Text = "出售中";
                }
                else
                {
                    if (literal.Text == "2")
                    {
                        literal.Text = "下架区";
                    }
                    else
                    {
                        literal.Text = "仓库中";
                    }
                }
                if (string.IsNullOrEmpty(literal2.Text))
                {
                    literal2.Text = "-";
                }
            }
        }
        private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            string text = this.grdProducts.DataKeys[e.RowIndex].Value.ToString();
            if (text != "")
            {
                list.Add(System.Convert.ToInt32(text));
            }
            if (ProductHelper.RemoveProduct(text) > 0)
            {
                this.ShowMsg("删除商品成功", true);
                this.ReloadProductOnSales(false);
            }
        }
        private void btnUpSale_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要上架的商品", false);
                return;
            }
            int num = ProductHelper.UpShelf(text);
            if (num > 0)
            {
                this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("上架商品失败，未知错误", false);
        }
        private void btnUnSale_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要下架的商品", false);
                return;
            }
            int num = ProductHelper.OffShelf(text);
            if (num > 0)
            {
                this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("下架商品失败，未知错误", false);
        }
        private void btnInStock_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要入库的商品", false);
                return;
            }
            int num = ProductHelper.InStock(text);
            if (num > 0)
            {
                this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("入库商品失败，未知错误", false);
        }
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要删除的商品", false);
                return;
            }
            int num = ProductHelper.RemoveProduct(text);
            if (num > 0)
            {
                this.ShowMsg("成功删除了选择的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("删除商品失败，未知错误", false);
        }
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要审核的商品", false);
                return;
            }
            bool ret = ProductHelper.ApproveProducts(text, 1);
            if (ret)
            {
                this.ShowMsg("成功审核了选择的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("审核商品失败，未知错误", false);
        }
        private void btnUnApprove_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要弃审的商品", false);
                return;
            }
            bool ret = ProductHelper.ApproveProducts(text, 0);
            if (ret)
            {
                this.ShowMsg("成功弃审了选择的商品", true);
                this.BindProducts(grdProducts.SortOrderBy, grdProducts.SortOrder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc);
                return;
            }
            this.ShowMsg("弃审商品失败，未知错误", false);
        }



        /// <summary>
        /// 生成打印任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerationQCode_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要生成二维码的商品", false);
                return;
            }
            string[] ArrProductId = text.Split(',');
            string QRcode = string.Empty;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string proid in ArrProductId)
            {
                QRcode = CreateQrcode("ProductId=" + proid, proid);
                dic.Add(proid, QRcode);
            }
            bool num = ProductHelper.UpdateQRcode(dic);
            if (num)
            {
                this.ShowMsg("成功生成二维码", true);
                this.ReloadProductOnSales(false);
            }
            else
            {
                this.ShowMsg("生成失败，未知错误", false);
            }
        }


        private void btnUpdateProductTags_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要关联的商品", false);
                return;
            }
            System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
            if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
            {
                string text2 = this.txtProductTag.Text.Trim();
                string[] array;
                if (text2.Contains(","))
                {
                    array = text2.Split(new char[]
					{
						','
					});
                }
                else
                {
                    array = new string[]
					{
						text2
					};
                }
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string value = array2[i];
                    list.Add(System.Convert.ToInt32(value));
                }
            }
            string[] array3;
            if (text.Contains(","))
            {
                array3 = text.Split(new char[]
				{
					','
				});
            }
            else
            {
                array3 = new string[]
				{
					text
				};
            }
            int num = 0;
            string[] array4 = array3;
            for (int j = 0; j < array4.Length; j++)
            {
                string value2 = array4[j];
                ProductHelper.DeleteProductTags(System.Convert.ToInt32(value2), null);
                if (list.Count > 0 && ProductHelper.AddProductTags(System.Convert.ToInt32(value2), list, null))
                {
                    num++;
                }
            }
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.SubjectProducts, string.Format(CultureInfo.InvariantCulture, "已成功修改了{0}件商品的商品标签", new object[]
			    {
				     num,
				}));
                this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
            }
            else
            {
                this.ShowMsg("已成功取消了商品的关联商品标签", true);
            }
            this.txtProductTag.Text = "";
        }
        private void BindProducts(string sortBy, SortAction sortOrder)
        {
            this.LoadParameters();
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = sortOrder,
                SortBy = sortBy,
                StartDate = this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.typeId,
                SaleStatus = this.saleStatus,
                EndDate = this.endDate,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                SupplierId = this.ddlSupplier.SelectedValue.HasValue ? this.ddlSupplier.SelectedValue : null,
                TemplateId = this.ddlShipping.SelectedValue.HasValue ? this.ddlShipping.SelectedValue : null,
                IsApproved = this.IsApproved
            };
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProductsAdmin(productQuery);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            // this.grdProducts.SortDirection = SortDirection.Ascending;
            this.txtSearchText.Text = productQuery.Keywords;
            this.txtSKU.Text = productQuery.ProductCode;
            this.dropCategories.SelectedValue = productQuery.CategoryId;
            this.dropType.SelectedValue = productQuery.TypeId;
            this.ddlImportSourceType.SelectedValue = productQuery.ImportSourceId;
            this.ddlSupplier.SelectedValue = productQuery.SupplierId;
            this.ddlShipping.SelectedValue = productQuery.TemplateId;
            if (productQuery.IsApproved.HasValue)
            {
                this.dropIsApproved.SelectedValue = productQuery.IsApproved.Value.ToString();
            }
            this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
        }
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
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
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            if (this.dropTagList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
            }
            if (this.ddlImportSourceType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("importSourceId", this.ddlImportSourceType.SelectedValue.ToString());
            }
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.ToString());
            }
            if (this.ddlShipping.SelectedValue.HasValue)
            {
                nameValueCollection.Add("templateId", this.ddlShipping.SelectedValue.ToString());
            }
            if (this.dropIsApproved.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsApproved", this.dropIsApproved.SelectedValue);
            }
            //if (!isSearch)
            //{
            //    nameValueCollection.Add("sortBy",this.hidSortBy.Value);// this.grdProducts.SortOrderBy);
            //    nameValueCollection.Add("sortOrder",this.hidSortOrder.Value);// this.grdProducts.SortOrder);
            //}
            nameValueCollection.Add("sortBy", this.hidSortBy.Value);// this.grdProducts.SortOrderBy);
            nameValueCollection.Add("sortOrder", this.hidSortOrder.Value);// this.grdProducts.SortOrder);
            nameValueCollection.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
            base.ReloadPage(nameValueCollection);
        }
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            int value = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
            {
                this.categoryId = new int?(value);
            }
            int value2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
            {
                this.dropBrandList.SelectedValue = new int?(value2);
            }
            int value3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
            {
                this.tagId = new int?(value3);
            }
            int value4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
            {
                this.typeId = new int?(value4);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
            {
                this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApproved"]))//是否审核过滤
            {
                int tmpIsApproved = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsApproved"], out tmpIsApproved))
                {
                    this.IsApproved = new Int32?(tmpIsApproved);
                }
            }
            int iSourceId = 0;
            if (int.TryParse(this.Page.Request.QueryString["importSourceId"], out iSourceId))
            {
                this.importSourceId = new int?(iSourceId);
            }

            int iSupplier = 0;
            if (int.TryParse(this.Page.Request.QueryString["supplierId"], out iSupplier))
            {
                this.supplierId = new int?(iSupplier);
            }
            int templateId = 0;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["templateId"]))
            {
                int.TryParse(this.Page.Request.QueryString["templateId"], out templateId);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                this.sortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                this.sortOrder = this.Page.Request.QueryString["sortOrder"];
            }
            //this.ShowMsg(this.sortBy + " " + this.sortOrder,false);
            this.txtSearchText.Text = this.productName;
            this.txtSKU.Text = this.productCode;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;

            this.ddlImportSourceType.DataBind();
            this.ddlImportSourceType.SelectedValue = this.importSourceId;

            this.ddlSupplier.DataBind();
            this.ddlSupplier.SelectedValue = this.supplierId;

            this.ddlShipping.DataBind();
            this.ddlShipping.SelectedValue = templateId;

            this.dropTagList.DataBind();
            this.dropTagList.SelectedValue = this.tagId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
            this.dropType.SelectedValue = this.typeId;
            this.dropSaleStatus.SelectedValue = this.saleStatus;


        }

        /// <summary>
        /// 根据条件，导出供货商商品明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelSupplierProDetatil_Click(object sender, System.EventArgs e)
        {
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : null,
                ProductCode = !string.IsNullOrEmpty(txtSKU.Text) ? txtSKU.Text : null,
                CategoryId = this.dropCategories.SelectedValue.HasValue ? this.dropCategories.SelectedValue : null,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this.calendarStartDate.SelectedDate.HasValue ? this.calendarStartDate.SelectedDate : null,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.dropType.SelectedValue.HasValue ? this.dropType.SelectedValue : null,
                SaleStatus = this.dropSaleStatus.SelectedValue,
                EndDate = this.calendarEndDate.SelectedDate.HasValue ? this.calendarEndDate.SelectedDate : null,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                SupplierId = this.ddlSupplier.SelectedValue.HasValue ? this.ddlSupplier.SelectedValue : null,
                TemplateId = this.ddlShipping.SelectedValue.HasValue ? this.ddlShipping.SelectedValue : null,
                IsApproved = (!string.IsNullOrEmpty(this.dropIsApproved.SelectedValue) && this.dropIsApproved.SelectedValue != "-1") ? int.Parse(this.dropIsApproved.SelectedValue) : -1
            };

            if (this.dropCategories.SelectedValue.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.dropCategories.SelectedValue.Value).Path;
            }
            Globals.EntityCoding(productQuery, true);
            DataSet supplierProductDetails = ProductHelper.GetSupplierProductDetail(productQuery);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>供货商商品明细</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>序号</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品编码</td>");
            stringBuilder.AppendLine("<td>货号</td>");
            stringBuilder.AppendLine("<td>品牌</td>");
            stringBuilder.AppendLine("<td>产地</td>");
            stringBuilder.AppendLine("<td>库存</td>");
            stringBuilder.AppendLine("<td>供应商名称</td>");
            stringBuilder.AppendLine("<td>供货价格</td>");
            stringBuilder.AppendLine("<td>销售价格</td>");
            stringBuilder.AppendLine("<td>在线情况</td>");

            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in supplierProductDetails.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["productId"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductCode"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SKU"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["BrandName"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["CnArea"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["SupplierName"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["CostPrice"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["SalePrice"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["StrSaleStatus"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=供货商商品明细.xls");
            }
            else
            {
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=供货商商品明细_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".xls");
            }

            base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
    }
}
