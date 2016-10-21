using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_AttributesList : WebControl//海美生活新版本使用
    {
        private int categoryId;
        private int brandId;
        private int importSourceId;
        private string valueStr = string.Empty;
        /// <summary>
        /// 品牌过滤条件
        /// </summary>
        public List<string> search_BrandStr = new List<string>();
        /// <summary>
        /// 产地过滤条件
        /// </summary>
        public List<string> search_ProducingStr = new List<string>();
        protected override void OnInit(EventArgs e)
        {
            int.TryParse(this.Context.Request.QueryString["categoryId"], out this.categoryId);
            int.TryParse(this.Context.Request.QueryString["brand"], out this.brandId);
            int.TryParse(this.Context.Request.QueryString["importSourceId"], out this.importSourceId);
            this.valueStr = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<div class=\"attribute_bd\">");
            this.RendeBrand(stringBuilder);
            this.RendeImportSourceType(stringBuilder);
            this.RendeAttribute(stringBuilder);
            stringBuilder.AppendLine("</div>");
            writer.Write(stringBuilder.ToString());
        }
        private void RendeBrand(StringBuilder sb)
        {
            DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, 1000);
            if (brandCategories != null && brandCategories.Rows.Count > 0)
            {
                sb.AppendLine("<dl class=\"attribute_dl\">");
                sb.AppendLine("<dt class=\"attribute_name\">品牌：</dt>");
                sb.AppendLine("<dd class=\"attribute_val\">");
                sb.AppendLine("<a class=\"more-attr\">更多<span></span></a>");
                sb.AppendLine("<div class=\"h_chooselist\">");
                string text = "all";
                if (this.brandId == 0)
                {
                    text += " select";
                }
                sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", text, this.CreateUrl("brand", "")).AppendLine();
                #region 过滤品牌

                foreach (DataRow dataRow in brandCategories.Rows)
                {
                    text = string.Empty;
                    if (this.brandId == (int)dataRow["BrandId"])
                    {
                        text += " select";
                    }
                    if (search_BrandStr != null)
                    {
                        if (search_BrandStr.Contains(dataRow["BrandId"].ToString()))
                            sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("brand", dataRow["BrandId"].ToString()), dataRow["BrandName"]).AppendLine();
                    }
                    else
                        sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("brand", dataRow["BrandId"].ToString()), dataRow["BrandName"]).AppendLine();
                }
                #endregion
                sb.AppendLine("</div>");
                sb.AppendLine("</dd>");
                sb.AppendLine("</dl>");
            }
        }
        private void RendeImportSourceType(StringBuilder sb)//原产地
        {
            IList<ImportSourceTypeInfo> importSourceTypeInfo = ImportSourceTypeHelper.GetAllImportSourceTypes();
            if (importSourceTypeInfo != null)
            {
                sb.AppendLine("<dl class=\"attribute_dl\">");
                sb.AppendLine("<dt class=\"attribute_name\">产地：</dt>");
                sb.AppendLine("<dd class=\"attribute_val\">");
                sb.AppendLine("<a class=\"more-attr\">更多<span></span></a>");
                sb.AppendLine("<div class=\"h_chooselist\">");
                string text = "all";
                if (this.importSourceId == 0)
                {
                    text += " select";
                }
                sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >不限</a>", text, this.CreateUrl("importsourceid", "")).AppendLine();

                #region 过滤原产地


                foreach (ImportSourceTypeInfo item in importSourceTypeInfo)
                {
                    text = string.Empty;
                    if (this.importSourceId == item.ImportSourceId)
                    {
                        text += " select";
                    }
                    if (search_ProducingStr != null)
                    {
                        if (search_ProducingStr.Contains(item.ImportSourceId.ToString()))
                            sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("importsourceid", item.ImportSourceId.ToString()), item.CnArea).AppendLine();
                    }
                    else
                        sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("importsourceid", item.ImportSourceId.ToString()), item.CnArea).AppendLine();
                }
                #endregion
                sb.AppendLine("</div>");
                sb.AppendLine("</dd>");
                sb.AppendLine("</dl>");
            }
        }
        private void RendeAttribute(StringBuilder sb)
        {
            IList<AttributeInfo> attributeInfoByCategoryId = CategoryBrowser.GetAttributeInfoByCategoryId(this.categoryId, 1000);
            if (attributeInfoByCategoryId != null && attributeInfoByCategoryId.Count > 0)
            {
                foreach (AttributeInfo current in attributeInfoByCategoryId)
                {
                    sb.AppendLine("<dl class=\"attribute_dl\">");
                    if (current.AttributeValues.Count > 0)
                    {
                        sb.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", current.AttributeName).AppendLine();
                        sb.AppendLine("<dd class=\"attribute_val\">");
                        sb.AppendLine("<div class=\"h_chooselist\">");
                        string paraValue = this.RemoveAttribute(this.valueStr, current.AttributeId, 0);
                        string arg = "all select";
                        if (!string.IsNullOrEmpty(this.valueStr) && new Regex(string.Format("{0}_[1-9]+", current.AttributeId)).IsMatch(this.valueStr))
                        {
                            arg = "all";
                        }
                        sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", arg, this.CreateUrl("valuestr", paraValue)).AppendLine();
                        foreach (AttributeValueInfo current2 in current.AttributeValues)
                        {
                            string arg2 = string.Empty;
                            paraValue = this.RemoveAttribute(this.valueStr, current.AttributeId, current2.ValueId);
                            if (!string.IsNullOrEmpty(this.valueStr))
                            {
                                string[] source = this.valueStr.Split(new char[]
								{
									'-'
								});
                                if (source.Contains(current.AttributeId + "_" + current2.ValueId))
                                {
                                    arg2 = "select";
                                }
                            }
                            sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", arg2, this.CreateUrl("valuestr", paraValue), current2.ValueStr).AppendLine();
                        }
                        sb.AppendLine("</div>");
                        sb.AppendLine("</dd>");
                    }
                    sb.AppendLine("</dl>");
                }
            }
        }
        private string RemoveAttribute(string paraValue, int attributeId, int valueId)
        {
            string text = string.Empty;
            if (!string.IsNullOrEmpty(paraValue))
            {
                string[] array = paraValue.Split(new char[]
				{
					'-'
				});
                if (array != null && array.Length > 0)
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string[] array3 = text2.Split(new char[]
							{
								'_'
							});
                            if (array3 != null && array3.Length > 0 && array3[0] != attributeId.ToString())
                            {
                                text = text + text2 + "-";
                            }
                        }
                    }
                }
            }
            return string.Concat(new object[]
			{
				text,
				attributeId,
				"_",
				valueId
			});
        }
        private string CreateUrl(string paraName, string paraValue)
        {
            string text = this.Context.Request.RawUrl;
            if (text.IndexOf("?") >= 0)
            {
                string text2 = text.Substring(text.IndexOf("?") + 1);
                string[] array = text2.Split(new char[]
				{
					Convert.ToChar("&")
				});
                text = text.Replace(text2, "");
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text3 = array2[i];
                    if (!text3.ToLower().StartsWith(paraName + "=") && !text3.ToLower().StartsWith("pageindex="))
                    {
                        text = text + text3 + "&";
                    }
                }
                text = text + paraName + "=" + Globals.UrlEncode(paraValue);
            }
            else
            {
                string text4 = text;
                text = string.Concat(new string[]
				{
					text4,
					"?",
					paraName,
					"=",
					Globals.UrlEncode(paraValue)
				});
            }
            return text;
        }
    }
}
