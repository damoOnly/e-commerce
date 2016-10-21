using EcShop.Core;
using EcShop.Entities;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_ImageNewAd : WebControl
    {
        /// <summary>
        /// divId
        /// </summary>
        public string AdId
        {
            get;
            set;
        }
        /// <summary>
        /// divClass样式 字符串
        /// </summary>
        public string DivCss
        {
            get;
            set;
        }
        /// <summary>
        /// img标签style样式字符串
        /// </summary>
        public string ImgStyle
        {
            get;
            set;
        }
        /// <summary>
        /// img标签属性和值字符串多个';'隔开
        /// </summary>
        public string ImageAttr
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
            string type = (this.AdId.StartsWith("newads_") ? "image" : "logo");
            XmlNode xmlNode = TagsHelper.FindNewAdNode(this.AdId, type);
            StringBuilder stringBuilder = new StringBuilder();
            if (xmlNode != null)
            {
                System.Collections.Generic.Dictionary<string, string> dic = new System.Collections.Generic.Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> dic1;
                if ((string.IsNullOrWhiteSpace(xmlNode.Attributes["DivCss"].Value) || string.IsNullOrWhiteSpace(xmlNode.Attributes["ImgStyle"].Value) || string.IsNullOrWhiteSpace(xmlNode.Attributes["ImageAttr"].Value)))
                {

                    if (!string.IsNullOrWhiteSpace(ImgStyle) && string.IsNullOrWhiteSpace(xmlNode.Attributes["ImgStyle"].Value))
                    {
                        dic.Add("ImgStyle", this.ImgStyle);
                    }
                    if (!string.IsNullOrWhiteSpace(DivCss) && string.IsNullOrWhiteSpace(xmlNode.Attributes["DivCss"].Value))
                    {
                        dic.Add("DivCss", this.DivCss);
                    }
                    if (!string.IsNullOrWhiteSpace(ImageAttr) && string.IsNullOrWhiteSpace(xmlNode.Attributes["ImageAttr"].Value))
                    {
                        dic.Add("ImageAttr", this.ImageAttr);
                    } 
                }
                if (dic.Count > 0)
                {
                    TagsHelper.UpdateNewAdNode(this.AdId, type, dic, out dic1);
                    xmlNode = TagsHelper.FindNewAdNode(this.AdId, type);
                }
                dic.Clear();
                dic1 = null;
                if (xmlNode.Attributes["ImgStyle"].Value != this.ImgStyle && !string.IsNullOrEmpty(this.ImgStyle))
                {
                    dic.Add("ImgStyle", this.ImgStyle);
                }
                if (xmlNode.Attributes["DivCss"].Value.Replace("cssEdite", "").Replace(" ", "") != this.DivCss.Replace("cssEdite", "").Replace(" ", "") && !string.IsNullOrEmpty(this.DivCss))
                {
                    dic.Add("DivCss", this.DivCss);
                }
                if (xmlNode.Attributes["ImageAttr"].Value != this.ImageAttr && !string.IsNullOrEmpty(this.ImageAttr))
                {
                    dic.Add("ImageAttr", this.ImageAttr);
                }
                if (dic.Count > 0)
                    TagsHelper.UpdateNewAdNode(this.AdId, type, dic, out dic1);
                stringBuilder.AppendFormat("<div class=\"{0}\" type=\"" + type + "\" id=\"{1}\"><a {2}><img src='{3}'class='lazyload'></a></div>", this.DivCss, this.AdId, string.IsNullOrWhiteSpace(Globals.HtmlDecode(xmlNode.Attributes["Url"].Value)) ? "" : ("href=\"" + Globals.HtmlDecode(xmlNode.Attributes["Url"].Value + "\"")), Globals.HtmlDecode(xmlNode.Attributes["Image"].Value), this.ImgStyle, string.IsNullOrWhiteSpace(ImageAttr) ? "" : ImageAttr);
                //stringBuilder.AppendFormat("<div class=\"ad_custom cssEdite\" type=\"custom\" id=\"ads_{0}\" >{1}</div>", this.AdId, Globals.HtmlDecode(xmlNode.Attributes["Html"].Value)).AppendLine();
            }
            else
            {
                System.Collections.Generic.Dictionary<string, string> dic = new System.Collections.Generic.Dictionary<string, string>();
                dic.Add("Url", "");
                dic.Add("Image", "");
                dic.Add("DivCss", this.DivCss);
                dic.Add("ImgStyle", this.ImgStyle);
                System.Collections.Generic.Dictionary<string, string> dic1;
                TagsHelper.UpdateNewAdNode(this.AdId, this.AdId.StartsWith("newads_") ? "image" : "logo", dic, out dic1);
                stringBuilder.AppendFormat("<div class=\"{0}\" type=\"" + type + "\" id=\"{1}\"><a {2}><img src='{3}' class='lazyload'></a></div>", this.DivCss, this.AdId, "", "", this.ImgStyle);
            }
            return stringBuilder.ToString();
        }
    }
}
