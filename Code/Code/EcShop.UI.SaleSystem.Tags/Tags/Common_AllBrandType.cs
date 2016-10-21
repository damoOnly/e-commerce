using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_AllBrandType : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(GetBrandTypeMune());
        }
        private string GetBrandTypeMune()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/AllBrandType.xml");
            string key = "CommentFileCache-A";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument xmlDocument = HiCache.Get(key) as XmlDocument;
            if (xmlDocument == null)
            {
                HttpContext context = HiContext.Current.Context;
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max(key, xmlDocument, new CacheDependency(filename));
            }
            XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (XmlNode xmlNode in childNodes)
            {
                if (xmlNode.Attributes["Visible"].Value.ToLower() == "true")
                {
                    string[] array = xmlNode.Attributes["Where"].Value.Split(new char[]
					{
						','
					});
                    if (xmlNode.Attributes["Id"].Value == "1")
                    {
                        stringBuilder.AppendFormat("<li class=\"cur\" categoryId={0}>{1}</li>", array[0], xmlNode.Attributes["Title"].Value);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("<li categoryId={0}>{1}</li>", array[0], xmlNode.Attributes["Title"].Value);
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
