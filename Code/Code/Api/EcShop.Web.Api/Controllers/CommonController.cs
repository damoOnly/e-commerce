using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Hosting;
using System.Xml;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Comments;
using EcShop.SaleSystem.Member;
using Ecdev.Components.Validation;
using EcShop.ControlPanel.Store;

using EcShop.Entities;
using EcShop.Web.Api.ApiException;
using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.Result;
using EcShop.Web.Api.Model.RequestJsonParams;
using EcShop.ControlPanel.Sales;

namespace EcShop.Web.Api.Controllers
{
    public class CommonController : EcdevApiController
    {
        [HttpGet]
        public IHttpActionResult Messages()
        {
            return null;
        }

        [HttpGet]
        public IHttpActionResult UnreadMessageCount()
        {
            return null;
        }

        [HttpGet]
        public IHttpActionResult Message()
        {
            return null;
        }

        [HttpPost]
        public IHttpActionResult Feedback(JObject request)
        {
            Logger.WriterLogger("Common.Feedback, Params: " + request.ToString(), LoggerType.Info);

            ParamFeedback param = new ParamFeedback();

            try
            {
                param = request.ToObject<ParamFeedback>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string userId = param.UserId;
            string content = param.Content;
            int feedbackType = param.FeedbackType;
            string contactWay = param.ContactWay;

            int? innerUserId = new Nullable<int>();
            string username = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                innerUserId = member.UserId;
                username = member.Username;
            }

            LeaveCommentInfo leaveCommentInfo = new LeaveCommentInfo();
            leaveCommentInfo.UserName = Globals.HtmlEncode(username);
            leaveCommentInfo.UserId = innerUserId;
            leaveCommentInfo.Title = "";
            leaveCommentInfo.PublishContent = Globals.HtmlEncode(content);
            leaveCommentInfo.ContactWay = Globals.HtmlEncode(contactWay);
            leaveCommentInfo.FeedbackType = feedbackType;

            if(param.Images.Count>0)
            {
                foreach(SubmitImage item in param.Images)
                {
                    leaveCommentInfo.Images.Add(item.Image);
                }
            }

            if (CommentBrowser.InsertLeaveComment(leaveCommentInfo))
            {
                StandardResult<string> result = new StandardResult<string>();
                result.code = 0;
                result.msg = "保存成功";
                result.data = null;

                return base.JsonActionResult(result);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpGet]
        public IHttpActionResult HelpCategory(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.HelpCategory, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Common.HelpCategory");
            }

            List<HelpCategoryListItem> items = new List<HelpCategoryListItem>();

            IList<HelpCategoryInfo> categories = CommentBrowser.GetHelpCategorys();

            if (categories != null)
            {
                HelpCategoryListItem item = null;

                foreach(var current in categories)
                {
                    item = new HelpCategoryListItem();

                    item.Id = current.CategoryId ?? 0;
                    item.Name = current.Name;
                    item.Description = current.Description;
                    item.IconUrl = Util.AppendImageHost(current.IconUrl);
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            return base.JsonActionResult(new StandardResult<ListResult<HelpCategoryListItem>>() { 
                code = 0,
                msg = "",
                data = new ListResult<HelpCategoryListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            });
        }

        [HttpGet]
        public IHttpActionResult HelpByCategory(int categoryId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.HelpByCategory, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&categoryId={4}", accessToken, channel, platform, ver, categoryId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Common.HelpByCategory");
            }

            IList<HelpCategoryInfo> categories = CommentBrowser.GetHelpCategorys();

            HelpQuery helpQuery = new HelpQuery();
            if (categoryId > 0)
            {
                helpQuery.CategoryId = categoryId;
            }
            helpQuery.PageIndex = 1;
            helpQuery.PageSize = int.MaxValue;
            helpQuery.SortBy = "AddedDate";
            helpQuery.SortOrder = EcShop.Core.Enums.SortAction.Desc;

            List<HelpListItem> items = new List<HelpListItem>();

            DbQueryResult dbQueryResult = CommentBrowser.GetHelpList(helpQuery);

            DataTable dt = dbQueryResult.Data as DataTable;

            if (dt != null)
            {
                HelpListItem item = null;

                foreach(DataRow row in dt.Rows)
                {
                    item = new HelpListItem();

                    item.CategoryId = (int)row["CategoryId"];
                    item.Id = (int)row["HelpId"];
                    item.Title = (string)row["Title"];
                    item.Description = "";
                    if (row["Description"] != DBNull.Value)
                    {
                        item.Description = (string)row["Description"];
                    }
                    item.AddedDate = "";
                    if (row["AddedDate"] != DBNull.Value)
                    {
                        item.AddedDate = ((DateTime)row["AddedDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    items.Add(item);
                }

            }

            return base.JsonActionResult(new StandardResult<ListResult<HelpListItem>>()
            {
                code = 0,
                msg = "",
                data = new ListResult<HelpListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            });
        }


        [HttpGet]
        public IHttpActionResult Help(int id, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.Help, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&id={4}", accessToken, channel, platform, ver, id), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Common.Help");
            }

            HelpListItem item = new HelpListItem();

            HelpInfo help = CommentBrowser.GetHelp(id);

            if (help != null)
            {
                item.CategoryId = help.CategoryId;
                item.Id = help.HelpId;
                item.Title = help.Title;
                item.Description = help.Description;

                var regex = new Regex(@"""/Storage/master");

                item.Content = regex.Replace(help.Content, "\"" + base.STORAGE_HOST + @"/Storage/master");

                item.Content=StringExtension.CovertToHtml(item.Content);
                item.AddedDate = help.AddedDate.ToString("yyyy-MM-dd HH:mm:ss");

                return base.JsonActionResult(new StandardResult<HelpListItem>()
                {
                    code = 0,
                    msg = "",
                    data = item
                });
            }

            else
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "找不到该帮助内容",
                    data = ""
                };

                return base.JsonActionResult(result);
            }
         
        }

        [HttpGet]
        public IHttpActionResult Dictionary(string dictType, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.Upgrade, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&dictType={4}", accessToken, channel, platform, ver, dictType), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.Get");
            }

            if (dictType.ToLower() == "region")
            {
                StandardResult<List<RegionListItem>> result = new StandardResult<List<RegionListItem>>()
                {
                    code = 0,
                    msg = "",
                    data = GetRegionDictionary()
                };

                return base.JsonActionResult(result);
            }

            if (dictType.ToLower() == "feedback")
            {
                StandardResult<List<DictionaryListItem>> result = new StandardResult<List<DictionaryListItem>>()
                {
                    code = 0,
                    msg = "",
                    data = GetFeedbackType()
                };

                return base.JsonActionResult(result);
            }

            if (dictType.ToLower() == "refundtype")
            {
                StandardResult<List<DictionaryListItem>> result = new StandardResult<List<DictionaryListItem>>()
                {
                    code = 0,
                    msg = "",
                    data = GetRefundType()
                };

                return base.JsonActionResult(result);
            }

            if (dictType.ToLower() == "express")
            {
                StandardResult<List<ExpressListItem>> result = new StandardResult<List<ExpressListItem>>()
                {
                    code = 0,
                    msg = "",
                    data = GetExpressCompany()
                };

                return base.JsonActionResult(result);
            }

            return null;
        }

        [HttpGet]
        public IHttpActionResult Upgrade(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.Upgrade, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Common.Upgrade");
            }

            decimal resultver = Util.ConvertVer(ver) * 1.00m / 100;

            string device = "";

            switch(platform)
            {
                case 3:
                    device = "android";
                    AppAndroidVersionResult result = GetAndroidLatestVersion();

                    result.IsForcibleUpgrade = APPHelper.IsForcibleUpgradeByPreviousVersion(resultver, "android");
                    return base.JsonActionResult(new StandardResult<AppAndroidVersionResult>()
                    {
                        code = 0,
                        msg = "",
                        data = result
                    });
                    //break;
                case 2:
                    device = "ios";
                    break;
                default:
                    break;
            }

            return base.JsonActionResult(new StandardResult<object>()
            {
                code = 0,
                msg = "",
                data = null
            });
        }


        [HttpGet]
        public IHttpActionResult UpgradeIos(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Common.UpgradeIos, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);
            decimal resultver = Util.ConvertVer(ver)*1.00m/ 100;
            AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecordOrderById("ios");

            if (appVersionRecordInfo!=null)
            {
                IosVersionResult result = new IosVersionResult();
                result.VerName = appVersionRecordInfo.Version.ToString("0.00").Replace(".", "");
                result.Descript = appVersionRecordInfo.Description;
                //result.IsForceUpgrade = appVersionRecordInfo.IsForcibleUpgrade;
                result.IsForceUpgrade = APPHelper.IsForcibleUpgradeByPreviousVersion(resultver, "ios");
                return base.JsonActionResult(new StandardResult<IosVersionResult>()
                {
                    code = 0,
                    msg = "",
                    data = result
                });
            }

            else
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 1,
                    msg = "未获取到ios版本信息",
                    data = null
                });
            }

                   
                
        }

        [HttpPost]
        public IHttpActionResult SendMessage(JObject request)
        {
            Logger.WriterLogger("Common.SendMessage, Params: " + request.ToString(), LoggerType.Info);

            ParamSendMessage param = new ParamSendMessage();

            try
            {
                param = request.ToObject<ParamSendMessage>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;
            string userId = param.UserId;
            Member member = GetMember(userId.ToSeesionId());

            if (string.IsNullOrWhiteSpace(param.Title))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "标题不能为空",
                    data = ""
                };

                return base.JsonActionResult(result);
            }

            if (string.IsNullOrWhiteSpace(param.Content))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "内容不能为空",
                    data = ""
                };

                return base.JsonActionResult(result);
            }

            if (member != null)
            {
                MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
                messageBoxInfo.Sernder = member.Username;
                messageBoxInfo.Accepter = "admin";
                messageBoxInfo.Title = param.Title;

                messageBoxInfo.Content = param.Content;
                if (CommentBrowser.SendMessage(messageBoxInfo))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "失败",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }




        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]

        public IHttpActionResult GetPayInfo(string accessToken, int channel, int platform, string ver,int type)
        {
            Logger.WriterLogger("Common.GetPayInfo, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Common.GetPayInfo");
            }

            if (type == 1)
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.ws_wappay.wswappayrequest");
                if (paymentMode != null)
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(xml);

                    AliPayInfo info = new AliPayInfo();
                    info.Partner = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
                    info.Key = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                    info.Account = xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText;


                    StandardResult<AliPayInfo> result = new StandardResult<AliPayInfo>()
                    {
                        code = 0,
                        msg = "",
                        data = info
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "未找到配置",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }

            else
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.wx_apppay.wxwappayrequest");
                if (paymentMode != null)
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(xml);
                    WxPayInfo info = new WxPayInfo();
                    info.AppId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                    info.Key = xmlDocument.GetElementsByTagName("Key")[0].InnerText; ;
                    info.AppSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                    info.Mch_id = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;

                    StandardResult<WxPayInfo> result = new StandardResult<WxPayInfo>()
                    {
                        code = 0,
                        msg = "",
                        data = info
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "未找到配置",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
                

                      
            }
        }

        #region Private

        private AppAndroidVersionResult GetAndroidLatestVersion()
        {
            AppAndroidVersionResult result = new AppAndroidVersionResult();
            AppVersionRecordInfo appVersion = APPHelper.GetLatestAppVersionRecordOrderById("android");

            if (appVersion != null)
            {
                result = new AppAndroidVersionResult();
                result.appname = appVersion.AppName;
                result.apkname = appVersion.AppPackageName;
                result.verName = appVersion.VersionName;
                result.verCode = (int)appVersion.Version;
                result.url = Util.AppendImageHost(appVersion.UpgradeUrl);
                result.infoUrl = Util.AppendImageHost(appVersion.UpgradeInfoUrl);

                // 打开文件
                ///Storage/data/app/android/haimylife_v10.1.txt
                result.info = "";

                result.IsForcibleUpgrade = appVersion.IsForcibleUpgrade;

                result.info = appVersion.Description;
                //string upgradeInfoPath = HostingEnvironment.MapPath(appVersion.UpgradeInfoUrl);

                //if (File.Exists(upgradeInfoPath))
                //{
                //    try
                //    {
                //        using (var fs = new FileStream(upgradeInfoPath, FileMode.Open, FileAccess.Read))
                //        {
                //            int len = (int)fs.Length;
                //            byte[] bytes = new byte[len];
                //            int r = fs.Read(bytes, 0, bytes.Length);
                //            result.info = System.Text.Encoding.UTF8.GetString(bytes);
                //        }
                //    }
                //    catch(Exception ex)
                //    {
                //        Logger.WriterLogger(ex.Message);
                //    }
                //}
                

            }

            return result;
        }

        private List<RegionListItem> GetRegionDictionary()
        {
            List<RegionListItem> pItems = new List<RegionListItem>();

            Dictionary<int, string> provinces = RegionHelper.GetAllProvinces();

            RegionListItem province = null;
            foreach (var p in provinces)
            {
                province = new RegionListItem("PROVINCE", 1);

                province.Id = p.Key;
                province.Name = p.Value;
                province.ParentId = 0;

                Dictionary<int, string> cities = RegionHelper.GetCitys(province.Id);

                List<RegionListItem> cItems = new List<RegionListItem>();
                RegionListItem city = null;
                foreach (var c in cities)
                {
                    city = new RegionListItem("CITY", 2);

                    city.Id = c.Key;
                    city.Name = c.Value;
                    city.ParentId = province.Id;

                    Dictionary<int, string> districts = RegionHelper.GetCountys(city.Id);
                    List<RegionListItem> dItems = new List<RegionListItem>();
                    RegionListItem district = null;
                    foreach (var d in districts)
                    {
                        district = new RegionListItem("DISTRICT", 3);

                        district.Id = d.Key;
                        district.Name = d.Value;
                        district.ParentId = city.Id;

                        dItems.Add(district);
                    }

                    city.Children = dItems;

                    cItems.Add(city);
                }

                province.Children = cItems;

                pItems.Add(province);
            }

            return pItems;
        }

        private List<DictionaryListItem> GetFeedbackType()
        {
            List<DictionaryListItem> items = new List<DictionaryListItem>();

            items.Add(new DictionaryListItem(1, "", "配送服务", "", 100));
            items.Add(new DictionaryListItem(2, "", "意见与反馈", "", 200));
            items.Add(new DictionaryListItem(3, "", "软件报错", "", 300));
            items.Add(new DictionaryListItem(4, "", "其他", "", 400));

            return items;
        }

        private List<DictionaryListItem> GetRefundType()
        {
            List<DictionaryListItem> items = new List<DictionaryListItem>();

            items.Add(new DictionaryListItem(2, "", "原路返回", "", 200));

            return items;
        }

        private List<ExpressListItem> GetExpressCompany()
        {
            List<ExpressListItem> items = new List<ExpressListItem>();

            IList<ExpressCompanyInfo> expressList = ExpressHelper.GetAllExpress();

            if (expressList != null)
            {
                int i = 1;

                foreach (var current in expressList)
                {
                    items.Add(new ExpressListItem(current.Kuaidi100Code, current.Name, "", i));

                    i++;
                }
            }

            return items;
        }

        #endregion
    }
}
