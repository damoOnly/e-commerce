using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.SaleSystem.Vshop;
using EcShop.SaleSystem.Catalog;
using EcShop.Membership.Context;
using System.IO;
using System.Web;
using Ecdev.Weixin.MP.Api;
using EcShop.Membership.Core;

namespace EcShop.UI.Web.API
{
    public class VshopDesignHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "LoadMoreProduct":
                    LoadMoreProduct(context);
                    break;
                case "GetAllNavigate":
                    GetAllNavigate(context);
                    break;
                case "LoadMoreTplCfgInfo":
                    LoadMoreTplCfgInfo(context);
                    break;
                case "LoadHomeTopicsList":
                    LoadHomeTopicsList(context);
                    break;
                case "SaveDesignTemplate":
                    SaveDesignTemplate(context);
                    break;
                case "ActivateDesignTemplate":
                    ActivateDesignTemplate(context);
                    break;
                case "LoadDesignTemplate":
                    LoadDesignTemplate(context);
                    break;
                case "InitJSApi":
                    InitJSApi(context);
                    break;

                default:
                    break;
            }

        }
        private void InitJSApi(System.Web.HttpContext context)
        {
            string strjs=GetJSApiScript();
            context.Response.Write(strjs);
            context.Response.End();
        }
        
        private void ActivateDesignTemplate(System.Web.HttpContext context)
        {
            string flag = string.Empty;
            string strHtml = string.Empty;
            strHtml = context.Request["strHtml"];
            bool startUsing = false;
            StringBuilder stringBuider = new StringBuilder();
            stringBuider.Append("{");
            if (!string.IsNullOrEmpty(context.Request["flag"]))
            {
                flag = context.Request["flag"];
            }
            else
            {
                stringBuider.Append("\"success\":0,");
                stringBuider.Append("\"mesg:\",\"改变状态失败\"");
                stringBuider.Append("}");
                context.Response.Write(stringBuider.ToString());
                context.Response.End();
                return;
            }
            if (flag == "1")
            {
                startUsing = false;
            }
            else
            {
                startUsing = true;
            }
            if (startUsing && !string.IsNullOrEmpty(strHtml))//生成静态页面
            {
                CreateHtml(strHtml);
            }
            if (!startUsing)
            {
                File.Delete(HttpContext.Current.Server.MapPath("/Vshop/index.html"));
            }

            if (StoreHelper.ActivateDefaultTemplate(startUsing))
            {
                stringBuider.Append("\"success\":1");
            }
            else
            {
                stringBuider.Append("\"success\":0,");
                stringBuider.Append("\"mesg:\",\"改变状态失败\"");
            }
            stringBuider.Append("}");
            context.Response.Write(stringBuider.ToString());
            context.Response.End();
        }
        private void LoadDesignTemplate(System.Web.HttpContext context)
        {
            string json = string.Empty;
            VshopDesignTemplate template = StoreHelper.GetVshopDesignTemplate(1, true, HiContext.Current.User.UserId);
            if (template != null)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(template);
            }
            context.Response.Write(json);
            context.Response.End();
        }

        protected int CheckSupplierRole()
        {
            int supplierId = 0;
            SiteManager siteManager = HiContext.Current.User as SiteManager;
            if (siteManager.IsInRole("供货商")) //加入供货商id
            {
                supplierId = UserHelper.GetAssociatedSupplierId(siteManager.UserId);
            }
            return supplierId;
        }
        private void SaveDesignTemplate(System.Web.HttpContext context)
        {
            string jsonViewList = context.Request["viewList"];

            VshopDesignTemplate template = StoreHelper.GetVshopDesignTemplate(1, true);
            StringBuilder stringBuider = new StringBuilder();
            stringBuider.Append("{");
            if (template == null)
            {
                VshopDesignTemplate newTemplate = new VshopDesignTemplate();
                newTemplate.AddTime = DateTime.Now;
                newTemplate.TemplateName = "首页模版";
                newTemplate.TemplateType = 1;
                newTemplate.InUse = false;
                newTemplate.Content = jsonViewList;
                newTemplate.SupplierId = CheckSupplierRole();
                if (StoreHelper.AddVshopDesignTemplate(newTemplate))
                {
                    stringBuider.Append("\"success\":1,");
                    stringBuider.Append("\"mesg:\",\"添加成功\"");
                }
                else
                {
                    stringBuider.Append("\"success\":0,");
                    stringBuider.Append("\"mesg:\",\"添加失败\"");
                }
            }
            else
            {
                template.Content = jsonViewList;
                template.SupplierId = CheckSupplierRole();
                if (StoreHelper.UpdateVshopDesignTemplate(template))
                {
                    if(!string.IsNullOrEmpty(context.Request["strHtml"]))
                    {
                        string path = HttpContext.Current.Server.MapPath("/Vshop/index.html");
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            CreateHtml(context.Request["strHtml"]);
                        }
                    }   
                    stringBuider.Append("\"success\":1,");
                    stringBuider.Append("\"mesg:\",\"更新成功\"");
                }
                else
                {
                    stringBuider.Append("\"success\":0,");
                    stringBuider.Append("\"mesg:\",\"更新失败\"");
                }
            }
            stringBuider.Append("}");
            context.Response.Write(stringBuider.ToString());
            context.Response.End();

        }
        private void LoadHomeTopicsList(System.Web.HttpContext context)
        {
            int pageSize = 8;
            int pageIndex = 0;
            int total = 0;
            if (!string.IsNullOrEmpty(context.Request["pageSize"]))
            {
                pageSize = int.Parse(context.Request["pageSize"]);
            }
            if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
            {
                pageIndex = int.Parse(context.Request["pageIndex"]);
                pageIndex--;
            }
            DataSet ds = VshopBrowser.GetHomeTopicsList(ClientType.VShop, pageSize, pageIndex, ref total);
            if (ds.Tables[1] != null && ds.Tables[1].Rows[0] != null)
            {
                total = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                total = total % pageSize == 0 ? total / pageSize : (total / pageSize + 1);
                ds.Tables[1].Rows[0][0] = total;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
            context.Response.Write(json);
            context.Response.End();
        }

        private void LoadMoreTplCfgInfo(System.Web.HttpContext context)
        {
            int type = 0;
            int pageSize = 8;
            int pageIndex = 0;
            int total = 0;
            if (!string.IsNullOrEmpty(context.Request["type"]))
            {
                int.TryParse(context.Request["type"], out type);
            }
            if (!string.IsNullOrEmpty(context.Request["pageSize"]))
            {
                pageSize = int.Parse(context.Request["pageSize"]);
            }
            if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
            {
                pageIndex = int.Parse(context.Request["pageIndex"]);
                pageIndex--;
            }
            IList<TplCfgInfo> listTplCfgInfo = VshopBrowser.GetTplCfgInfoList(ClientType.VShop, type, pageSize, pageIndex, ref total);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(listTplCfgInfo);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.AppendFormat("\"tplCfgInfo\"  :{0},", json);
            total = total % pageSize == 0 ? total / pageSize : (total / pageSize + 1);
            stringBuilder.AppendFormat("\"totalPage\":{0}", total);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();

        }
        private void CreateHtml(string strHtml)
        {
            StringBuilder stringBuiderPage = new StringBuilder();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            stringBuiderPage.AppendLine("<!DOCTYPE html>");
            stringBuiderPage.AppendLine("<html lang=\"zh-CN\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            stringBuiderPage.AppendLine(string.Format("<title>首页 - {0}</title>", masterSettings.SiteName));
            stringBuiderPage.AppendLine("    <meta content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\" name=\"viewport\">");
            stringBuiderPage.AppendLine("<meta name=\"format-detection\" content=\"telephone=no\">");
            stringBuiderPage.AppendLine("<meta name=\"apple-mobile-web-app-capable\" content=\"yes\">");
            stringBuiderPage.AppendLine(" <meta name=\"apple-mobile-web-app-status-bar-style\" content=\"black\">");
            stringBuiderPage.AppendLine("</head>");
            stringBuiderPage.AppendLine("<body class=\"w640\">");
            stringBuiderPage.AppendLine(strHtml);
            stringBuiderPage.AppendLine("  </body>");
            stringBuiderPage.AppendLine("</html>");

            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("/Vshop/index.html"), true, Encoding.UTF8);
            sw.Write(stringBuiderPage.ToString());
            sw.Flush();
            sw.Close();

        }
        private  string GetJSApiScript()
        {
            string jspai = HiCache.Get("StringCache-Ticket") as string;
            if (jspai == null)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                string weixinAppId = masterSettings.WeixinAppId;

                TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
                StringBuilder stringBuilder = new StringBuilder();
                string nonceStr = DateTime.Now.ToString("fffffff");
                stringBuilder.Append("{");
                stringBuilder.AppendFormat("\"debug\":{0},", "false");
                stringBuilder.AppendFormat("\"appId\":\"{0}\",", weixinAppId);
                stringBuilder.AppendFormat("\"timestamp\":\"{0}\",", timestamp);
                stringBuilder.AppendFormat("\"nonceStr\":\"{0}\",", nonceStr);
                stringBuilder.AppendFormat("\"signature\":\"{0}\",", TicketApi.GetTicketSignature(nonceStr, timestamp, "http://" + masterSettings.SiteUrl + "/Vshop/", weixinAppId, masterSettings.WeixinAppSecret));
                stringBuilder.Append("\"jsApiList\": [\"scanQRCode\",\"onMenuShareAppMessage\",\"onMenuShareTimeline\",\"onMenuShareQQ\",\"onMenuShareWeibo\"]");
                stringBuilder.Append("}");
                jspai = stringBuilder.ToString();
                HiCache.Insert("StringCache-Ticket", stringBuilder.ToString(), 9000);  
            }
            return jspai;
        }
        private void GetAllNavigate(System.Web.HttpContext context)
        {
            IList<NavigateInfo> listNav = VShopHelper.GetAllNavigate(ClientType.VShop);
            string retJson = Newtonsoft.Json.JsonConvert.SerializeObject(listNav);
            context.Response.Write(retJson);
            context.Response.End();
        }
        private void LoadMoreProduct(System.Web.HttpContext context)
        {
            int total = 0;
            int pageNumber = 1;
            int pageSize = 0;

            int.TryParse(context.Request["size"], out pageSize);

            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            int? topicId = null;
            int tmpTopicId = 0;
            int.TryParse(context.Request["TopicId"], out tmpTopicId);
            if (tmpTopicId != 0) topicId = tmpTopicId;//topicId

            string keyWord = context.Request["keyWord"];//搜索词
            keyWord = Globals.UrlDecode(keyWord);
            int? categoryId = null;
            int tmpCategoryId = 0;
            int.TryParse(context.Request["categoryId"], out tmpCategoryId);
            if (tmpCategoryId != 0) categoryId = tmpCategoryId;//分类

            string sort = "sort2";//排序字段
            string tmpSort = context.Request["sort"];
            if (!string.IsNullOrEmpty(tmpSort))
            {
                sort = tmpSort;
            }
            string order = "asc";//排序方式
            string tmpOrder = context.Request["order"];
            if (!string.IsNullOrEmpty(tmpOrder))
            {
                order = tmpOrder;
            }

            DataTable dtProducts = ProductBrowser.GetProducts(topicId, categoryId, keyWord, pageNumber, pageSize, out total, sort, true, order);//分页查询
            int totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"products\":");
            if (dtProducts.Rows.Count > 0)
            {

                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dtProducts);


                //strProducts = strProducts.Remove(strProducts.Length - 1).Substring(1);
                stringBuilder.Append(strProducts);
            }
            else
            {
                stringBuilder.Append("\"\"");
            }
            stringBuilder.Append(",");
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }
    }
}
