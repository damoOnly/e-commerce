using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace EcShop.Entities
{
    public static class TagsHelper
    {
        public static XmlNode FindProductNode(int subjectId, string type)
        {
            XmlDocument productDocument = TagsHelper.GetProductDocument();
            return productDocument.SelectSingleNode(string.Format("//Subject[@SubjectId='{0}' and @Type='{1}']", subjectId, type));
        }
        public static XmlNode FindAdNode(int id, string type)
        {
            XmlDocument adDocument = TagsHelper.GetAdDocument();
            return adDocument.SelectSingleNode(string.Format("//Ad[@Id='{0}' and @Type='{1}']", id, type));
        }
        public static XmlNode FindCommentNode(int id, string type)
        {
            XmlDocument commentDocument = TagsHelper.GetCommentDocument();
            return commentDocument.SelectSingleNode(string.Format("//Comment[@Id='{0}' and @Type='{1}']", id, type));
        }
        public static XmlNode FindHeadMenuNode(int id)
        {
            XmlDocument headMenuDocument = TagsHelper.GetHeadMenuDocument();
            return headMenuDocument.SelectSingleNode(string.Format("//Menu[@Id='{0}']", id));
        }
        public static bool UpdateProductNode(int subjectId, string type, System.Collections.Generic.Dictionary<string, string> simplenode)
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
            bool result = false;
            XmlDocument productDocument = TagsHelper.GetProductDocument();
            XmlNode xmlNode = TagsHelper.FindProductNode(subjectId, type);
            if (xmlNode != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in simplenode)
                {
                    xmlNode.Attributes[current.Key].Value = current.Value;
                }
                productDocument.Save(filename);
                TagsHelper.RemoveProductNodeCache();
                result = true;
            }
            return result;
        }
        public static bool UpdateAdNode(int aId, string type, System.Collections.Generic.Dictionary<string, string> adnode)
        {
            bool result = false;
            XmlDocument adDocument = TagsHelper.GetAdDocument();
            XmlNode xmlNode = TagsHelper.FindAdNode(aId, type);
            if (xmlNode != null)
            {
                if (adnode.ContainsKey("Id"))
                {
                    adnode.Remove("Id");
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in adnode)
                {
                    xmlNode.Attributes[current.Key].Value = current.Value;
                }
                string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
                adDocument.Save(filename);
                TagsHelper.RemoveAdNodeCache();
                result = true;
            }
            return result;
        }
        public static XmlNode FindNewAdNode(string id, string type)
        {
            XmlDocument adDocument = TagsHelper.GetAdDocument();
            return adDocument.SelectSingleNode(string.Format("//Ad[@Id='{0}' and @Type='{1}']", id, type));
        }
        public static bool UpdateNewAdNode(string aId, string type, System.Collections.Generic.Dictionary<string, string> adnode, out Dictionary<string, string> common_ImageNewAdCtrAttr)
        {
            common_ImageNewAdCtrAttr = new Dictionary<string, string>();
            bool result = false;
            XmlDocument adDocument = TagsHelper.GetAdDocument();
            XmlNode xmlNode = TagsHelper.FindNewAdNode(aId, type);
            if (xmlNode != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in adnode)
                {
                    xmlNode.Attributes[current.Key].Value = current.Value;
                }
                string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
                common_ImageNewAdCtrAttr.Add("DivCss", xmlNode.Attributes["DivCss"].Value);
                common_ImageNewAdCtrAttr.Add("ImgStyle", xmlNode.Attributes["ImgStyle"].Value);
                common_ImageNewAdCtrAttr.Add("ImageAttr", xmlNode.Attributes["ImageAttr"].Value);
                adDocument.Save(filename);
                TagsHelper.RemoveAdNodeCache();
                result = true;
            }
            else
            {
                XmlElement xmlel = TagsHelper.CreateNewAdNode(aId, type, adnode);
                adDocument.DocumentElement.AppendChild(xmlel);
                string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
                adDocument.Save(filename);
                TagsHelper.RemoveAdNodeCache();
                result = true;
            }
            return result;
        }
        public static XmlElement CreateNewAdNode(string newads, string type, System.Collections.Generic.Dictionary<string, string> newadsAttrs)
        {
            XmlDocument adDocument = TagsHelper.GetAdDocument();
            XmlElement xml = adDocument.CreateElement("Ad");
            xml.SetAttribute("Type", type);
            xml.SetAttribute("Id", newads);
            foreach (string s in newadsAttrs.Keys)
            {
                xml.SetAttribute(s, newadsAttrs[s]);
            }
            if (!newadsAttrs.ContainsKey("DivCss"))
            {
                xml.SetAttribute("DivCss", "");
            }
            if (!newadsAttrs.ContainsKey("ImgStyle"))
            {
                xml.SetAttribute("ImgStyle", "");
            }
            if (!newadsAttrs.ContainsKey("ImageAttr"))
            {
                xml.SetAttribute("ImageAttr", "");
            }
            return xml;
        }
        public static bool UpdateCommentNode(int commentId, string type, System.Collections.Generic.Dictionary<string, string> commentnode)
        {
            bool result = false;
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
            XmlDocument commentDocument = TagsHelper.GetCommentDocument();
            XmlNode xmlNode = TagsHelper.FindCommentNode(commentId, type);
            if (xmlNode != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in commentnode)
                {
                    xmlNode.Attributes[current.Key].Value = current.Value;
                }
                commentDocument.Save(filename);
                TagsHelper.RemoveCommentNodeCache();
                result = true;
            }
            return result;
        }
        public static bool UpdateHeadMenuNode(int menuId, System.Collections.Generic.Dictionary<string, string> headmenunode)
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
            bool result = false;
            XmlDocument commentDocument = TagsHelper.GetCommentDocument();
            XmlNode xmlNode = TagsHelper.FindHeadMenuNode(menuId);
            if (xmlNode != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in headmenunode)
                {
                    xmlNode.Attributes[current.Key].Value = current.Value;
                }
                commentDocument.Save(filename);
                TagsHelper.RemoveHeadMenuCache();
                result = true;
            }
            return result;
        }
        private static void RemoveProductNodeCache()
        {
            string key = "ProductFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }
        private static void RemoveHeadMenuCache()
        {
            string key = "HeadMenuCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }
        private static void RemoveAdNodeCache()
        {
            string key = "AdFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }
        private static void RemoveCommentNodeCache()
        {
            string key = "CommentFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }
        private static XmlDocument GetProductDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
            string key = "ProductFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument xmlDocument = HiCache.Get(key) as XmlDocument;
            if (xmlDocument == null)
            {
                HttpContext context = HiContext.Current.Context;
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max(key, xmlDocument, new CacheDependency(filename));
            }
            return xmlDocument;
        }
        private static XmlDocument GetAdDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
            string key = "AdFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument xmlDocument = HiCache.Get(key) as XmlDocument;
            if (xmlDocument == null)
            {
                HttpContext context = HiContext.Current.Context;
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max(key, xmlDocument, new CacheDependency(filename));
            }
            return xmlDocument;
        }
        private static XmlDocument GetCommentDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
            string key = "CommentFileCache-Admin";
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
            return xmlDocument;
        }
        private static XmlDocument GetHeadMenuDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
            string key = "HeadMenuCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument xmlDocument = HiCache.Get(key) as XmlDocument;
            if (xmlDocument == null)
            {
                HttpContext context = HiContext.Current.Context;
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max(key, xmlDocument, new CacheDependency(filename));
            }
            return xmlDocument;
        }
    }
}
