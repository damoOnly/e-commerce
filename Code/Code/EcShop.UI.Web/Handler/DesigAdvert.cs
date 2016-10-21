using EcShop.Core;
using EcShop.Entities;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.SaleSystem.Tags;
using Ecdev.Components.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using HtmlAgilityPack;
namespace EcShop.UI.Web.Handler
{
    public class DesigAdvert : AdminPage, System.Web.IHttpHandler
    {
        private string message = "";
        private string modeId = "";
        private string elementId = "";
        private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";
        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public new void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                this.modeId = context.Request.Form["ModelId"];
                string a;
                if ((a = this.modeId) != null)
                {
                    if (!(a == "editeadvertslide"))
                    {
                        if (!(a == "editeadvertimage"))
                        {
                            if (!(a == "editelogo"))
                            {
                                if (a == "editeadvertcustom")
                                {
                                    string text = context.Request.Form["Param"];
                                    if (text != "")
                                    {
                                        JObject advertcustomobject = (JObject)JsonConvert.DeserializeObject(text);
                                        if (this.CheckAdvertCustom(advertcustomobject) && this.UpdateAdvertCustom(advertcustomobject))
                                        {
                                            var value = new
                                            {
                                                AdCustom = new Common_CustomAd
                                                {
                                                    AdId = System.Convert.ToInt32(this.elementId)
                                                }.RendHtml()
                                            };
                                            this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string text2 = context.Request.Form["Param"];
                                if (text2 != "")
                                {
                                    JObject jObject = (JObject)JsonConvert.DeserializeObject(text2);
                                    if (this.CheckLogo(jObject) && this.UpdateLogo(jObject))
                                    {
                                        Common_Logo common_Logo = new Common_Logo();
                                        var value2 = new
                                        {
                                            LogoUrl = common_Logo.RendHtml()
                                        };
                                        this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value2));
                                    }
                                }
                            }
                        }
                        else
                        {
                            string text3 = context.Request.Form["Param"];
                            if (text3 != "")
                            {
                                JObject advertimageobject = (JObject)JsonConvert.DeserializeObject(text3);
                                Dictionary<string, string> common_ImageNewAdCtrAttr;
                                if (this.CheckAdvertImage(advertimageobject) && this.UpdateAdvertImage(advertimageobject, context, out common_ImageNewAdCtrAttr))
                                {
                                    var value3 = new
                                    {
                                        AdImage = new Common_ImageNewAd
                                        {
                                            AdId = this.elementId,
                                            ImgStyle = common_ImageNewAdCtrAttr != null ? common_ImageNewAdCtrAttr["ImgStyle"]:"",
                                            DivCss = common_ImageNewAdCtrAttr != null ? common_ImageNewAdCtrAttr["DivCss"] : ""
                                        }.RendHtml()
                                    };
                                    this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value3));
                                }
                            }
                        }
                    }
                    else
                    {
                        string text4 = context.Request.Form["Param"];
                        if (text4 != "")
                        {
                            JObject avdvertobject = (JObject)JsonConvert.DeserializeObject(text4);
                            if (this.CheckAdvertSlide(avdvertobject) && this.UpdateAdvertSlide(avdvertobject))
                            {
                                var value4 = new
                                {
                                    AdSlide = new Common_SlideAd
                                    {
                                        AdId = System.Convert.ToInt32(this.elementId)
                                    }.RendHtml()
                                };
                                this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value4));
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }
        private bool CheckAdvertSlide(JObject avdvertobject)
        {
            if (string.IsNullOrEmpty(avdvertobject["Image1"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image2"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image3"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image4"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image5"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image6"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image7"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image8"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image9"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image10"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
                return false;
            }
            int num = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (!string.IsNullOrEmpty(avdvertobject["Image" + i].ToString()))
                {
                    num++;
                }
            }
            if (num < 3)
            {
                this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
                return false;
            }
            if (string.IsNullOrEmpty(avdvertobject["Id"].ToString()) || avdvertobject["Id"].ToString().Split(new char[]
			{
				'_'
			}).Length != 2)
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
                return false;
            }
            return true;
        }
        private bool UpdateAdvertSlide(JObject avdvertobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改轮播广告失败\"");
            this.elementId = avdvertobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
            avdvertobject["Id"] = this.elementId;
            System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(avdvertobject);
            return TagsHelper.UpdateAdNode((int)System.Convert.ToInt32(this.elementId), "slide", xmlNodeString);
        }
        private bool CheckAdvertImage(JObject advertimageobject)
        {
            if (string.IsNullOrEmpty(advertimageobject["Image"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择广告图片！\"");
                return false;
            }
            if (string.IsNullOrEmpty(advertimageobject["Id"].ToString()) || advertimageobject["Id"].ToString().Split(new char[]
			{
				'_'
			}).Length != 2)
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
                return false;
            }
            return true;
        }
        private bool UpdateAdvertImage(JObject advertimageobject, System.Web.HttpContext context, out Dictionary<string, string> common_ImageNewAdCtrAttr)
        {
            System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertimageobject);
            if (advertimageobject["Id"].ToString().StartsWith("newads_"))
            {
                return TagsHelper.UpdateNewAdNode(advertimageobject["Id"].ToString(), "image", xmlNodeString, out common_ImageNewAdCtrAttr);
                //return UpdateAdvertCustomWriteToHTML(advertimageobject, context);
            }
            if (advertimageobject["Id"].ToString().StartsWith("logo_"))
            {
                return TagsHelper.UpdateNewAdNode(advertimageobject["Id"].ToString(), "logo", xmlNodeString, out common_ImageNewAdCtrAttr);
                //return UpdateAdvertCustomWriteToHTML(advertimageobject, context);
            }
            this.message = string.Format(this.resultformat, "false", "\"修改单张广告图片失败\"");
            this.elementId = advertimageobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
            advertimageobject["Id"] = this.elementId;
            common_ImageNewAdCtrAttr = null;
            //System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertimageobject);
            return TagsHelper.UpdateAdNode((int)System.Convert.ToInt32(this.elementId), "image", xmlNodeString);
        }
        private bool CheckAdvertCustom(JObject advertcustomobject)
        {
            if (string.IsNullOrEmpty(advertcustomobject["Id"].ToString()) || advertcustomobject["Id"].ToString().Split(new char[]
			{
				'_'
			}).Length != 2)
            {
                this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
                return false;
            }
            return true;
        }
        private bool UpdateAdvertCustom(JObject advertcustomobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"自定义编辑失败\"");
            this.elementId = advertcustomobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];
            advertcustomobject["Id"] = this.elementId;
            System.Collections.Generic.Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertcustomobject);
            if (xmlNodeString.Keys.Contains("Html"))
            {
                xmlNodeString["Html"] = Globals.HtmlDecode(xmlNodeString["Html"]);
            }
            return TagsHelper.UpdateAdNode((int)System.Convert.ToInt32(this.elementId), "custom", xmlNodeString);
        }
        private bool UpdateAdvertCustomWriteToHTML(JObject advertcustomobject, System.Web.HttpContext context)//写入文件
        {
            this.elementId = advertcustomobject["Id"].ToString();
            string content = string.Empty;
            string skinName = "Skin-Default.html";
            string skinHeaderName = "Ascx/tags/Skin-Common_Header1.ascx";
            string skinPath = HiContext.Current.GetSkinPath();
            if (skinName.StartsWith(skinPath))
            {
                skinPath = skinName;
            }
            else if (skinName.StartsWith("/"))
            {
                skinPath += skinName;
            }
            else
            {
                skinPath += "/" + skinName;
            }
            try
            {
                if (elementId == "newads_001")
                {
                    skinPath = skinPath.Replace(skinName, skinHeaderName);
                }
                using (StreamReader reader = new StreamReader(context.Server.MapPath(skinPath), Encoding.UTF8))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch
            {
                return false;
            }
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            Regex regex = new Regex(string.Format("<div[^>]*id=\"{0}\"[^>]*>[\\s\\S]*?(((?'open'<div[^>]*>)[\\s\\S]*?)+((?'-open'</div>)[\\s\\S]*?)+)*(?(open)(?!))</div>", elementId));
            MatchCollection tempMatches = regex.Matches(content);
            if (tempMatches.Count == 1)
            {
                string image = advertcustomobject["Image"].ToString();
                string url = advertcustomobject["Url"].ToString();
                string strDiv = tempMatches[0].Value;
                string newStrDiv = string.Empty;
                int lastIndex = 0;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(strDiv);
                HtmlNode node = doc.GetElementbyId(elementId);
                HtmlNode childNode = node.FirstChild;
                if (childNode != null)
                {
                    if (childNode.Attributes.Contains("href"))
                    {
                        childNode.Attributes["href"].Value = url;
                    }
                    else
                    {
                        childNode.Attributes.Add("href", url);
                    }
                    HtmlNode childImageNode = childNode.FirstChild;
                    if (childImageNode != null)
                    {
                        if (childImageNode.Attributes.Contains("data-original"))
                        {
                            childImageNode.Attributes["data-original"].Value = image;
                        }
                        else
                        {
                            if (childImageNode.Attributes.Contains("src"))
                            {
                                childImageNode.Attributes["src"].Value = image;
                            }
                            else
                            {
                                childImageNode.Attributes.Add("src", url);
                            }
                        }
                    }
                    else
                    {
                        childImageNode = HtmlNode.CreateNode(string.Format("<img  src=\"/images/lazy.png\" data-original=\"{0}\" class=\"lazyload\"/>", image));
                        childNode.ChildNodes.Add(childImageNode);//构造img            
                    }
                    newStrDiv = node.OuterHtml;
                }
                else
                {
                    newStrDiv = string.Format("<div class=\"cssEdite\" id=\"{0}\"><a href=\"{1}\"><img  src=\"/images/lazy.png\" data-original=\"{2}\" class=\"lazyload\"/></a></div>", elementId, url, image);//构造
                }
                content = content.Replace(strDiv, newStrDiv);
                try
                {
                    File.WriteAllText(context.Server.MapPath(skinPath), content, Encoding.UTF8);
                    this.elementId = advertcustomobject["Id"].ToString().Split(new char[]
			{
				'_'
			})[1];//防止报错
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool CheckLogo(JObject logoobject)
        {
            if (string.IsNullOrEmpty(logoobject["LogoUrl"].ToString()))
            {
                this.message = string.Format(this.resultformat, "false", "\"请上传Logo图片！\"");
                return false;
            }
            return true;
        }
        private bool UpdateLogo(JObject advertimageobject)
        {
            this.message = string.Format(this.resultformat, "false", "\"修改Logo图片失败\"");
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            siteSettings.LogoUrl = advertimageobject["LogoUrl"].ToString();
            SettingsManager.Save(siteSettings);
            return true;
        }
        private bool ValidationSettings(SiteSettings setting, ref string errors)
        {
            ValidationResults validationResults = Validation.Validate<SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
            if (!validationResults.IsValid)
            {
                foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
                {
                    errors += Formatter.FormatErrorMessage(current.Message);
                }
            }
            return validationResults.IsValid;
        }
        public System.Collections.Generic.Dictionary<string, string> GetXmlNodeString(JObject scriptobject)
        {
            System.Collections.Generic.Dictionary<string, string> dictionary = scriptobject.ToObject<System.Collections.Generic.Dictionary<string, string>>();
            System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
            {
                dictionary2.Add(current.Key, Globals.HtmlEncode(current.Value.ToString()));
            }
            return dictionary2;
        }
    }
}
