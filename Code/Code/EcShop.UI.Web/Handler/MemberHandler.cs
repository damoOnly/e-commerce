using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using Ecdev.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using EcShop.ControlPanel.Members;
using Ecdev.Weixin.Pay.Pay;
using EcShop.ControlPanel.Sales;
using EcShop.Entities.Orders;
using System.Data;
using EcShop.Core.ErrorLog;
using System.IO;
using System.Drawing;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using EcShop.Entities.Commodities;
using EcShop.ControlPanel.Commodities;
using System.Linq;
using EcShop.SqlDal.Commodities;
using EcShop.ControlPanel.Promotions;
using System.Configuration;
using EcShop.ControlPanel.Comments;
using System.Net;
using System.Web.UI;
namespace EcShop.UI.Web.Handler
{
    public class MemberHandler : System.Web.IHttpHandler
    {
        private string message = "";
        private int cid = 0;
        private string wid = string.Empty;
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "ExistUsername":
                    this.ExistUsername(context);
                    break;
                case "ExistCellPhoneAndUserName":
                    this.ExistCellPhoneAndUserName(context);
                    break;
                case "ExistEmailAndUserName":
                    this.ExistEmailAndUserName(context);
                    break;
                case "ExistTelphoneVerifyCode":
                    this.ExistTelphoneVerifyCode(context);
                    break;
                case "ExistRecemmendCode":
                    this.ExistRecemmendCode(context);
                    break;
                case "CheckAuthcode":
                    this.CheckAuthcode(context);
                    break;
                case "RegisterMember":
                    this.RegisterMember(context);
                    break;
                case "VerficationCellphone":
                    this.VerficationCellphone(context);
                    break;
                case "VerficationEmail":
                    this.VerficationEmail(context);
                    break;
                case "RepeatEmail":
                    this.RepeatEmail(context);
                    break;
                case "UpdateFavorite":
                    this.UpdateFavorite(context);
                    break;
                case "BindFavorite":
                    this.BindFavorite(context);
                    break;
                case "DelteFavoriteTags":
                    this.DelteFavoriteTags(context);
                    break;
                case "AddFavorite":
                    this.AddProductToFavorite(context);
                    break;
                case "DelFavorite":
                    this.DeleteFavorite(context);
                    break;
                case "ChageQuantity":
                    this.ChageQuantity(context);
                    break;
                case "SendRegisterEmail":
                    this.SendRegisterEmail(context);
                    break;
                case "SendRegisterTel":
                    this.SendRegisterTel(context);
                    break;
                case "GetVCodePayState":
                    this.GetVCodePayState(context);
                    break;
                case "CheckIsFirstOrder":
                    this.CheckIsFirstOrder(context);
                    break;
                case "GetReturnProducts":
                    this.GetReturnProducts(context);
                    break;
                case "RemoveAppImg":
                    RemoveAppImg(context);
                    break;
                case "UpLoadAppImg":
                    UpLoadAppImg(context);
                    break;
                case "BindAreaList":
                    BindAreaList(context);
                    break;
                case "GetBinList":
                    GetBindList(context);
                    break;
                case "SelectDownCategories": //获取限时抢购项
                    SelectDownCategories(context);
                    break;
                case "GetDownCateProducts"://根据活动类型获取商品列
                    GetDownCateProducts(context);
                    break;
                case "GetActiveProducts"://获取活动中的前6件商品
                    GetActiveProducts(context);
                    break;
                case "GetUserInfo":
                    GetUserInfo(context);
                    break;
                case "CheckPurchase":
                    this.CheckPurchase(context);
                    break;
                case "CartDelete":
                    this.CartDelete(context);
                    break;
                case "GetOrderItemInfo":
                    this.GetOrderItemInfo(context);
                    break;
            }
            context.Response.Write(this.message);
        }
        /// <summary>
        /// 根据删除购物车的信息获取对应的商品信息
        /// </summary>
        /// <param name="context"></param>
        private void CartDelete(System.Web.HttpContext context)
        {
            string skuId = context.Request["productSkuId"];
            DataTable dt = ProductHelper.GetAdOrderInfo(skuId);
            if (dt != null && dt.Rows.Count > 0)
            {
                context.Response.Write("{\"Status\":\"OK\",\"data\":"+Newtonsoft.Json.JsonConvert.SerializeObject(dt)+"}");
            } 
            else
            { 
                context.Response.Write("{\"Status\":\"No\",\"data\":\"\"}");
            }
        }
        /// <summary>
        /// 根据订单号获取商品明细推送给广告商
        /// </summary>
        /// <param name="context"></param>
        private void GetOrderItemInfo(System.Web.HttpContext context)
        {

            string OrderId = context.Request["orderId"];
            DataTable dt = ProductHelper.GetAdOrderProductByOrderId(OrderId);
            if (dt != null && dt.Rows.Count > 0)
            {
                context.Response.Write("{\"Status\":\"OK\",\"data\":" + Newtonsoft.Json.JsonConvert.SerializeObject(dt) + "}");
            }
            else
            {
                context.Response.Write("{\"Status\":\"No\",\"data\":\"\"}");
            }
        }
        /// <summary>
        /// 检查限购
        /// </summary>
        /// <param name="context"></param>
        private void CheckPurchase(System.Web.HttpContext context)
        {
            string skuId = context.Request["productSkuId"];
            int quantity = 0;
            int.TryParse(context.Request["quantity"], out quantity);
            //检查商品是否超过限购数量
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int MaxCount = 0;
                int Payquantity = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                if ((Payquantity + quantity) > MaxCount && MaxCount != 0) //当前购买数量大于限购剩余购买数量
                {
                    context.Response.Write("{\"Status\":\"4\"}");
                    return;
                }
            }
            context.Response.Write("{\"Status\":\"0\"}");
        }

        private void GetUserInfo(System.Web.HttpContext context)
        {
            DataTable dt = SitesManagementHelper.GetMySubMemberByUserId(HiContext.Current.User.UserId);
            StringBuilder text = new StringBuilder("[");
            string DownCategoriesStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            text.Append("]");
            this.message = "{\"data\":" + DownCategoriesStr + "}";
        }
        /// <summary>
        /// 获取活动中的前6件商品
        /// </summary>
        /// <param name="context"></param>
        private void GetActiveProducts(System.Web.HttpContext context)
        {
            DataTable dt = PromoteHelper.GetDownCateProducts(0, 0);
            StringBuilder text = new StringBuilder("[");
            string DownCategoriesStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            text.Append("]");
            this.message = "{\"data\":" + DownCategoriesStr + "}";
        }
        /// <summary>
        /// 根据获取类型获取商品列表
        /// </summary>
        /// <param name="context"></param>
        private void GetDownCateProducts(System.Web.HttpContext context)
        {
            int CountDownCateGoryId = 0;
            int.TryParse(context.Request.QueryString["CountDownCateGoryId"], out CountDownCateGoryId);
            DataTable dt = PromoteHelper.GetDownCateProducts(CountDownCateGoryId, 0);
            StringBuilder text = new StringBuilder("[");
            string DownCategoriesStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            text.Append("]");
            this.message = "{\"data\":" + DownCategoriesStr + "}";
        }
        /// <summary>
        /// 获取限时抢购管理项
        /// </summary>
        /// <param name="context"></param>
        private void SelectDownCategories(System.Web.HttpContext context)
        {
            DataTable dt = (DataTable)ProductBrowser.GetCountDownCategories(1, 5).Data; //PromoteHelper.SelectCountDownCategories(); 
            StringBuilder text = new StringBuilder("[");
            string DownCategoriesStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            text.Append("]");
            this.message = "{\"data\":" + DownCategoriesStr + "}";
        }
        /// <summary>
        /// 删除指定app启动图
        /// </summary>
        /// <param name="context"></param>
        private void RemoveAppImg(System.Web.HttpContext context)
        {
            string imgName = context.Request.QueryString["imageName"];
            string path = Globals.ApplicationPath + imgName;
            string mapPath = context.Server.MapPath(path);
            //string savePath = mapPath;
            if (File.Exists(mapPath))
            {
                File.Delete(mapPath);
                this.message = "{\"success\":\"SUCCESS\",\"MSG\":\"\"}";
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"找不到文件\"}";
            }

        }
        /// <summary>
        /// 添加指定app启动图
        /// </summary>
        /// <param name="context"></param>
        private void UpLoadAppImg(System.Web.HttpContext context)
        {
            //context.Response.ContentType = "json";
            //获取前台的FILE  
            HttpPostedFile file = context.Request.Files["fileUpload"];

            string mapPath = Globals.MapPath("/Storage/master/app/");

            string fileUuid = Guid.NewGuid().ToString().ToLower();

            string fileName = Path.GetFileName(file.FileName);
            string ext = fileName.Substring(fileName.LastIndexOf('.') + 1);

            string savePath = mapPath + fileUuid + "." + ext;

            file.SaveAs(savePath);

            if (File.Exists(savePath))
            {
                this.message = "{\"success\":\"SUCCESS\",\"name\":\"" + fileUuid + "\",\"suffix\":\"" + ext + "\"}";
            }
            else
            {
                this.message = "{'success':'NO','MSG':''}";
            }
        }
        /// <summary>
        /// 获取PC指定orderId的订单信息
        /// </summary>
        /// <param name="context"></param>
        private void GetReturnProducts(System.Web.HttpContext context)
        {
            string orderId = context.Request.QueryString["OrderId"];
            DataTable dt = ShoppingProcessor.GetOrderItems(orderId);
            StringBuilder text = new StringBuilder("[");
            List<string> list = new List<string>();

            string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            //foreach (DataRow r in dt.Rows)
            //{
            //    StringBuilder row = new StringBuilder("{");
            //    //SkuId,ItemDescription,sku,SKUContent,Quantity,isnull(TaxRate,0) TaxRate,ProductId,ThumbnailsUrl,ItemListPrice,ItemAdjustedPrice
            //    row.AppendFormat("\"SkuId\":\"{0}\"", r["SkuId"]);
            //    row.AppendFormat(",\"ItemDescription\":\"{0}\"", r["ItemDescription"]);
            //    row.AppendFormat(",\"sku\":\"{0}\"", r["sku"]);
            //    row.AppendFormat(",\"SKUContent\":\"{0}\"", r["SKUContent"]);
            //    row.AppendFormat(",\"Quantity\":\"{0}\"", r["Quantity"]);
            //    row.AppendFormat(",\"TaxRate\":\"{0}\"", r["TaxRate"]);
            //    row.AppendFormat(",\"ProductId\":\"{0}\"", r["ProductId"]);
            //    row.AppendFormat(",\"ThumbnailsUrl\":\"{0}\"", r["ThumbnailsUrl"]);
            //    row.AppendFormat(",\"ItemListPrice\":\"{0}\"", r["ItemListPrice"]);
            //    row.AppendFormat(",\"ItemAdjustedPrice\":\"{0}\"", r["ItemAdjustedPrice"]);
            //    row.Append("}");
            //    list.Add(row.ToString());
            //}
            //text.Append(string.Join(",", list));
            text.Append("]");
            this.message = "{\"data\":" + strProducts + "}";
        }
        /// <summary>
        /// 微信扫码支付状态获取
        /// </summary>
        /// <param name="context"></param>
        private void GetVCodePayState(System.Web.HttpContext context)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);

            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest");

            string appId = "";
            string partnerKey = "";
            string appSecret = "";
            string partnerId = "";

            if (paymentMode != null)
            {
                if (paymentMode.Settings != "")
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                    try
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);

                        appId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        partnerKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        appSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        partnerId = xmlDocument.GetElementsByTagName("MchId")[0].InnerText;
                    }
                    catch
                    {
                        ErrorLog.Write("微信扫码支付未设置");
                        return;
                    }
                }
            }

            string orderId = context.Request["orderId"];
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("appid", appId);
            dic.Add("mch_id", partnerId);
            dic.Add("out_trade_no", orderId);
            dic.Add("nonce_str", VCodePayHelper.CreateRandom(20));
            VCodePayHelper.key = partnerKey;
            //OrderInfo orderinfo = OrderHelper.GetOrderInfo(text);
            //if ((int)orderinfo.OrderStatus >= 2)
            //{
            //    this.message = "{ \"state\":\"NO\"}";
            //    return;
            //}
            //微信支付订单号
            string transaction_id;

            string state = VCodePayHelper.GetOrderState(dic, null, out transaction_id);

            if (state == "SUCCESS")
            {
                //支付成功扣商品库存
                OrderHelper.DebuctFactStock(orderId);

                /*
                //支付成功修改订单支付状态
                OrderHelper.SetOrderPayStatus(orderId, 2);
                //修改订单状态
                OrderHelper.UpdateOrderStatus(orderId, transaction_id, 2);
                */

                OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);
                order.GatewayOrderId = transaction_id;

                this.UserPayOrder(order);

                // 反写销售数量
                //ConFirmPay(orderId);
                sw.Stop();

                #region 推送广告信息
                try
                {
                    if (System.Web.HttpContext.Current.Request.Cookies["AdCookies_cid"] != null && System.Web.HttpContext.Current.Request.Cookies["AdCookies_wi"] != null)
                    {
                        int.TryParse(context.Request.Cookies["AdCookies_cid"].Value.ToString(), out cid);
                        wid = context.Request.Cookies["AdCookies_wi"].Value.ToString();
                        Action ac = new Action(() =>
                        {
                            List<orderStatus> orderst=new List<orderStatus>();
                            orderStatus adInfo = new orderStatus();
                            adInfo.orderNo = orderId;
                            adInfo.feedback = wid;
                            adInfo.updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
                            adInfo.orderstatus = "active";
                            adInfo.paymentStatus = "2";
                            adInfo.paymentType = order.PaymentType;
                            orderst.Add(adInfo);
                            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(orderst);

                            string url = "http://o.yiqifa.com/servlet/handleCpsInterIn";
                            string DataStr = "interId=519200e5e03bbcaa579e8b04&json=" + HttpUtility.UrlEncode(jsonStr) + "&encoding=UTF-8";
                            string RqRest = HttpGet(url, DataStr);
                            ErrorLog.Write("推送CPS返回结果：" + RqRest.ToString());
                        });
                        ac.BeginInvoke(null, ac);
                    }
                    else
                    {
                        ErrorLog.Write("没有获取Cookies值！");
                    }
                }
                catch (Exception ee)
                { }
                #endregion

                ErrorLog.Write("微信扫码支付所用的时间（毫秒）:" + sw.ElapsedMilliseconds.ToString());
            }
            this.message = "{ \"state\":\"" + state + "\"}";
        }
        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("推送CPS返回结果：" + ee.Message, ee);
                return ee.Message.ToString();
            }
        }
        private void ConFirmPay(string orderid)
        {
            ProductDao productDao = new ProductDao();
            OrderInfo order = OrderHelper.GetOrderInfo(orderid);
            foreach (LineItemInfo current in order.LineItems.Values)
            {
                ProductInfo productDetails = productDao.GetProductDetails(current.ProductId);
                productDetails.SaleCounts += current.Quantity;
                productDetails.ShowSaleCounts += current.Quantity;
                productDao.UpdateProduct(productDetails, null);
            }
        }
        private void ExistUsername(System.Web.HttpContext context)
        {
            string text = context.Request["username"].ToLower();
            if (string.IsNullOrEmpty(text.ToLower()))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"请输入要检验的用户名\"}";
                return;
            }
            if (UserHelper.IsExistUserName(text))
            {
                this.message = "{\"success\":true,\"msg\":\"用户名已存在\"}";
                return;
            }
            this.message = "{\"success\":false,\"msg\":\"用户名可用\"}";
        }


        private void ExistCellPhoneAndUserName(System.Web.HttpContext context)
        {
            string cellPhone = context.Request["cellPhone"].ToLower();
            if (string.IsNullOrEmpty(cellPhone))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"请输入要检验的手机号码\"}";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(cellPhone, "^(13|14|15|17|18)\\d{9}$"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的手机号码\"}";
                return;
            }
            int result = UserHelper.IsExistCellPhoneAndUserName(cellPhone);
            if (result > 0)
            {
                this.message = "{\"success\":false,\"msg\":\"手机号码已存在\"}";
                return;
            }
            else
            {
                this.message = "{\"success\":true,\"msg\":\"手机号码可用\"}";
            }

        }

        private void ExistEmailAndUserName(System.Web.HttpContext context)
        {
            string email = context.Request["email"].ToLower();
            if (string.IsNullOrEmpty(email))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"请输入要检验的邮箱\"}";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的邮箱账号\"}";
                return;
            }

            int result = UserHelper.IsExistEmailAndUserName(email);
            if (result > 0)
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱已存在\"}";
                return;
            }
            else
            {
                this.message = "{\"success\":true,\"msg\":\"邮箱可用\"}";
            }

        }

        private void ExistTelphoneVerifyCode(System.Web.HttpContext context)
        {
            string cellphone = context.Request["Cellphone"];
            string phoneVerifyCode = context.Request["CellphoneVerifyCode"].ToLower();
            if (string.IsNullOrEmpty(cellphone))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"请输入手机号码\"}";
                return;
            }
            if (string.IsNullOrEmpty(phoneVerifyCode))
            {
                this.message = "{\"success\":\"false\",\"msg\":\"请输入验证码\"}";
                return;
            }

            if (!TelVerifyHelper.CheckVerify(cellphone, phoneVerifyCode))
            {
                this.message = "{\"success\":false,\"msg\":\"手机验证码验证错误\"}";
                return;
            }
            else
            {
                this.message = "{\"success\":true,\"msg\":\"手机验证码验证成功\"}";
            }

        }

        private void ExistRecemmendCode(System.Web.HttpContext context)
        {
            string text = context.Request["code"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入邀请码\"}";
                return;
            }

            string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
            string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
            string currcode = BaseConvertHelper.BaseConvert(text.Trim().ToUpper(), newsourcechars, sourcechars);
            string useredId = MemberHelper.GetUserIdByRecommendCode(currcode);
            if (string.IsNullOrWhiteSpace(useredId))
            {
                this.message = "{\"success\":false,\"msg\":\"没有该邀请码\"}";
                return;
            }
            this.message = "{\"success\":true,\"msg\":\"邀请码输入成功\",\"UseredId\":" + useredId + ",\"RecemmendCode\":" + currcode + "}";
        }

        private void CheckAuthcode(System.Web.HttpContext context)
        {
            string text = context.Request["code"];
            if (context.Request.Cookies["VerifyCode"] == null)
            {
                this.message = "{\"success\":false,\"msg\":\"请重新生成验证码\"}";
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入验证码\"}";
                return;
            }
            if (string.Compare(HiCryptographer.Decrypt(context.Request.Cookies["VerifyCode"].Value), text, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
            {
                this.message = "{\"success\":true}";
                return;
            }
            this.message = "{\"success\":false,\"msg\":\"验证码输入不正确\"}";
        }
        private void RegisterMember(System.Web.HttpContext context)
        {
            int regType;
            int.TryParse(context.Request.Form["register$regType"], out regType);

            if (this.CheckRegisteParam(context, regType))
            {
                Member member = new Member(UserRole.Member);
                if (HiContext.Current.ReferralUserId > 0)
                {
                    member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
                }
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();

                string cellPhone = context.Request["register$txtCellPhone"];
                string email = context.Request["register$txtEmail"];

                //注册类型（1为手机注册，2为邮箱注册）
                if (regType == 1)
                {
                    member.Username = cellPhone;
                    member.Email = "";// cellPhone + "@mail.haimylife.com";
                    member.CellPhone = cellPhone;
                    member.CellPhoneVerification = true;
                    //member.MembershipUser.MobilePIN = cellPhone;
                }
                else if (regType == 2)
                {
                    member.Username = email;
                    member.Email = email;
                    member.CellPhone = "";
                }

                member.Password = context.Request.Form["register$txtPassword"];
                member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
                member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
                member.TradePassword = context.Request["register$txtPassword"];
                member.IsApproved = true;
                member.RealName = string.Empty;
                member.Address = string.Empty;
                member.UserType = UserType.PC;
                member.CreateDate = DateTime.Now;

                if (!this.ValidationMember(member))
                {
                    return;
                }
                CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);
                CreateUserStatus createUserStatus2 = createUserStatus;
                switch (createUserStatus2)
                {
                    case CreateUserStatus.UnknownFailure:
                        this.message = "{\"success\":false,\"msg\":\"未知错误\"}";
                        return;
                    case CreateUserStatus.Created:
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(member.Username, "^(13|14|15|17|18)\\d{9}$"))
                            {
                                member.CellPhone = member.Username;
                            }
                            if (System.Text.RegularExpressions.Regex.IsMatch(member.Username, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                            {
                                member.Email = member.Username;
                            }
                            Messenger.UserRegister(member, member.Password);
                            member.OnRegister(new UserEventArgs(member.Username, member.Password, null));
                            IUser user = Users.GetUser(0, member.Username, false, true);
                            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                            if (shoppingCart != null)
                            {
                                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                                ShoppingCartProcessor.ClearCookieShoppingCart();
                            }
                            HiContext.Current.User = user;
                            if (shoppingCart != null)
                            {
                                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                            }
                            System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
                            IUserCookie userCookie = user.GetUserCookie();
                            userCookie.WriteCookie(authCookie, 30, false);
                            #region ==判断是否有优惠卷
                            MemberHelper.NewUserSendRegisterCoupon(member);
                            #endregion
                            #region == 判断是否有邀请码
                            string systemcode = context.Request.Form["register$txtRecommendCode"];
                            if (!string.IsNullOrEmpty(systemcode))
                            {
                                string sourcechars = ConfigurationManager.AppSettings["sourcechars"];
                                string newsourcechars = ConfigurationManager.AppSettings["newsourcechars"];
                                string currcode = BaseConvertHelper.BaseConvert(systemcode.ToUpper(), newsourcechars, sourcechars);
                                string useredId = MemberHelper.GetUserIdByRecommendCode(currcode);
                                if (!string.IsNullOrWhiteSpace(useredId))
                                {
                                    // 插入到邀请码记录表
                                    MemberHelper.AddRecommendCodeRecord(member.UserId, Convert.ToInt32(useredId), currcode, systemcode);
                                }
                            }
                           
                            #endregion
                            string urlToEncode = "/User/UserDefault.aspx";
                            if (HiContext.Current.ReferralUserId > 0)
                            {
                                urlToEncode = "/User/ReferralRegisterAgreement.aspx";
                            }
                            this.message = "{\"success\":true,\"msg\":\"" + Globals.UrlEncode(urlToEncode) + "\",\"userId\":\"" + member.Username + "\"}";

                            break;
                        }
                    case CreateUserStatus.DuplicateUsername:
                        this.message = "{\"success\":false,\"msg\":\"已经存在相同的用户名\"}";
                        return;
                    case CreateUserStatus.DuplicateEmailAddress:
                    case CreateUserStatus.InvalidFirstCharacter:
                        break;
                    case CreateUserStatus.DisallowedUsername:
                        this.message = "{\"success\":false,\"msg\":\"用户名禁止注册\"}";
                        return;
                    default:
                        if (createUserStatus2 != CreateUserStatus.InvalidPassword)
                        {
                            return;
                        }
                        this.message = "{\"success\":false,\"msg\":\"无效的密码\"}";
                        return;
                }
            }
        }
        private bool CheckRegisteParam(System.Web.HttpContext context, int regType)
        {
            if (context.Request.Form["register$chkAgree"] != "on")
            {
                this.message = "{\"success\":false,\"您必须先阅读并同意注册协议\"}";
                return false;
            }
            //注册类型（1为手机注册，2为邮箱注册）
            if (regType == 1)
            {
                string cellphone = context.Request.Form["register$txtCellPhone"].Trim().ToLower();
                string telphoneVerifyCode = context.Request.Form["register$txtTelphoneVerifyCode"].Trim().ToLower();
                if (string.IsNullOrEmpty(telphoneVerifyCode))
                {
                    this.message = "{\"success\":false,\"msg\":\"手机验证码不允许为空\"}";
                    return false;
                }

                if (!TelVerifyHelper.CheckVerify(cellphone, telphoneVerifyCode))
                {
                    this.message = "{\"success\":false,\"msg\":\"手机验证码验证错误\"}";
                    return false;
                }

                if (string.IsNullOrEmpty(cellphone))
                {
                    this.message = "{\"success\":false,\"msg\":\"手机号码不允许为空\"}";
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(cellphone, "^(13|14|15|17|18)\\d{9}$"))
                {
                    this.message = "{\"success\":false,\"msg\":\"请输入正确的手机号码\"}";
                    return false;
                }
                if (UserHelper.IsExistCellPhoneAndUserName(cellphone) > 0)
                {
                    this.message = "{\"success\":false,\"msg\":\"已经存在相同的手机号码\"}";
                    return false;
                }

            }
            else if (regType == 2)
            {
                if (!HiContext.Current.CheckVerifyCode(context.Request.Form["register$txtNumber"]))
                {
                    this.message = "{\"success\":false,\"msg\":\"验证码输入错误\"}";
                    return false;
                }

                string email = context.Request.Form["register$txtEmail"].Trim().ToLower();
                if (string.IsNullOrEmpty(email))
                {
                    this.message = "{\"success\":false,\"msg\":\"邮箱不允许为空\"}";
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                {
                    this.message = "{\"success\":false,\"msg\":\"请输入正确的邮箱账号\"}";
                    return false;
                }
                if (UserHelper.IsExistEmailAndUserName(email) > 0)
                {
                    this.message = "{\"success\":false,\"msg\":\"已经存在相同的邮箱\"}";
                    return false;
                }
            }

            if (string.Compare(context.Request.Form["register$txtPassword"], context.Request.Form["register$txtPassword2"]) != 0)
            {
                this.message = "{\"success\":false,\"msg\":\"两次输入的密码不相同\"}";
                return false;
            }
            if (context.Request.Form["register$txtPassword"].Length == 0)
            {
                this.message = "{\"success\":false,\"msg\":\"密码不能为空\"}";
                return false;
            }
            if (context.Request.Form["register$txtPassword"].Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength || context.Request.Form["register$txtPassword"].Length > HiConfiguration.GetConfig().PasswordMaxLength)
            {
                this.message = string.Format("{{\"success\":false,\"密码的长度只能在{0}和{1}个字符之间\"}}", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, HiConfiguration.GetConfig().PasswordMaxLength);
                return false;
            }
            if (context.Request.Form["register$txtRecommendCode"].Length > 0)
            {
                if (context.Request.Form["register$hiduseredId"].Length == 0)
                {
                    this.message = "{\"success\":false,\"msg\":\"邀请码不正确\"}";
                    return false;
                }
            }
            return true;
        }
        private bool ValidationMember(Member member)
        {
            ValidationResults validationResults = Validation.Validate<Member>(member, new string[]
			{
				"ValMember"
			});
            string text = string.Empty;
            if (!validationResults.IsValid)
            {
                foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
                {
                    text += Formatter.FormatErrorMessage(current.Message);
                }
                this.message = "{\"success\":false,\"msg\":\"" + text + "\"}";
            }
            return validationResults.IsValid;
        }
        private void VerficationCellphone(System.Web.HttpContext context)
        {
            if (HiContext.Current.User.IsAnonymous || HiContext.Current.User == null)
            {
                this.message = "{\"success\":false,\"msg\":\"请先登录\"}";
            }
            string text = context.Request["cellphone"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"手机号码不允许为空\"}";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, "^(13|14|15|17|18)\\d{9}$"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的手机号码\"}";
                return;
            }
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"手机服务未配置\"}";
                return;
            }
            this.SendMessage(siteSettings, text);
        }
        private void RepeatEmail(System.Web.HttpContext context)
        {
            string text = context.Request["email"];
            string username = context.Request["username"];
            if (text == null)
            {
                return;
            }
            if (UserHelper.IsExistEmal(text, username))
            {
                this.message = "{\"success\":true,\"msg\":\"邮箱已被使用过\"}";
                return;
            }
            this.message = "{\"success\":false,\"msg\":\"邮箱可用\"}";
        }
        private void VerficationEmail(System.Web.HttpContext context)
        {
            if (HiContext.Current.User.IsAnonymous || HiContext.Current.User == null)
            {
                this.message = "{\"success\":false,\"msg\":\"请先登录\"}";
            }
            string text = context.Request["email"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱账号不允许为空\"}";
                return;
            }
            if (text.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(text, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的邮箱账号\"}";
                return;
            }
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱服务未配置\"}";
                return;
            }
            int userIdByEmail = UserHelper.GetUserIdByEmail(text);
            if (userIdByEmail != HiContext.Current.User.UserId && userIdByEmail != 0)
            {
                this.message = "{\"success\":false,\"msg\":\"该邮箱已被其它用户使用了,请更换其它邮箱！\"}";
                return;
            }
            this.SendEmail(siteSettings, text);
        }

        private void SendRegisterEmail(System.Web.HttpContext context)
        {
            string text = context.Request["email"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱账号不允许为空\"}";
                return;
            }
            if (text.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(text, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的邮箱账号\"}";
                return;
            }
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"邮箱服务未配置\"}";
                return;
            }
            int userIdByEmail = UserHelper.GetUserIdByEmail(text);
            if (userIdByEmail != HiContext.Current.User.UserId && userIdByEmail != 0)
            {
                this.message = "{\"success\":false,\"msg\":\"该邮箱已被其它用户使用了,请更换其它邮箱！\"}";
                return;
            }
            this.SendEmail(siteSettings, text);
        }
        private void SendMessage(SiteSettings settings, string cellphone)
        {
            try
            {
                string text = HiContext.Current.CreateVerifyCode(4);
                ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.SMSSettings));
                SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
                string text2 = string.Format("尊敬的会员{0}您好：欢迎使用" + settings.SiteName + "系统，此次验证码为：{1},请在3分钟内完成验证", HiContext.Current.User.Username, text);
                string text3;
                bool flag = sMSSender.Send(cellphone, text2, out text3);
                if (flag)
                {
                    HiCache.Insert(HiContext.Current.User.UserId + "cellphone", text, 10800);
                }
                //this.message = string.Concat(new object[]
                //{
                //    "{\"success\":",
                //    flag,
                //    ",\"msg\":\"",
                //    text3,
                //    "\"}"
                //});
                this.message = "{\"success\":true,\"msg\":\"" + text3 + "\"}";
            }
            catch (System.Exception)
            {
                this.message = "{\"success\":false,\"msg\":\"未知错误\"}";
            }
        }
        private void SendEmail(SiteSettings settings, string email)
        {
            try
            {
                string text = HiContext.Current.CreateVerifyCode(4);
                ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.EmailSettings));
                string body = string.Format("尊敬的会员{0}您好：欢迎使用" + settings.SiteName + "系统，此次验证码为：{1},请在3分钟内完成验证", HiContext.Current.User.Username, text);
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
                {
                    IsBodyHtml = true,
                    Priority = System.Net.Mail.MailPriority.High,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    Body = body,
                    Subject = "来自" + settings.SiteName
                };
                mailMessage.To.Add(email);
                EmailSender emailSender = EmailSender.CreateInstance(settings.EmailSender, configData.SettingsXml);
                if (emailSender.Send(mailMessage, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                {
                    this.message = "{\"success\":true,\"msg\":\"发送邮件成功，请查收\"}";
                    HiCache.Insert(HiContext.Current.User.UserId + "email", text, 10800);
                }
                else
                {
                    this.message = "{\"success\":false,\"msg\":\"发送邮件失败，请检查邮箱账号是否存在\"}";
                }
            }
            catch (System.Exception)
            {
                this.message = "{\"success\":false,\"msg\":\"发送失败，请检查邮箱账号是否存在\"}";
            }
        }
        protected void UpdateFavorite(System.Web.HttpContext context)
        {
            string value = context.Request.Form["favoriteid"];
            string text = context.Request.Form["tags"];
            if (text.Contains(",") && text.Split(new char[]
			{
				','
			}).Length >= 4)
            {
                this.message = "{\"success\":false,\"msg\":\"最多输入3个标签\"}";
                return;
            }
            if (ProductBrowser.UpdateFavorite(System.Convert.ToInt32(value), text, "") > 0)
            {
                this.message = "{\"success\":true}";
            }
        }
        protected void BindFavorite(System.Web.HttpContext context)
        {
            string favoriteTags = ProductBrowser.GetFavoriteTags();
            if (!string.IsNullOrEmpty(favoriteTags))
            {
                this.message = "{\"success\":true,\"msg\":[" + favoriteTags + "]}";
                return;
            }
            this.message = "{\"success\":false}";
        }
        protected void DelteFavoriteTags(System.Web.HttpContext context)
        {
            string text = context.Request.Form["tagname"];
            if (string.IsNullOrEmpty(text))
            {
                this.message = "{\"success\":false,\"msg\":2}";
                return;
            }
            if (ProductBrowser.DeleteFavoriteTags(text) <= 0)
            {
                this.message = "{\"success\":false,\"msg\":1}";
                return;
            }
            string favoriteTags = ProductBrowser.GetFavoriteTags();
            if (!string.IsNullOrEmpty(favoriteTags))
            {
                this.message = "{\"success\":true,\"msg\":[" + favoriteTags + "]}";
                return;
            }
            this.message = "{\"success\":false,\"msg\":0}";
        }
        protected void AddProductToFavorite(System.Web.HttpContext context)
        {
            try
            {
                if (!(HiContext.Current.User is Member))
                {
                    this.message = "{\"success\":false,\"msg\":1}";
                }
                else
                {
                    int productId = 0;
                    int.TryParse(context.Request.Form["ProductId"], out productId);
                    int num = ProductBrowser.GetUserFavoriteCount() + 1;
                    int collectNum = ProductBrowser.GetFavoriteCountByProductId(productId) + 1;
                    if (!ProductBrowser.ExistsProduct(productId, HiContext.Current.User.UserId))
                    {
                        int num2 = ProductBrowser.AddProduct(productId, HiContext.Current.User.UserId);
                        if (num2 > 0)
                        {
                            this.message = string.Concat(new object[]
							{
								"{\"success\":true,\"favoriteid\":\"",
								num2,
								"\",\"Count\":\"",
								num,
                                "\",\"CollectNum\":",
								collectNum,
								"}"
							});
                        }
                        else
                        {
                            this.message = "{\"success\":false}";
                        }
                    }
                    else
                    {
                        this.message = "{\"success\":false,\"Count\":" + num + ",\"CollectNum\":" + collectNum + "}";
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.message = "{\"success\":false,\"msg\":" + ex.Message + "}";
            }
        }
        protected void DeleteFavorite(System.Web.HttpContext context)
        {
            int favoriteId = System.Convert.ToInt32(context.Request.Form["favoriteid"]);
            if (ProductBrowser.DeleteFavorite(favoriteId) > 0)
            {
                this.message = "{\"success\":true}";
                return;
            }
            this.message = "{\"success\":false}";
        }
        protected void ChageQuantity(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int num = 1;
            int.TryParse(context.Request["quantity"], out num);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            int skuStock = ShoppingCartProcessor.GetSkuStock(skuId);
            if (num > skuStock)
            {
                stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                num = skuStock;
            }
            else
            {
                stringBuilder.Append("\"Status\":\"OK\",");
                ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num > 0) ? num : 1);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\"", shoppingCart.GetAmount());
            }
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }

        private void SendRegisterTel(System.Web.HttpContext context)
        {
            string token = context.Request["token"];
            var cookie = context.Request.Cookies.Get("__RequestVerificationToken");
            string cellphone = context.Request["cellphone"];
            //string type = context.Request["type"];
            ErrorLog.Write("token:" + token + " &cookie:" + cookie);
            string ipAddress = "";

            try
            {
                ipAddress = Globals.IPAddress;
            }
            catch (Exception ex)
            {
                ErrorLog.Write("SendRegisterTelX：" + ex.Message);
            }

            ErrorLog.Write(string.Format("SendRegister: IP-{0}, Mobile-{1}", ipAddress, cellphone));

            if (cookie != null && token != null)
            {
                System.Web.Helpers.AntiForgery.Validate(cookie.Value, token);
            }
            else
            {
                this.message = "{\"success\":false,\"msg\":\"非法调用\"}";
                ErrorLog.Write(string.Format("SendRegisterTelXX: IP-{0}, Mobile-{1}", ipAddress, cellphone));
                ErrorLog.Write("SendRegisterTelXXX: " + this.message);
                return;
            }

            //1 代表为PC端注册 2 为WAP端注册
            //if (type == "1")
            //{
            //    if (!HiContext.Current.CheckVerifyCode(context.Request["code"]))
            //    {
            //        this.message = "{\"success\":false,\"msg\":\"验证码输入错误\"}";
            //        ErrorLog.Write("SendRegisterTelX: " + this.message);
            //        return;
            //    }

            //}
            ErrorLog.Write("end:end");
            if (string.IsNullOrEmpty(cellphone))
            {
                this.message = "{\"success\":false,\"msg\":\"手机号码不允许为空\"}";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(cellphone, "^(13|14|15|17|18)\\d{9}$"))
            {
                this.message = "{\"success\":false,\"msg\":\"请输入正确的手机号码\"}";
                return;
            }
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
            {
                this.message = "{\"success\":false,\"msg\":\"手机服务未配置\"}";
                return;
            }

            if (UserHelper.IsExistCellPhoneAndUserName(cellphone) > 0)
            {
                this.message = "{\"success\":false,\"msg\":\"已经存在相同的手机号码\"}";
                return;
            }
            this.HaiMeiSendMessage(siteSettings, cellphone);
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="cellphone"></param>
        private void HaiMeiSendMessage(SiteSettings settings, string cellphone)
        {
            try
            {
                ErrorLog.Write("SendRegisterTel");

                string ipAddress = "";

                try
                {
                    ipAddress = Globals.IPAddress;
                    ErrorLog.Write("SendRegisterTel: " + ipAddress);
                }
                catch (Exception ex)
                {
                    ErrorLog.Write("SendRegisterTel：" + ex.Message);
                }

                ErrorLog.Write(string.Format("SendRegisterTel: IP-{0}, Mobile-{1}", ipAddress, cellphone));

                string cacheCellphone = HiCache.Get("REG_CODE_" + cellphone) as string;
                if (cacheCellphone != null)
                {
                    DateTime dt = DateTime.MinValue;

                    if (DateTime.TryParse(cacheCellphone, out dt))
                    {
                        if (dt.AddSeconds(60) > DateTime.Now)
                        {
                            this.message = "{\"success\":false,\"msg\":\"该号码调用太快\"}";
                            ErrorLog.Write("SendRegisterTel: " + this.message);
                            return;
                        }
                    }
                }

                object cacheCellphoneTimes = HiCache.Get("REG_CODE_TIMES_" + cellphone);
                int times = 0;
                if (cacheCellphoneTimes != null)
                {
                    try
                    {
                        times = Convert.ToInt32(cacheCellphoneTimes);

                        if (times >= 7)
                        {
                            this.message = "{\"success\":false,\"msg\":\"该号码调用太多(7)\"}";
                            ErrorLog.Write("SendRegisterTel: " + this.message);
                            return;
                        }
                    }
                    catch (Exception ex2)
                    {
                        ErrorLog.Write("SendRegisterTel(ex2)：" + ex2.Message);
                    }
                }

                int ipTimes = 0;

                if (!string.IsNullOrWhiteSpace(ipAddress))
                {
                    string cacheIp = HiCache.Get("REG_SMS_IP_" + ipAddress) as string;
                    if (cacheIp != null)
                    {
                        DateTime dt = DateTime.MinValue;

                        if (DateTime.TryParse(cacheIp, out dt))
                        {
                            if (dt.AddSeconds(60) > DateTime.Now)
                            {
                                this.message = "{\"success\":false,\"msg\":\"该地址调用太快(" + ipAddress + ")\"}";
                                ErrorLog.Write("SendRegisterTel: " + this.message);
                                return;
                            }
                        }
                    }

                    object cacheIpTimes = HiCache.Get("REG_SMS_IP_TIMES_" + ipAddress);

                    if (cacheIpTimes != null)
                    {
                        try
                        {
                            ipTimes = Convert.ToInt32(cacheIpTimes);

                            if (ipTimes >= 10)
                            {
                                this.message = "{\"success\":false,\"msg\":\"该地址调用太多(10)\"}";
                                ErrorLog.Write("SendRegisterTel: " + this.message + "，" + ipAddress);
                                return;
                            }
                        }
                        catch (Exception ex3)
                        {
                            ErrorLog.Write("SendRegisterTel(ex3)：" + ex3.Message);
                        }
                    }
                }

                string code = HiContext.Current.GenerateRandomNumber(4);
                ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings.SMSSettings));
                SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
                //发送6位数字自动走短信验证码通道
                //string text2 = string.Format(@"您好！您正在进行海美生活会员注册，本次的验证码为:{0}，请勿向任何人提供您收到的短信验证码，并尽快完成验证。", text);

                string text2 = string.Format(@"您好！您正在进行海美生活会员注册或重设密码，本次的验证码为:{0}，请勿向任何人提供您收到的短信验证码，并尽快完成验证", code);
                string result;

                bool flag = sMSSender.Send(cellphone, text2, out result);
                if (flag)
                {
                    ErrorLog.Write(string.Format("SendRegisterTel: 发送成功，开始记录跟踪缓存IP-{0}, Mobile-{1}", ipAddress, cellphone));

                    //HiCache.Insert(HiContext.Current.User.UserId + "cellphone", text, 10800);
                    HiCache.Insert("REG_CODE_" + cellphone, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 60 * 60);
                    HiCache.Insert("REG_CODE_TIMES_" + cellphone, times + 1, 60 * 60 * 24);
                    if (!string.IsNullOrWhiteSpace(ipAddress))
                    {
                        HiCache.Insert("REG_SMS_IP_" + ipAddress, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 60 * 60);
                        HiCache.Insert("REG_SMS_IP_TIMES_" + ipAddress, ipTimes + 1, 60 * 60 * 24);
                    }

                    ErrorLog.Write(string.Format("SendRegisterTel: 发送成功，写入数据库，IP-{0}, Mobile-{1}", ipAddress, cellphone));

                    //HiCache.Insert(HiContext.Current.User.UserId + "cellphone", text, 10800);
                    EcShop.Entities.Members.Verify verfyinfo = new Entities.Members.Verify();
                    verfyinfo.VerifyCode = code;
                    verfyinfo.CellPhone = cellphone.Trim();
                    EcShop.ControlPanel.Members.TelVerifyHelper.CreateVerify(verfyinfo);

                    this.message = "{\"success\":true,\"msg\":\"" + result + "\"}";
                }
                else
                {
                    #region ==为了方便测试
                    EcShop.Entities.Members.Verify verfyinfo = new Entities.Members.Verify();
                    verfyinfo.VerifyCode = code;
                    verfyinfo.CellPhone = cellphone.Trim();
                    EcShop.ControlPanel.Members.TelVerifyHelper.CreateVerify(verfyinfo);
                    #endregion

                    this.message = "{\"success\":false,\"msg\":\"" + result + "\"}";
                }
            }
            catch (System.Exception)
            {
                this.message = "{\"success\":false,\"msg\":\"短信发送失败，请重试\"}";
            }
        }

        private void CheckIsFirstOrder(System.Web.HttpContext context)
        {
            try
            {
                string orderId = context.Request["orderId"];
                if (DateTime.Now < DateTime.Parse("2015-08-21"))
                {
                    //判断该用户是否有参与首单的活动
                    int result = -2;
                    ShoppingProcessor.CheckIsFirstOrder(HiContext.Current.User.UserId, (int)OrderSource.PC, orderId, out result);
                    if (result == 1)
                    {
                        this.message = "{\"success\":false,\"msg\":\"您已经参加过首单活动\"}";
                    }
                    else
                    {
                        this.message = "{\"success\":true,\"msg\":\"您没有参加首单活动\"}";
                    }

                }
                else
                {
                    this.message = "{\"success\":true,\"msg\":\"首单活动已过期\"}";
                }
            }
            catch (System.Exception)
            {
                this.message = "{\"success\":false,\"msg\":\"未知错误\"}";
            }
        }

        protected void BindAreaList(System.Web.HttpContext context)
        {
            StringBuilder str = new StringBuilder();
            int categoryId;
            string keywords = context.Request["keywords"];
            int.TryParse(context.Request["categoryId"], out categoryId);
            string minSalePrice = context.Request["minSalePrice"];
            string maxSlaePrice = context.Request["maxSlaePrice"];
            string brandId = context.Request["brandId"];
            string favoriteTags = ProductBrowser.GetFavoriteTags();

            DataTable brandCategories = CategoryBrowser.GetBrandCategories(categoryId, 50);
            IList<ImportSourceTypeInfo> importSourceTypeInfo = ImportSourceTypeHelper.GetAllImportSourceTypes();
            DataTable thirdCategories = CategoryBrowser.GetThirdCategoryiesById(categoryId);
            str.Append("{");
            str.Append("\"Success\":true");
            if (brandCategories != null && brandCategories.Rows.Count > 0)
            {
                str.Append(",\"BrandList\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(brandCategories, Formatting.None, convert);
                str.Append(strReviews);
            }
            if (importSourceTypeInfo != null && importSourceTypeInfo.Count > 0)
            {

                if (importSourceTypeInfo.Count > 50)
                {
                    importSourceTypeInfo = importSourceTypeInfo.Take(50).ToList<ImportSourceTypeInfo>();
                }
                str.Append(",\"SourceTypeList\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(importSourceTypeInfo, Formatting.None, convert);
                str.Append(strReviews);
            }
            if (thirdCategories != null && thirdCategories.Rows.Count > 0)
            {
                str.Append(",\"CategorieList\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(thirdCategories, Formatting.None, convert);
                str.Append(strReviews);
            }
            str.Append("}");
            this.message = str.ToString();
        }

        protected void GetBindList(System.Web.HttpContext context)
        {
            int categoryId;
            int.TryParse(context.Request["categoryId"], out categoryId);
            DataTable brinds = CategoryBrowser.GetBindList(categoryId);
            StringBuilder str = new StringBuilder();
            str.Append("{");
            str.Append("\"Success\":true");
            if (brinds != null && brinds.Rows.Count > 0)
            {
                str.Append(",\"BrandList\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(brinds, Formatting.None, convert);
                str.Append(strReviews);
            }
            str.Append("}");
            this.message = str.ToString();
        }

        private void UserPayOrder(OrderInfo order)
        {
            ErrorLog.Write("微信扫码支付，状态检查开始...");

            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                return;
            }

            string json = "";

            if (order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                //如果需要拆单
                if (TradeHelper.CheckIsUnpack(order.OrderId))
                {
                    ErrorLog.Write(string.Format("微信扫码支付，状态检查，拆单，原订单{0},返回信息{1}", order.OrderId, json));
                    if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false, 1))
                    {
                        //OrderHelper.SetOrderPayStatus(order.OrderId, 2);
                        OrderHelper.SetOrderPayStatus(order.OrderId, 2, order.PaymentTypeId, order.PaymentType, order.Gateway, order.GatewayOrderId);
                        if (order.UserId != 0 && order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, order, order.GetTotal());
                            }
                        }
                        order.OnPayment();
                        return;
                    }
                }
                else if (order.OrderType == (int)OrderType.WillMerge)//合并单据
                {
                    ErrorLog.Write(string.Format("微信扫码支付，状态检查，合并单据，原订单{0},返回信息{1}", order.OrderId, json));
                    bool b = ShoppingProcessor.mergeOrder(order);
                    int flag = 0;
                    if (b)
                    {
                        flag = 2;
                    }
                    if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false, flag))
                    {
                        //OrderHelper.SetOrderPayStatus(order.OrderId, 2);
                        OrderHelper.SetOrderPayStatus(order.OrderId, 2, order.PaymentTypeId, order.PaymentType, order.Gateway, order.GatewayOrderId);
                        if (order.UserId != 0 && order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, order, order.GetTotal());
                            }
                        }
                        order.OnPayment();
                        return;
                    }
                }
                else
                {
                    ErrorLog.Write(string.Format("微信扫码支付，状态检查，正常单据，原订单{0},返回信息{1}", order.OrderId, json));
                    if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false))
                    {
                        //OrderHelper.SetOrderPayStatus(order.OrderId, 2);
                        OrderHelper.SetOrderPayStatus(order.OrderId, 2, order.PaymentTypeId, order.PaymentType, order.Gateway, order.GatewayOrderId);
                        if (order.UserId != 0 && order.UserId != 1100)
                        {
                            IUser user = Users.GetUser(order.UserId);
                            if (user != null && user.UserRole == UserRole.Member)
                            {
                                Messenger.OrderPayment(user, order, order.GetTotal());
                            }
                        }
                        order.OnPayment();
                        return;
                    }
                }
            }
            else
            {
                ErrorLog.Write(string.Format("微信扫码支付，状态检查，当前状态不支持付款，订单号：{0}，订单状态：{1}", order.OrderId, order.OrderStatus));
            }
        }
    }
}
