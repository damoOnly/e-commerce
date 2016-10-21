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
    public class Common_SearchList : WebControl//海美生活新版本使用
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            string oldUrl = this.Context.Request.RawUrl;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<ul>");
            string flag = string.Empty;
            string hit = string.Empty;
            string url = CreateUrl(oldUrl, "sortOrderBy", "DisplaySequence", ref flag, ref hit);
            stringBuilder.AppendFormat("<li class=\"{0}\"><a href='{1}'>默认</a></li>", hit, url);
            flag = string.Empty;
            hit = string.Empty;
            url = CreateUrl(oldUrl, "sortOrderBy", "SalePrice", ref flag, ref hit);
            stringBuilder.AppendFormat("<li class=\"{0}\"><a class=\"{1}\" href=\"{2}\">价格<span></span></a></li>", hit, flag, url);
            flag = string.Empty;
            hit = string.Empty;
            url = CreateUrl(oldUrl, "sortOrderBy", "ShowSaleCounts", ref flag, ref hit);
            stringBuilder.AppendFormat("<li class=\"{0}\"><a class=\"{1}\" href=\"{2}\">销量<span></span></a></li>", hit, flag, url);
            flag = string.Empty;
            hit = string.Empty;
            url = CreateUrl(oldUrl, "sortOrderBy", "AddedDate", ref flag, ref hit);
            stringBuilder.AppendFormat("<li class=\"{0}\"><a class=\"{1}\"  href=\"{2}\">上架时间<span></span></a></li>", hit, flag, url);
            flag = string.Empty;
            hit = string.Empty;
            url = CreateUrl(oldUrl, "Stock", "has", ref flag, ref hit);
            stringBuilder.AppendFormat("<li><label><a href=\"{0}\"><input type=\"checkbox\" {1} class=\"checkbox\" onclick=\"window.location.href=\'{0}\'\">有货</a></label></li>", url, flag.Length > 0 ? "checked=\"checked\"" : "");
            stringBuilder.AppendLine("</ul>");
            writer.Write(stringBuilder.ToString());
        }
        private string CreateUrl(string oldUrl, string paraName, string paraValue, ref string flag, ref string hit)
        {
            string text = oldUrl;
            string sortOrderBy = this.Context.Request.QueryString["sortOrderBy"];
            string sortOrder = this.Context.Request.QueryString["sortOrder"];
            string stock = this.Context.Request.QueryString["Stock"];
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
                    if (!text3.StartsWith(paraName + "=") && !text3.ToLower().StartsWith("pageindex=") && !text3.StartsWith("sortOrder="))
                    {
                        text = text + text3 + "&";
                    }
                }
                if (paraName == "Stock")
                {
                    if (stock == "has")
                    {
                        flag = "has";
                        stock = "no";
                    }
                    else
                    {
                        stock = "has";
                    }
                    text = text + paraName + "=" + Globals.UrlEncode(stock);
                }
                else if (!string.IsNullOrEmpty(sortOrderBy) && !string.IsNullOrEmpty(sortOrder))
                {
                    if (sortOrderBy == paraValue)
                    {
                        hit = "sel";
                        //if (sortOrderBy != "AddedDate")
                        //{
                            if (sortOrder == "Desc")
                            {
                                sortOrder = "Asc";
                            }
                            else
                            {
                                sortOrder = "Desc";
                                flag = "up";
                            }
                        //}
                        if (sortOrderBy == "DisplaySequence")
                        {
                            sortOrder = "Desc";
                        }
                    }
                    else
                    {
                        sortOrder = "Desc";
                    }
                    text = text + paraName + "=" + Globals.UrlEncode(paraValue) + "&sortOrder=" + sortOrder;
                }
                else 
                {
                    text = text + paraName + "=" + Globals.UrlEncode(paraValue) + "&sortOrder=Desc";
                }
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
					Globals.UrlEncode(paraValue),
                    "&sortOrder=Desc"

				});
            }
            return text;
        }
    }
}
