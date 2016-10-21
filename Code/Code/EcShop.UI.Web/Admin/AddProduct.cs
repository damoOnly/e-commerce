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
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.UI.ControlPanel.Utility;
using Admin;
using EcShop.Entities;
using ASPNET.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddProducts)]
    public class AddProduct : ProductBasePage
    {
        private int categoryId;
        protected Script Script2;
        protected Script Script1;
        protected System.Web.UI.WebControls.Literal litCategoryName;
        protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
        protected ProductTypeDownList dropProductTypes;
        protected TaxRateDropDownList dropTaxRate;
        protected SupplierDropDownList ddlSupplier;
        protected ShippingTemplatesDropDownList ddlShipping;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected DropDownList dropSaleType;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductCode;
        protected System.Web.UI.WebControls.TextBox txtsysProductName;
        
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDeductFee;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtFactStock;
        protected TrimTextBox txtWeight;
        protected TrimTextBox txtGrossWeight;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtBuyCardinality;//购买基数
        protected TrimTextBox txtIngredient;//成分
        protected TrimTextBox txtProductStandard;//商品规格，单规格时使用
        protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
        protected ImageUploader uploader1;
        protected ImageUploader uploader2;
        protected ImageUploader uploader3;
        protected ImageUploader uploader4;
        protected ImageUploader uploader5;
        protected TrimTextBox txtShortDescription;
        protected KindeditorControl editDescription;
        protected KindeditorControl editmobbileDescription;
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
        protected YesNoRadioButtonList radlEnableMemberDiscount;
        protected TrimTextBox txtTitle;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected System.Web.UI.WebControls.Button btnAdd;
        protected ImportSourceTypeDropDownList ddlImportSourceType;


        protected System.Web.UI.WebControls.CheckBox ChkisPromotion;


        protected System.Web.UI.WebControls.CheckBox ChkisDisplayDiscount;
        protected System.Web.UI.WebControls.TextBox txtEnglishName;
       
        protected System.Web.UI.WebControls.RadioButtonList Rd_Purchase;
        protected System.Web.UI.WebControls.TextBox txtSectionDay;
        protected System.Web.UI.WebControls.TextBox txtMaxCount;

        /// <summary>
        /// 计量单位
        /// </summary>
        protected UnitDropDownList ddlUnit;
        protected TrimTextBox txtManufacturer;
        protected TrimTextBox txtItemNo;
        protected TrimTextBox txtBarCode;

        protected TrimTextBox txtConversionRelation;

        //商品副标题
        protected TrimTextBox txtProductTitle;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true";
            if (flag)
            {
                base.DoCallback();
                return;
            }
            if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
                return;
            }
            string text = (base.Request.UrlReferrer == null) ? "" : base.Request.UrlReferrer.PathAndQuery.ToString();
            if (!this.Page.IsPostBack || text.ToLower().IndexOf("selectcategory.aspx") > -1)
            {
                this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                if (!string.IsNullOrEmpty(this.litralProductTag.Text))
                {
                    this.l_tags.Visible = true;
                }
                this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                this.dropProductTypes.DataBind();
                this.dropProductTypes.SelectedValue = category.AssociatedProductType;

             
                this.dropTaxRate.DataBind();
       
                this.dropTaxRate.SelectedValue = category.TaxRateId;

                this.ddlSupplier.DataBind();

                this.ddlShipping.DataBind();

                this.ddlImportSourceType.DataBind();
                this.dropBrandCategories.DataBind();

                this.ddlUnit.DataBind();
                //if (category.AssociatedProductType != null)
                //{
                //    this.dropBrandCategories.DataBindByTypeId(category.AssociatedProductType);
                //}
                //else
                //{
                //    this.dropBrandCategories.DataBind();
                //}                          
                this.txtProductCode.Text = "0-" + categoryId + "-" + (this.txtSku.Text = category.SKUPrefix + new System.Random(System.DateTime.Now.Millisecond).Next(1, 99999).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(5, '0'));
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                this.litReferralDeduct.Text = "（全站统一比例：" + siteSettings.ReferralDeduct.ToString("F2") + " %）";
                this.litSubMemberDeduct.Text = "（全站统一比例：" + siteSettings.SubMemberDeduct.ToString("F2") + " %）";
                this.litSubReferralDeduct.Text = "（全站统一比例：" + siteSettings.SubReferralDeduct.ToString("F2") + " %）";
            }
        }
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            int displaySequence;
            decimal salePrice;
            decimal? costPrice;
            decimal? marketPrice;
            int stock;
            int factstock;
            decimal? num4;
            decimal? referralDeduct;
            decimal? subMemberDeduct;
            decimal? subReferralDeduct;
            decimal? deductFee;
            int buyCardinality;
            decimal? grossweight;

            //销售类型
            int saletype = string.IsNullOrWhiteSpace(this.dropSaleType.SelectedValue) ? 1 : Convert.ToInt32(this.dropSaleType.SelectedValue);
            
            if (!this.ValidateConverts(this.chkSkuEnabled.Checked, out displaySequence, out salePrice, out costPrice, out marketPrice, out stock, out factstock, out num4, out referralDeduct, out subMemberDeduct, out subReferralDeduct, out deductFee, out buyCardinality,out grossweight))
            {
                return;
            }

            if (saletype != 2)
            {
                if (this.ddlImportSourceType.SelectedValue == null || this.ddlImportSourceType.SelectedValue == 0)
                {
                    this.ShowMsg("请选择原产地", false);
                    return;
                }
            }

            if (saletype != 2)
            {
                if (this.ddlSupplier.SelectedValue == null || this.ddlSupplier.SelectedValue == 0)
                {
                    this.ShowMsg("请选择供货商", false);
                    return;
                }
            }

            if (this.ddlUnit.SelectedValue == null || this.ddlUnit.SelectedValue == "")
            {
                this.ShowMsg("请选择计量单位", false);
                return;
            }
            if (this.ddlShipping.SelectedValue == null || this.ddlShipping.SelectedValue == 0)
            {
                this.ShowMsg("请选择运费模版", false);
                return;
            }
            int ConversionRelation;
            if (!int.TryParse(this.txtConversionRelation.Text, out ConversionRelation))
            {
                this.ShowMsg("输入换算关系不正确", false);
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
                if (!costPrice.HasValue)//|| !deductFee.HasValue
                {
                    this.ShowMsg("商品成本价不能为空", false);//与扣点
                    return;
                }
                if (string.IsNullOrEmpty(txtProductStandard.Text))
                {
                    this.ShowMsg("未开启多规格时，商品规格必填", false);
                    return;
                }
            }
            
            string text = Globals.StripScriptTags(this.txtProductName.Text.Trim());
            text = Globals.StripHtmlXmlTags(text).Replace("\\", "").Replace("'", "");
            if (string.IsNullOrEmpty(text) || text == "")
            {
                this.ShowMsg("商品名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
                return;
            }
            //判断是否存在广告关键字
            Dictionary<string, string> msg;
            if (!ValidateKeyWordHelper.ValidateKeyWord(new Dictionary<string, string>() { { "商品名称", this.txtProductName.Text }, { "商品简介", this.txtShortDescription.Text } }, out msg))
            {
                System.Text.StringBuilder showMsg = new System.Text.StringBuilder();
                foreach (string k in msg.Keys)
                {
                    showMsg.Append(k + "中不能包含广告词:" + msg[k]+";");
                }
                this.ShowMsg(showMsg.ToString(), false);
                return;
            }
            string text2 = this.editDescription.Text;
            string text3 = this.editmobbileDescription.Text;
            if (this.ckbIsDownPic.Checked)
            {
                text2 = base.DownRemotePic(text2);
                text3 = base.DownRemotePic(text3);
            }
           
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<script[^>]*?>.*?</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            ProductInfo productInfo = new ProductInfo
            {
                CategoryId = this.categoryId,
                TypeId = this.dropProductTypes.SelectedValue,
                ProductName = text,
                EnglishName=this.txtEnglishName.Text,
                SysProductName=this.txtsysProductName.Text.Trim(),
                ProductCode = this.txtProductCode.Text,
                MarketPrice = marketPrice,
                Unit = this.ddlUnit.SelectedItem.Text,
                ImageUrl1 = this.uploader1.UploadedImageUrl,
                ImageUrl2 = this.uploader2.UploadedImageUrl,
                ImageUrl3 = this.uploader3.UploadedImageUrl,
                ImageUrl4 = this.uploader4.UploadedImageUrl,
                ImageUrl5 = this.uploader5.UploadedImageUrl,
                ThumbnailUrl40 = this.uploader1.ThumbnailUrl40,
                ThumbnailUrl60 = this.uploader1.ThumbnailUrl60,
                ThumbnailUrl100 = this.uploader1.ThumbnailUrl100,
                ThumbnailUrl160 = this.uploader1.ThumbnailUrl160,
                ThumbnailUrl180 = this.uploader1.ThumbnailUrl180,
                ThumbnailUrl220 = this.uploader1.ThumbnailUrl220,
                ThumbnailUrl310 = this.uploader1.ThumbnailUrl310,
                ThumbnailUrl410 = this.uploader1.ThumbnailUrl410,
                ShortDescription = this.txtShortDescription.Text,
                IsCustomsClearance = this.ChkisCustomsClearance.Checked,
                Description = (!string.IsNullOrEmpty(text2) && text2.Length > 0) ? regex.Replace(text2, "") : null,
                MobblieDescription = (!string.IsNullOrEmpty(text3) && text3.Length > 0) ? regex.Replace(text3, "") : null,
                Title = this.txtTitle.Text,
                MetaDescription = this.txtMetaDescription.Text,
                MetaKeywords = this.txtMetaKeywords.Text,
                AddedDate = System.DateTime.Now,
                BrandId = this.dropBrandCategories.SelectedValue,
                MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|",
                IsfreeShipping = this.ChkisfreeShipping.Checked,
                ReferralDeduct = referralDeduct,
                SubMemberDeduct = subMemberDeduct,
                SubReferralDeduct = subReferralDeduct,
                TaxRateId = this.dropTaxRate.SelectedValue,
                TemplateId = this.ddlShipping.SelectedValue,
                SupplierId = this.ddlSupplier.SelectedValue,
                ImportSourceId = this.ddlImportSourceType.SelectedValue,
                IsApproved = true,
                BuyCardinality = buyCardinality,
                UnitCode = this.ddlUnit.SelectedValue,
                Manufacturer = txtManufacturer.Text,
                ItemNo = txtItemNo.Text,
                BarCode = txtBarCode.Text,
                Ingredient = txtIngredient.Text,
                ProductStandard = txtProductStandard.Text,
                ConversionRelation = ConversionRelation,
                ProductTitle = this.txtProductTitle.Text,
                SaleType = saletype,
                IsPromotion = this.ChkisPromotion.Checked,
                IsDisplayDiscount = this.ChkisDisplayDiscount.Checked,
                Purchase=int.Parse(this.Rd_Purchase.SelectedValue),
                SectionDay=int.Parse(this.txtSectionDay.Text),
                PurchaseMaxNum=int.Parse(this.txtMaxCount.Text.ToString())
            };
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

                if (productInfo.SaleType != 2)
                {
                    this.ShowMsg("商品还未完成归类操作,不能出售", false);
                    return;
                }
            }

            productInfo.SaleStatus = saleStatus;


            //如果是组合商品，默认已审价
            if(productInfo.SaleType==2)
            {
                productInfo.IsApprovedPrice=1;
            }

            else
            {
                 productInfo.IsApprovedPrice=0;
            }
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
            System.Collections.Generic.Dictionary<string, SKUItem> dictionary;
            if (this.chkSkuEnabled.Checked)
            {
                productInfo.HasSKU = true;
                dictionary = base.GetSkus(this.txtSkus.Text);
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
							Weight = num4.HasValue ? num4.Value : 0m,
                            DeductFee = deductFee.HasValue ? deductFee.Value :0m,
                            GrossWeight=grossweight.HasValue?grossweight.Value:0m
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
            ValidationResults validationResults = Validation.Validate<ProductInfo>(productInfo, new string[]
			{
				"AddProduct"
			});
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
            #region   ==组合商品
            List<ProductsCombination> combinations = new List<ProductsCombination>();
            string combinationInfos = base.Request.Form["selectProductsinfo"];
            string[] curCom = combinationInfos.Split(new char[]
			{
				','
			});
            string[] curCom2 = curCom;
            for (int i = 0; i < curCom2.Length; i++)
            {
                string combinationInfo = curCom2[i];
                ProductsCombination com = new ProductsCombination();
                string[] array3 = combinationInfo.Split(new char[]
				{
					'|'
				});
                if (array3.Length == 10)
                {
                    com.SkuId = array3[0];
                    com.ProductId = array3[1] == "" ? 0 : Convert.ToInt32(array3[1]);
                    com.ProductName = array3[2];
                    com.ThumbnailsUrl = array3[3];
                    decimal tempweight;
                    if (decimal.TryParse(array3[4], out tempweight))
                    {
                        com.Weight = tempweight;
                    }
                    com.SKU = array3[5];
                    com.SKUContent = array3[6];
                    com.Quantity = array3[8] == "" ? 0 : Convert.ToInt32(array3[8]);
                    decimal tempprice;
                    if (decimal.TryParse(array3[9], out tempprice))
                    {
                        com.Price = tempprice;
                    }
                    combinations.Add(com);
                }
            }
            productInfo.CombinationItemInfos = combinations;
            #endregion

            if (productInfo.SaleType == 2)
            {
                decimal CombinationTotalPrice = 0M;
                foreach (var item in productInfo.CombinationItemInfos)
                {
                    CombinationTotalPrice += item.Price * item.Quantity;
                }

                if (Math.Round(CombinationTotalPrice, 2) != Math.Round(salePrice, 2))
                {
                    this.ShowMsg("添加商品失败，组合商品一口价和组合商品明细总价不一致", false);
                    return;
                }
            }


            ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, dictionary, attrs, list, combinations);
            if (productActionStatus == ProductActionStatus.Success)
            {
                this.ShowMsg("添加商品成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", this.categoryId, productInfo.ProductId)), true);
                return;
            }
            if (productActionStatus == ProductActionStatus.AttributeError)
            {
                this.ShowMsg("添加商品失败，保存商品属性时出错", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.DuplicateName)
            {
                this.ShowMsg("添加商品失败，商品名称不能重复", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.DuplicateSKU)
            {
                this.ShowMsg("添加商品失败，商家编码不能重复", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.SKUError)
            {
                this.ShowMsg("添加商品失败，商品规格错误", false);
                return;
            }
            if (productActionStatus == ProductActionStatus.ProductTagEroor)
            {
                this.ShowMsg("添加商品失败，保存商品标签时出错", false);
                return;
            }
            this.ShowMsg("添加商品失败，未知错误", false);
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
            factstock = 0;
            salePrice = 0m;
            deductFee = null;
            buyCardinality = 1;
            if (this.txtProductCode.Text.Length > 30)
            {
                text += Formatter.FormatErrorMessage("商家编码的长度不能超过30个字符");
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
                    decimal valueGrossWeight;
                    if (decimal.TryParse(this.txtGrossWeight.Text, out valueGrossWeight))
                    {
                        grossweight = new decimal?(valueGrossWeight);
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
                    text += Formatter.FormatErrorMessage("请正确填写扣点");
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                this.ShowMsg(text, false);
                return false;
            }
            return true;
        }
    }
}
