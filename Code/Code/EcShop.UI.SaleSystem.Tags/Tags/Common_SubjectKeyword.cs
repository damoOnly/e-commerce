using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_SubjectKeyword : WebControl
	{
		public int CommentId
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}
		public string RendHtml()
		{
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "keyword");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				int categoryId = 0;
				int categoryId2 = 0;
				int hotKeywordsNum = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId2);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out hotKeywordsNum);
				CategoryInfo category = CategoryBrowser.GetCategory(categoryId2);
				if (category != null)
				{
					categoryId = category.TopCategoryId;
				}
				//DataTable hotKeywords = CommentBrowser.GetHotKeywords(categoryId, hotKeywordsNum);

                DataTable hotKeywords = CommentBrowser.GetHotKeywords(categoryId, hotKeywordsNum, ClientType.PC);
				stringBuilder.AppendFormat("<ul class=\"keyword cssEdite\" type=\"keyword\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				if (hotKeywords != null && hotKeywords.Rows.Count > 0)
				{
					foreach (DataRow dataRow in hotKeywords.Rows)
					{
                        if (dataRow["CategoryId"] !=DBNull.Value)
                        {
                            stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory((int)dataRow["CategoryId"], null) + "?keywords=" + Globals.UrlEncode((string)dataRow["Keywords"]), dataRow["Keywords"]).AppendLine();
                        }
                        else
                        {
                            stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", "/SubCategory.aspx?keywords=" + Globals.UrlEncode((string)dataRow["Keywords"]), dataRow["Keywords"]).AppendLine();
                        }
					}
				}
				stringBuilder.AppendLine("</ul>");
			}
			return stringBuilder.ToString();
		}
	}
}
