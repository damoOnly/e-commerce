using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.UI.ControlPanel.Utility;
using System.Data;
using Admin;
using ASPNET.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SupplierProductEdit)]
    public class SupplierEditProduct : ProductBasePage
    {
        private int productId;
        protected int categoryId;
        protected Script Script2;
        protected Script Script1;
        protected System.Web.UI.WebControls.Literal litCategoryName;
        protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
        protected ProductTypeDownList dropProductTypes;
        protected TaxRateDropDownList dropTaxRate;
        protected SupplierDropDownList ddlSupplier;
        protected ShippingTemplatesDropDownList ddlShipping;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtDisplaySequence;
        protected TrimTextBox txtProductCode;
        protected System.Web.UI.WebControls.TextBox txtEnglishName;
        protected System.Web.UI.WebControls.TextBox txtsysProductName;

        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtBuyCardinality;//购买基数
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDeductFee;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtFactStock;
        protected TrimTextBox txtWeight;
        protected TrimTextBox txtGrossWeight;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtIngredient;//成分
        protected TrimTextBox txtProductStandard;//商品规格，单规格时使用
        protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
        protected ImageUploader uploader1;
        protected ImageUploader uploader2;
        protected ImageUploader uploader3;
        protected ImageUploader uploader4;
        protected ImageUploader uploader5;
        protected TrimTextBox txtShortDescription;
        protected KindeditorControl fckDescription;
        protected KindeditorControl fckmobbileDescription;
        protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
        protected System.Web.UI.WebControls.TextBox txtReferralDeduct;
        protected System.Web.UI.WebControls.Literal litReferralDeduct;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtReferralDeductTip;
        protected System.Web.UI.WebControls.TextBox txtSubMemberDeduct;
        protected System.Web.UI.WebControls.Literal litSubMemberDeduct;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtSubMemberDeductTip;
        protected System.Web.UI.WebControls.TextBox txtSubReferralDeduct;
        protected System.Web.UI.WebControls.Literal litSubReferralDeduct;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtSubReferralDeductTip;
        protected System.Web.UI.WebControls.RadioButton radOnSales;
        protected System.Web.UI.WebControls.RadioButton radUnSales;
        protected System.Web.UI.WebControls.RadioButton radInStock;
        protected System.Web.UI.WebControls.CheckBox ChkisfreeShipping;
        protected System.Web.UI.WebControls.CheckBox ChkisCustomsClearance;//新增
        protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
        protected ProductTagsLiteral litralProductTag;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtTitle;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected System.Web.UI.WebControls.Button btnSave;
        protected ImportSourceTypeDropDownList ddlImportSourceType;

        protected System.Web.UI.WebControls.RadioButtonList Rd_Purchase;
        protected System.Web.UI.WebControls.TextBox txtSectionDay;
        protected System.Web.UI.WebControls.TextBox txtMaxCount;
        protected System.Web.UI.HtmlControls.HtmlGenericControl li_EditPurchase;
        protected System.Web.UI.WebControls.HiddenField hid_Checked;

        /// <summary>
        /// 提升权重值
        /// </summary>
        protected TrimTextBox txtAdminFraction;
        /// <summary>
        /// 权重值
        /// </summary>
        protected TrimTextBox txtFraction;

        /// <summary>
        /// 计量单位
        /// </summary>
        protected UnitDropDownList ddlUnit;
        protected TrimTextBox txtManufacturer;
        protected TrimTextBox txtItemNo;
        protected TrimTextBox txtBarCode;
        /// <summary>
        /// 只有一个规格时的备案编辑号 
        /// </summary>
        protected System.Web.UI.WebControls.HiddenField hidProductRegistrationNumber;

        protected System.Web.UI.WebControls.HiddenField hidLJNo;

        protected System.Web.UI.WebControls.HiddenField ArrhidProductRegistrationNumber;
        protected TrimTextBox txtConversionRelation;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
            string text = (base.Request.UrlReferrer == null) ? "" : base.Request.UrlReferrer.PathAndQuery.ToString();
            if (!this.Page.IsPostBack || text.ToLower().IndexOf("selectcategory.aspx") > -1)
            {
                System.Collections.Generic.IList<int> list = null;
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs;
                ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out attrs, out list);
                if (productDetails == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                {
                    this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                    this.ViewState["ProductCategoryId"] = this.categoryId;
                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    this.litCategoryName.Text = CatalogHelper.GetFullCategory(productDetails.CategoryId);
                    this.ViewState["ProductCategoryId"] = productDetails.CategoryId;
                    this.categoryId = productDetails.CategoryId;
                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + productDetails.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                System.Web.UI.WebControls.HyperLink expr_185 = this.lnkEditCategory;
                expr_185.NavigateUrl = expr_185.NavigateUrl + "&productId=" + productDetails.ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                this.litralProductTag.SelectedValue = list;
                if (list.Count > 0)
                {
                    foreach (int current in list)
                    {
                        TrimTextBox expr_1DC = this.txtProductTag;
                        expr_1DC.Text = expr_1DC.Text + current.ToString() + ",";
                    }
                    this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                }
                this.dropProductTypes.DataBind();
                this.dropBrandCategories.DataBind();
                //if (categoryId > 0)
                //{
                //   this.dropTaxRate.DataBind(categoryId);
                //}
                //else
                //{
                this.dropTaxRate.DataBind();
                //}
                this.ddlSupplier.DataBind();

                this.ddlShipping.DataBind();

                ddlImportSourceType.DataBind();
                this.ddlUnit.DataBind();

                this.LoadProduct(productDetails, attrs);
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                this.litReferralDeduct.Text = "（全站统一比例：" + siteSettings.ReferralDeduct.ToString("F2") + " %）";
                this.litSubMemberDeduct.Text = "（全站统一比例：" + siteSettings.SubMemberDeduct.ToString("F2") + " %）";
                this.litSubReferralDeduct.Text = "（全站统一比例：" + siteSettings.SubReferralDeduct.ToString("F2") + " %）";
            }
        }



        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.categoryId == 0)
            {
                this.categoryId = (int)this.ViewState["ProductCategoryId"];
            }
            int displaySequence;
            decimal salePrice;
            decimal? costPrice;
            decimal? marketPrice;
            int stock;
            int factstock;
            decimal? num3;
            decimal? referralDeduct;
            decimal? subMemberDeduct;
            decimal? subReferralDeduct;
            decimal? deductFee;
            int buyCardinality;
            decimal? grossweight;
            if (!this.ValidateConverts(this.chkSkuEnabled.Checked, out displaySequence, out salePrice, out costPrice, out marketPrice, out stock, out factstock, out num3, out referralDeduct, out subMemberDeduct, out subReferralDeduct, out deductFee, out buyCardinality, out grossweight))
            {
                return;
            }
            decimal adminFraction = 0m;
            if (string.IsNullOrWhiteSpace(txtAdminFraction.Text) || !decimal.TryParse(txtAdminFraction.Text, out adminFraction))
            {
                this.ShowMsg("请输入正确的提升权重值", false);
                return;
            }
            int ConversionRelation;
            if (!int.TryParse(this.txtConversionRelation.Text, out ConversionRelation))
            {
                this.ShowMsg("输入换算关系不正确", false);
                return;
            }
            string text = Globals.StripScriptTags(this.txtProductName.Text.Trim());
            text = Globals.StripHtmlXmlTags(text).Replace("\\", "").Replace("'", "");
            if (string.IsNullOrEmpty(text) || text == "")
            {
                this.ShowMsg("商品名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
                return;
            }
            if (!this.chkSkuEnabled.Checked)
            {
                if (salePrice <= 0m)
                {
                    this.ShowMsg("商品一口价必须大于0", false);
                    return;
                }
                if (costPrice.HasValue && (costPrice.Value > salePrice || costPrice.Value < 0m))
                {
                    this.ShowMsg("商品成本价必须大于0且小于商品一口价", false);
                    return;
                }
                if (!costPrice.HasValue || !deductFee.HasValue)
                {
                    this.ShowMsg("商品成本价与扣点不能为空", false);
                    return;
                }
                if (string.IsNullOrEmpty(txtProductStandard.Text))
                {
                    this.ShowMsg("未开启多规格时，商品规格必填", false);
                    return;
                }
            }
            //判断是否存在广告关键字
            Dictionary<string, string> msg;
            if (!ValidateKeyWordHelper.ValidateKeyWord(new Dictionary<string, string>() { { "商品名称", this.txtProductName.Text }, { "商品简介", this.txtShortDescription.Text } }, out msg))
            {
                string showM = "";
                foreach (string k in msg.Keys)
                {
                    showM += k + "中不能包含广告词:" + msg[k] + ";";
                }
                this.ShowMsg(showM, false);
                return;
            }
            string text2 = this.fckDescription.Text;
            string text3 = this.fckmobbileDescription.Text;
            if (this.ckbIsDownPic.Checked)
            {
                text2 = base.DownRemotePic(text2);
                text3 = base.DownRemotePic(text3);
            }

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<script[^>]*?>.*?</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            ProductInfo productInfo = ProductHelper.GetProductDetails(this.productId);

            if (productInfo.ConversionRelation != ConversionRelation)
            {
                if (productInfo.Stock > 0)
                {
                    this.ShowMsg("销售库存大于0不允许修改换算系数", false);
                    return;
                }
            }

            productInfo.ProductId = this.productId;
            productInfo.CategoryId = this.categoryId;
            productInfo.TypeId = this.dropProductTypes.SelectedValue;
            productInfo.ProductName = text;
            productInfo.EnglishName = this.txtEnglishName.Text;
            productInfo.ProductCode = this.txtProductCode.Text;
            productInfo.SysProductName = this.txtsysProductName.Text;
            productInfo.AdminFraction = adminFraction;
            productInfo.DisplaySequence = displaySequence;
            productInfo.MarketPrice = marketPrice;
            productInfo.Unit = this.ddlUnit.SelectedItem.Text;
            productInfo.ImageUrl1 = this.uploader1.UploadedImageUrl;
            productInfo.ImageUrl2 = this.uploader2.UploadedImageUrl;
            productInfo.ImageUrl3 = this.uploader3.UploadedImageUrl;
            productInfo.ImageUrl4 = this.uploader4.UploadedImageUrl;
            productInfo.ImageUrl5 = this.uploader5.UploadedImageUrl;
            productInfo.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
            productInfo.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
            productInfo.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
            productInfo.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
            productInfo.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
            productInfo.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
            productInfo.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
            productInfo.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
            productInfo.ShortDescription = this.txtShortDescription.Text;
            productInfo.IsfreeShipping = this.ChkisfreeShipping.Checked;
            productInfo.IsCustomsClearance = this.ChkisCustomsClearance.Checked;
            productInfo.Description = (!string.IsNullOrEmpty(text2) && text2.Length > 0) ? regex.Replace(text2, "") : null;
            productInfo.MobblieDescription = (!string.IsNullOrEmpty(text3) && text3.Length > 0) ? regex.Replace(text3, "") : null;
            productInfo.Title = this.txtTitle.Text;
            productInfo.MetaDescription = this.txtMetaDescription.Text;
            productInfo.MetaKeywords = this.txtMetaKeywords.Text;
            productInfo.AddedDate = System.DateTime.Now;
            productInfo.BrandId = this.dropBrandCategories.SelectedValue;
            productInfo.ReferralDeduct = referralDeduct;
            productInfo.SubMemberDeduct = subMemberDeduct;
            productInfo.SubReferralDeduct = subReferralDeduct;
            productInfo.TaxRateId = this.dropTaxRate.SelectedValue;
            productInfo.TemplateId = this.ddlShipping.SelectedValue;
            productInfo.SupplierId = this.ddlSupplier.SelectedValue;
            productInfo.ImportSourceId = this.ddlImportSourceType.SelectedValue;
            productInfo.BuyCardinality = buyCardinality;
            productInfo.UnitCode = this.ddlUnit.SelectedValue;
            productInfo.Manufacturer = txtManufacturer.Text;
            productInfo.ItemNo = txtItemNo.Text;
            productInfo.BarCode = txtBarCode.Text;
            productInfo.Ingredient = txtIngredient.Text;
            productInfo.ProductStandard = txtProductStandard.Text;
            productInfo.ConversionRelation = ConversionRelation;

            productInfo.Purchase = int.Parse(this.Rd_Purchase.SelectedValue);
            productInfo.SectionDay = int.Parse(this.txtSectionDay.Text);
            productInfo.PurchaseMaxNum = int.Parse(this.txtMaxCount.Text.ToString());

            ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
            if (this.radInStock.Checked)
            {
                saleStatus = ProductSaleStatus.OnStock;
            }
            if (this.radUnSales.Checked)
            {
                saleStatus = ProductSaleStatus.UnSale;
            }
            if (this.radOnSales.Checked)
            {
                saleStatus = ProductSaleStatus.OnSale;

                if (productInfo.SaleStatus != ProductSaleStatus.OnSale && ProductHelper.IsExitNoClassifyProduct(this.productId.ToString()))
                {
                    this.ShowMsg("商品还未完成归类操作,不能出售", false);
                    return;
                }
            }
            productInfo.SaleStatus = saleStatus;
            CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
            if (category != null)
            {
                productInfo.MainCategoryPath = category.Path + "|";
            }
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
            System.Collections.Generic.Dictionary<string, SKUItem> dictionary;
            if (this.chkSkuEnabled.Checked)
            {
                productInfo.HasSKU = true;
                dictionary = base.GetSkus(this.txtSkus.Text);

                if (!string.IsNullOrEmpty(ArrhidProductRegistrationNumber.Value))
                {

                    Dictionary<string, string>[] dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>[]>(ArrhidProductRegistrationNumber.Value);

                    foreach (string item in dictionary.Keys)
                    {
                        foreach (Dictionary<string, string> d in dic)
                        {
                            if (d["SKU"] == dictionary[item].SKU)
                            {
                                dictionary[item].ProductRegistrationNumber = d["ProductRegistrationNumber"];
                                dictionary[item].LJNo = d["LJNo"];
                                int wmsstock;
                                int.TryParse(d["WMSStock"], out wmsstock);
                                dictionary[item].WMSStock = wmsstock;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                AutoCalcCostPriceAndDeductFee(salePrice, ref costPrice, ref deductFee);
                dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>
				{

					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = Globals.HtmlEncode(Globals.StripScriptTags(this.txtSku.Text.Trim()).Replace("\\", "")),
							SalePrice = salePrice,
							CostPrice = costPrice.HasValue ? costPrice.Value : 0m,
							Stock = stock,
                            FactStock = factstock,
							Weight = num3.HasValue ? num3.Value : 0m,
                            DeductFee = deductFee.HasValue ? deductFee.Value : 0m,
                            ProductRegistrationNumber = hidProductRegistrationNumber.Value,
                            LJNo = hidLJNo.Value,
                            WMSStock=productInfo.DefaultSku.WMSStock
						}
					}
				};
                if (this.txtMemberPrices.Text.Length > 0)
                {
                    base.GetMemberPrices(dictionary["0"], this.txtMemberPrices.Text);
                }
            }
            if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
            {
                attrs = base.GetAttributes(this.txtAttributes.Text);
            }
            ValidationResults validationResults = Validation.Validate<ProductInfo>(productInfo);
            if (!validationResults.IsValid)
            {
                this.ShowMsg(validationResults);
                return;
            }
            System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
            if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
            {
                string text4 = this.txtProductTag.Text.Trim();
                string[] array;
                if (text4.Contains(","))
                {
                    array = text4.Split(new char[]
					{
						','
					});
                }
                else
                {
                    array = new string[]
					{
						text4
					};
                }
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string value = array2[i];
                    list.Add(System.Convert.ToInt32(value));
                }
            }
            ProductActionStatus productActionStatus = ProductHelper.UpdateProduct(productInfo, dictionary, attrs, list);
            if (productActionStatus == ProductActionStatus.Success)
            {
                this.litralProductTag.SelectedValue = list;
                this.ShowMsg("修改商品成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}&IsEdit=1", this.categoryId, productInfo.ProductId)), true);
                return;
            }
            if (productActionStatus == ProductActionStatus.AttributeError)
            {
                this.ShowMsg("修改商品失败，保存商品属性时出错", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.DuplicateName)
            {
                this.ShowMsg("修改商品失败，商品名称不能重复", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.DuplicateSKU)
            {
                this.ShowMsg("修改商品失败，商家编码不能重复", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.SKUError)
            {
                this.ShowMsg("修改商品失败，商家编码不能重复", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.ProductTagEroor)
            {
                this.ShowMsg("修改商品失败，保存商品标签时出错", false);
                return;
            }
            this.ShowMsg("修改商品失败，未知错误", false);
        }
        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int factstock, out decimal? weight, out decimal? referralDeduct, out decimal? subMemberDeduct, out decimal? subReferralDeduct, out decimal? deductFee, out int buyCardinality, out decimal? grossweight)
        {
            string text = string.Empty;
            costPrice = null;
            marketPrice = null;
            weight = null;
            grossweight = null;
            referralDeduct = null;
            subMemberDeduct = null;
            subReferralDeduct = null;
            displaySequence = (stock = 0);
            salePrice = 0m;
            factstock = 0;
            deductFee = null;
            buyCardinality = 1;
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                text += Formatter.FormatErrorMessage("请正确填写商品排序");
            }
            if (this.txtProductCode.Text.Length > 30)
            {
                text += Formatter.FormatErrorMessage("商家编码的长度不能超过30个字符");
            }
            if (this.ddlSupplier.SelectedValue == null || this.ddlSupplier.SelectedValue == 0)
            {
                text += Formatter.FormatErrorMessage("请选择供货商");
            }
            if (this.ddlUnit.SelectedValue == null || this.ddlUnit.SelectedValue == "")
            {
                text += Formatter.FormatErrorMessage("请选择计量单位");
            }
            if (this.ddlImportSourceType.SelectedValue == null || this.ddlImportSourceType.SelectedValue == 0)
            {
                text += Formatter.FormatErrorMessage("请选择原产地");
            }

            if (this.ddlShipping.SelectedValue == null || this.ddlShipping.SelectedValue == 0)
            {
                text += Formatter.FormatErrorMessage("请选择运费模版");
            }

            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                decimal value;
                if (decimal.TryParse(this.txtMarketPrice.Text, out value))
                {
                    marketPrice = new decimal?(value);
                }
                else
                {
                    text += Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (!string.IsNullOrEmpty(this.txtBuyCardinality.Text) && !int.TryParse(this.txtBuyCardinality.Text, out buyCardinality))
            {
                text += Formatter.FormatErrorMessage("请正确填写商品的购买基数");
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    text += Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal value2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out value2))
                    {
                        if (value2 >= 0)
                        {
                            costPrice = new decimal?(value2);
                        }
                    }
                    else
                    {
                        text += Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    text += Formatter.FormatErrorMessage("请正确填写商品的销售库存数量");
                }
                if (string.IsNullOrEmpty(this.txtFactStock.Text) || !int.TryParse(this.txtFactStock.Text, out factstock))
                {
                    text += Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                //if (!string.IsNullOrWhiteSpace(this.txtBuyCardinality.Text))
                //{
                //    if (int.Parse(this.txtBuyCardinality.Text) > int.Parse(this.txtFactStock.Text))
                //    {
                //        text += Formatter.FormatErrorMessage("购买基数应该小于等于商品库存");
                //    }
                //}
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    decimal value3;
                    if (decimal.TryParse(this.txtWeight.Text, out value3))
                    {
                        weight = new decimal?(value3);
                    }
                    else
                    {
                        text += Formatter.FormatErrorMessage("请正确填写商品的净重");
                    }
                }

                if (!string.IsNullOrEmpty(this.txtGrossWeight.Text))
                {
                    decimal valuegrossweight;
                    if (decimal.TryParse(this.txtGrossWeight.Text, out valuegrossweight))
                    {
                        grossweight = new decimal?(valuegrossweight);
                    }
                    else
                    {
                        text += Formatter.FormatErrorMessage("请正确填写商品的毛重");
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.txtReferralDeduct.Text))
            {
                decimal value4;
                if (decimal.TryParse(this.txtReferralDeduct.Text, out value4))
                {
                    referralDeduct = new decimal?(value4);
                }
                else
                {
                    text += Formatter.FormatErrorMessage("请正确填写直接推广佣金");
                }
            }
            if (!string.IsNullOrEmpty(this.txtSubMemberDeduct.Text))
            {
                decimal value5;
                if (decimal.TryParse(this.txtSubMemberDeduct.Text, out value5))
                {
                    subMemberDeduct = new decimal?(value5);
                }
                else
                {
                    text += Formatter.FormatErrorMessage("请正确填写下级会员佣金");
                }
            }
            if (!string.IsNullOrEmpty(this.txtSubReferralDeduct.Text))
            {
                decimal value6;
                if (decimal.TryParse(this.txtSubReferralDeduct.Text, out value6))
                {
                    subReferralDeduct = new decimal?(value6);
                }
                else
                {
                    text += Formatter.FormatErrorMessage("请正确填写下级会员佣金");
                }
            }
            if (!string.IsNullOrEmpty(this.txtDeductFee.Text))
            {
                decimal value7;
                if (decimal.TryParse(this.txtDeductFee.Text, out value7))
                {
                    if (value7 >= 0 && value7 <= 1)
                    {
                        deductFee = new decimal?(value7);
                    }
                    //else
                    //{
                    //    text += Formatter.FormatErrorMessage("扣点必须大于0小于1");
                    //}
                }
                else
                {
                    text += Formatter.FormatErrorMessage("请正确填写商品的扣点");
                }
            }


            if (!string.IsNullOrEmpty(text))
            {
                this.ShowMsg(text, false);
                return false;
            }
            return true;
        }
        private void LoadProduct(ProductInfo product, System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropTaxRate.SelectedValue = product.TaxRateId;
            this.ddlSupplier.SelectedValue = product.SupplierId;
            this.ddlSupplier.Enabled = false;
            this.ddlShipping.SelectedValue = product.TemplateId;
            this.dropBrandCategories.SelectedValue = product.BrandId;
            this.ddlImportSourceType.SelectedValue = product.ImportSourceId;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtEnglishName.Text = product.EnglishName;
            this.txtProductCode.Text = product.ProductCode.Replace(product.CategoryId.ToString(), this.categoryId.ToString());
            this.txtsysProductName.Text = product.SysProductName;
            this.txtAdminFraction.Text = product.AdminFraction.ToString();
            this.txtFraction.Text = product.Fraction.ToString();
            this.ddlUnit.SelectedValue = product.UnitCode;
            this.txtManufacturer.Text = product.Manufacturer;
            this.txtItemNo.Text = product.ItemNo;
            this.txtBarCode.Text = product.BarCode;
            this.txtConversionRelation.Text = product.ConversionRelation == 0 ? "1" : product.ConversionRelation.ToString();
            this.txtIngredient.Text = product.Ingredient;
            this.hid_Checked.Value = product.Purchase.ToString();
            this.Rd_Purchase.SelectedValue = product.Purchase.ToString();
            this.txtSectionDay.Text = product.SectionDay.ToString();
            this.txtMaxCount.Text = product.PurchaseMaxNum.ToString();

            if (!string.IsNullOrEmpty(product.ProductStandard))
            {
                this.txtProductStandard.Text = product.ProductStandard;
            }
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            if (product.ReferralDeduct.HasValue)
            {
                this.txtReferralDeduct.Text = product.ReferralDeduct.Value.ToString("F2");
            }
            if (product.SubMemberDeduct.HasValue)
            {
                this.txtSubMemberDeduct.Text = product.SubMemberDeduct.Value.ToString("F2");
            }
            if (product.SubReferralDeduct.HasValue)
            {
                this.txtSubReferralDeduct.Text = product.SubReferralDeduct.Value.ToString("F2");
            }
            this.txtShortDescription.Text = product.ShortDescription;
            this.ChkisfreeShipping.Checked = product.IsfreeShipping;
            this.ChkisCustomsClearance.Checked = product.IsCustomsClearance;
            this.fckDescription.Text = product.Description;
            this.fckmobbileDescription.Text = product.MobblieDescription;
            this.txtTitle.Text = product.Title;
            this.txtMetaDescription.Text = product.MetaDescription;
            this.txtMetaKeywords.Text = product.MetaKeywords;
            this.txtBuyCardinality.Text = product.BuyCardinality > 0 ? product.BuyCardinality.ToString() : "1";
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else
            {
                if (product.SaleStatus == ProductSaleStatus.UnSale)
                {
                    this.radUnSales.Checked = true;
                }
                else
                {
                    this.radInStock.Checked = true;
                }
            }
            this.uploader1.UploadedImageUrl = product.ImageUrl1;
            this.uploader2.UploadedImageUrl = product.ImageUrl2;
            this.uploader3.UploadedImageUrl = product.ImageUrl3;
            this.uploader4.UploadedImageUrl = product.ImageUrl4;
            this.uploader5.UploadedImageUrl = product.ImageUrl5;
            if (attrs != null && attrs.Count > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("<xml><attributes>");
                foreach (int current in attrs.Keys)
                {
                    stringBuilder.Append("<item attributeId=\"").Append(current.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(current).UsageMode).ToString()).Append("\" >");
                    foreach (int current2 in attrs[current])
                    {
                        stringBuilder.Append("<attValue valueId=\"").Append(current2.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    stringBuilder.Append("</item>");
                }
                stringBuilder.Append("</attributes></xml>");
                this.txtAttributes.Text = stringBuilder.ToString();
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
                stringBuilder2.Append("<xml><productSkus>");
                DataTable dt = CreateTable();
                foreach (string current3 in product.Skus.Keys)
                {
                    SKUItem sKUItem = product.Skus[current3];
                    string text = string.Concat(new string[]
					{
						"<item skuCode=\"",
						sKUItem.SKU,
						"\" salePrice=\"",
						sKUItem.SalePrice.ToString("F2"),
						"\" costPrice=\"",
						(sKUItem.CostPrice > 0m) ? sKUItem.CostPrice.ToString("F2") : "",
						"\" qty=\"",
						sKUItem.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        "\" factQty=\"",
						sKUItem.FactStock.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"\" weight=\"",
						(sKUItem.Weight > 0m) ? sKUItem.Weight.ToString("F2") : "",
						"\" deductFee=\"",
						(sKUItem.DeductFee > 0m) ? sKUItem.DeductFee.ToString("F2") : "",
                         "\" grossweight=\"",
						(sKUItem.GrossWeight > 0m) ? sKUItem.GrossWeight.ToString("F2") : "",
                        "\">"
					});
                    text += "<skuFields>";
                    //add by wangjun 2015年12月30日18:04:56 加备案编号隐藏记录 
                    DataRow dr = dt.NewRow();
                    dr["SKU"] = sKUItem.SKU;
                    dr["ProductRegistrationNumber"] = sKUItem.ProductRegistrationNumber;
                    dr["LJNo"] = sKUItem.LJNo;
                    dr["WMSStock"] = sKUItem.WMSStock;
                    dr["GrossWeight"] = sKUItem.GrossWeight;
                    dr["Weight"] = sKUItem.Weight;
                    dt.Rows.Add(dr);


                    foreach (int current4 in sKUItem.SkuItems.Keys)
                    {
                        string str = string.Concat(new string[]
						{
							"<sku attributeId=\"",
							current4.ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" valueId=\"",
							sKUItem.SkuItems[current4].ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" />"
						});
                        text += str;
                    }
                    text += "</skuFields>";
                    if (sKUItem.MemberPrices.Count > 0)
                    {
                        text += "<memberPrices>";
                        foreach (int current5 in sKUItem.MemberPrices.Keys)
                        {
                            text += string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", current5.ToString(System.Globalization.CultureInfo.InvariantCulture), sKUItem.MemberPrices[current5].ToString("F2"));
                        }
                        text += "</memberPrices>";
                    }
                    text += "</item>";
                    stringBuilder2.Append(text);
                }
                ArrhidProductRegistrationNumber.Value = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder2.Append("</productSkus></xml>");
                this.txtSkus.Text = stringBuilder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = ((defaultSku != null && defaultSku.SalePrice > 0m) ? defaultSku.SalePrice.ToString("F2") : "");
            this.txtCostPrice.Text = ((defaultSku != null && defaultSku.CostPrice > 0m) ? defaultSku.CostPrice.ToString("F2") : "");
            this.txtDeductFee.Text = ((defaultSku != null && defaultSku.DeductFee > 0m) ? defaultSku.DeductFee.ToString("F2") : "0");
            this.txtStock.Text = defaultSku != null ? defaultSku.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
            this.txtFactStock.Text = defaultSku != null ? defaultSku.FactStock.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
            this.txtWeight.Text = ((defaultSku != null && defaultSku.Weight > 0m) ? defaultSku.Weight.ToString("F2") : "");
            this.txtGrossWeight.Text = ((defaultSku != null && defaultSku.GrossWeight > 0m) ? defaultSku.GrossWeight.ToString("F2") : "");
            this.hidProductRegistrationNumber.Value = defaultSku.ProductRegistrationNumber;
            this.hidLJNo.Value = defaultSku.LJNo;
            if (defaultSku != null && defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int current6 in defaultSku.MemberPrices.Keys)
                {
                    TrimTextBox expr_78C = this.txtMemberPrices;
                    expr_78C.Text += string.Format("<grande id=\"{0}\" price=\"{1}\" />", current6.ToString(System.Globalization.CultureInfo.InvariantCulture), defaultSku.MemberPrices[current6].ToString("F2"));
                }
                TrimTextBox expr_7ED = this.txtMemberPrices;
                expr_7ED.Text += "</gradePrices></xml>";
            }
        }

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            DataColumn dcSKU = new DataColumn("SKU");
            DataColumn dcProductRegistrationNumber = new DataColumn("ProductRegistrationNumber");
            DataColumn dcLJNo = new DataColumn("LJNo");
            DataColumn dcWMSStock = new DataColumn("WMSStock");
            DataColumn dcGrossWeight = new DataColumn("GrossWeight");
            DataColumn dcWeight = new DataColumn("Weight");

            dt.Columns.Add(dcSKU);
            dt.Columns.Add(dcProductRegistrationNumber);
            dt.Columns.Add(dcLJNo);
            dt.Columns.Add(dcWMSStock);
            dt.Columns.Add(dcGrossWeight);
            dt.Columns.Add(dcWeight);
            return dt;
        }
    }
}
