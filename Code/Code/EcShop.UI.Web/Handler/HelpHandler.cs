using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core.Configuration;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Comments;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Web.Admin.PO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WMS.Jobs;

namespace EcShop.UI.Web.Handler
{
    public class HelpHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string message = "";
        private string _PdfPath;
        public void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/json";
                string text = context.Request["action"];
                string key;
                switch (key = text)
                {
                    case "GetHelpContent":
                        this.GetHelpContent(context);
                        break;
                    case "GetXYCode":
                        this.GetXYCode(context);
                        break;
                    case "GetHSCode":
                        this.GetHSCode(context);
                        break;
                    case "GetHS_CODE_ELMENTS":
                        this.GetHSCODEELMENTS(context);
                        break;
                    case "GetHS_Declare":
                        this.GetHS_Declare(context);
                        break;
                    case "GetHS_Status":
                        this.GetHS_Status(context);
                        break;
                    case "GetOrderMonitoring":
                        this.GetOrderMonitoring(context);
                        break;
                    case "GetOrderMonitoringItem":
                        this.GetOrderMonitoringItem(context);
                        break;
                    case "ReMoveHS_Declare":
                        this.ReMoveHS_Declare(context);
                        break;
                    case "setProductNum":
                        this.setProductNum(context);
                        break;
                    case "GetPO_Status":
                        this.GetPO_Status(context);
                        break;
                    case "joinProductNum":
                        this.joinProductNum(context);
                        break;
                    case "CheckProductNum":
                        this.CheckProductNum(context);
                        break;
                    case "GetCountry":
                        this.GetCountry(context);
                        break;
                    case "PODeclareRequest":
                        this.PODeclareRequest(context);
                        break;
                    case "PODeclareResponse":
                        this.PODeclareResponse(context);
                        break;
                    case "POCommodityInspectionInfo":
                        this.POCommodityInspectionInfo(context);
                        break;
                    //报关单二级明细
                    case "POCDPurchaseOrderList":
                        this.POCDPurchaseOrderList(context);
                        break;
                    case "POCDPurchaseOrderBodyLj":
                        this.POCDPurchaseOrderBodyLj(context);
                        break;
                    case "GetCI_Status":
                        this.GetCI_Status(context);
                        break;
                }
                context.Response.Write(this.message);
            }
            catch (Exception e)
            {
                context.Response.Write("{\"success\":\"NO\",\"MSG\":\"执行错误：" + e.Message.Replace("\"", "") + "\"}");
            }
        }
        /// <summary>
        /// 商检信息
        /// </summary>
        /// <param name="context"></param>
        private void POCommodityInspectionInfo(System.Web.HttpContext context)
        {
            string id = context.Request["ID"];

            DataTable head = PurchaseOrderHelper.POCommodityInspectionInfo(id);
            DataTable body = PurchaseOrderHelper.POCommodityInspectionInfoDes(id);

            if (head != null && head.Rows.Count > 0)
            {
                htb htb = new htb();
                htb.orgCode = "051527056";
                htb.agentName = "深圳市信捷网科技有限公司";
                htb.customsCode = "5349/深圳前海湾保税港区口岸作业区";
                htb.ciqCode = "470000/深圳局本部";
                htb.supervisionCode = "5349/深圳前海湾保税港区口岸作业区";
                htb.businessMode = "2";
                htb.businessType = head.Rows[0]["businessType"].ToString();
                //htb.businessType = "BA/桶";
                htb.emsNo = "I440366003415001";
                htb.storageTradeName = "深圳市信捷网科技有限公司";
                htb.storageEbpName = "深圳市信捷网科技有限公司";
                htb.logisticsName = "招商局保税物流有限公司";
                htb.strIeDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                htb.storageEnd = System.DateTime.Now.ToString("yyyy-MM-dd");
                htb.psType = "1";
                htb.storagePortCode = head.Rows[0]["storagePortCode"].ToString();
                htb.storageCusName = "深圳市信捷网科技有限公司";
                htb.storageTrafMode = head.Rows[0]["storageTrafMode"].ToString();
                htb.shipName = head.Rows[0]["shipName"].ToString();
                htb.voyageNo = head.Rows[0]["voyageNo"].ToString();
                htb.billNo = head.Rows[0]["billNo"].ToString();
                htb.storageDestinationPort = "142/中国";
                //htb.storageWrapType = head.Rows[0]["storageWrapType"].ToString();
                if (head.Rows[0]["storageWrapType"].ToString() == "1/木箱")
                {
                    htb.storageWrapType = "PN/厚木板";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "2/纸箱")
                {
                    htb.storageWrapType = "CT/纸板箱";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "3/桶装")
                {
                    htb.storageWrapType = "BA/桶";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "4/散装")
                {
                    htb.storageWrapType = "BU/散装";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "5/拖盘")
                {
                    htb.storageWrapType = "PU/拖盘包装";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "6/包")
                {
                    htb.storageWrapType = "PK/包";
                }
                else if (head.Rows[0]["storageWrapType"].ToString() == "7/其他")
                {
                    htb.storageWrapType = "无";
                }
                //htb.storageWrapType = "BA/桶";
                htb.packNo = head.Rows[0]["packNo"].ToString();
                htb.weight = head.Rows[0]["weight"].ToString();
                htb.netWt = head.Rows[0]["netWt"].ToString();
                htb.storageTradeMode = "1210/保税电商";
                htb.storageTransMode = head.Rows[0]["storageTransMode"].ToString();
                htb.note = "";
                htb.select6 = "0";
                if (body != null && body.Rows.Count > 0)
                {
                    List<goods1> goodsList = new List<goods1>();
                    for (int i = 0; i < body.Rows.Count; i++)
                    {
                        goods1 goods = new goods1();

                        goods.itemNo = "ISHMSH" + body.Rows[i]["BarCode"].ToString();
        
                        goods.unit1 = "035/千克";
                        goods.price = body.Rows[i]["CostPrice"].ToString();
                        goods.ManufactureDate = Convert.ToDateTime(body.Rows[i]["ManufactureDate"]).ToString("yyyy-MM-dd");
                        goods.EffectiveDate = Convert.ToDateTime(body.Rows[i]["EffectiveDate"].ToString()).ToString("yyyy-MM-dd");
                        goods.qty1 = body.Rows[i]["RoughWeight"].ToString();
                        goods.BatchNumber = body.Rows[i]["BatchNumber"].ToString();
                        goods.DeclareNumber = body.Rows[i]["ExpectQuantity"].ToString();
                        goods.mergerNo = body.Rows[i]["num_no"].ToString();
                        //json.setInfo[i] = setInfo;
                        goodsList.Add(goods);
                    }
                    htb.goods = goodsList;
                }
                this.message = Newtonsoft.Json.JsonConvert.SerializeObject(htb);
            }
        }

        /// <summary>
        /// PO申报发送响应
        /// </summary>
        /// <param name="context"></param>
        private void PODeclareResponse(System.Web.HttpContext context)
        {
            string id = context.Request["ID"];
            string json = context.Request["json"];
            POJsonInfo POJsonInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<POJsonInfo>(json);
            //ErrorLogs("json: " + json);
            //ErrorLogs("formId: " + header.formId);
            POJsonInfo.POId = id;
            POJsonInfo.contractNo = PurchaseOrderHelper.UP_GetNewRecordNo(4, "D",DateTime.Now.ToString("yyMMdd"));

            PurchaseOrderHelper.SavePOcustomsDeclaration(POJsonInfo);
            

            //if (!PurchaseOrderHelper.SavePOcustomsDeclaration(header))
            //{
            //    this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            //} 
        }

        /// <summary>
        /// PO申报发送请求
        /// </summary>
        /// <param name="context"></param>
        private void PODeclareRequest(System.Web.HttpContext context)
        {
            string id = context.Request["ID"];

            DataTable head = PurchaseOrderHelper.GetPODeclareJSON(id);
            DataTable body = PurchaseOrderHelper.GetPODeclareBodyJSON(id);

            if (head != null && head.Rows.Count > 0)
            {
                PODeclareJSON json = new PODeclareJSON();
                json.applyCustoms = "深圳前海湾保税港区";
                json.applyCustomsCode = "5349";
                json.applyBusinessCode = "4403660034";
                json.applyBusinessName = "深圳市信捷网科技有限公司";
                json.applyort = head.Rows[0]["applyort"].ToString();
                json.applyortCode = head.Rows[0]["applyortCode"].ToString();
                json.transportType = head.Rows[0]["transportType"].ToString();
                json.transportTypeCode = head.Rows[0]["transportTypeCode"].ToString();
                json.companyCode = "4403660034";
                json.companyName = "深圳市信捷网科技有限公司";
                json.getGoodsBusinName = "深圳市信捷网科技有限公司";
                json.businessType = head.Rows[0]["businessType"].ToString();
                json.allQlt = head.Rows[0]["ExpectQuantity"].ToString();
                json.wrapType = head.Rows[0]["wrapType"].ToString();
                json.wrapTypeCode = head.Rows[0]["wrapTypeCode"].ToString();
                json.superviseType = "保税电商";
                json.superviseTypeCode = "1210";
                json.tradeType = head.Rows[0]["tradeType"].ToString();
                json.tradeTypeCode = head.Rows[0]["tradeTypeCode"].ToString();
                json.Qty = head.Rows[0]["NetWeight"].ToString();
                json.allQty = head.Rows[0]["RoughWeight"].ToString();
                json.sendCountry = head.Rows[0]["StartPortID"].ToString();
                json.sendCountryName = head.Rows[0]["StartPort"].ToString();
                json.RecordCompanyName = "信捷网";
                json.declarationType = "通关无纸化";
                json.transname = head.Rows[0]["transname"].ToString();
                json.loadport_no = head.Rows[0]["StartPortID"].ToString();
                json.end_country_no = "44036";
                json.cabin_no = head.Rows[0]["cabinno"].ToString();
                
                if (body != null && body.Rows.Count > 0)
                {
                    List<setInfo> setInfoList = new List<setInfo>();
                    for (int i = 0; i < body.Rows.Count; i++)
                    {
                        setInfo setInfo = new setInfo();
                        setInfo.mergeType = "手工";
                        setInfo.SkuId = body.Rows[i]["SkuId"].ToString();
                        setInfo.proLJNO = body.Rows[i]["LJNo"].ToString();
                        setInfo.HS_CODE = body.Rows[i]["HS_CODE"].ToString();
                        setInfo.ProductRegistrationNumber = body.Rows[i]["ProductRegistrationNumber"].ToString();
                        setInfo.HSProductName = body.Rows[i]["HSProductName"].ToString();
                        setInfo.applyNum = body.Rows[i]["ExpectQuantity"].ToString();
                        setInfo.AllPrice = body.Rows[i]["TotalCostPrice"].ToString();
                        setInfo.FriNum = body.Rows[i]["GrossWeight"].ToString();
                        setInfo.Country = body.Rows[i]["StandardCName"].ToString();
                        setInfo.CountryCode = body.Rows[i]["HSCode"].ToString();
                        setInfo.getTax = body.Rows[i]["getTax"].ToString();
                        setInfo.getTaxCode = body.Rows[i]["getTaxCode"].ToString();
                        setInfo.useType = body.Rows[i]["useType"].ToString();
                        setInfo.useTypeCode = body.Rows[i]["useTypeCode"].ToString();
                        setInfo.mergeRule = "10位规则";
                        setInfo.Price = body.Rows[i]["CostPrice"].ToString();

                        //json.setInfo[i] = setInfo;
                        setInfoList.Add(setInfo);
                        
                        //json.setInfo[i].mergeRule = "手工";
                        //json.setInfo[i].SkuId = body.Rows[i]["SkuId"].ToString();
                        //json.setInfo[i].proLJNO = body.Rows[i]["LJNo"].ToString();
                        //json.setInfo[i].HS_CODE = body.Rows[i]["HS_CODE"].ToString();
                        //json.setInfo[i].ProductRegistrationNumber = body.Rows[i]["ProductRegistrationNumber"].ToString();
                        //json.setInfo[i].HSProductName = body.Rows[i]["HSProductName"].ToString();
                        //json.setInfo[i].applyNum = body.Rows[i]["ExpectQuantity"].ToString();
                        //json.setInfo[i].AllPrice = body.Rows[i]["TotalCostPrice"].ToString();
                        //json.setInfo[i].FriNum = body.Rows[i]["GrossWeight"].ToString();
                        //json.setInfo[i].Country = body.Rows[i]["StandardCName"].ToString();
                        //json.setInfo[i].CountryCode = body.Rows[i]["HSCode"].ToString();
                        //json.setInfo[i].getTax = body.Rows[i]["getTax"].ToString();
                        //json.setInfo[i].getTaxCode = body.Rows[i]["getTaxCode"].ToString();
                        //json.setInfo[i].useType = body.Rows[i]["useType"].ToString();
                        //json.setInfo[i].useTypeCode = body.Rows[i]["useTypeCode"].ToString();
                        //json.setInfo[i].mergeRule = "10位规则";
                        //json.setInfo[i].Price = body.Rows[i]["CostPrice"].ToString();
                    }
                    json.setInfo = setInfoList;
                }
                this.message = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            }
        }
        /// <summary>
        /// 更新商检状态
        /// </summary>
        /// <param name="context"></param>
        public void joinProductNum(System.Web.HttpContext context)
        {
            string ProductID = context.Request["ProductID"];
            string Remark = context.Request["Remark"];
            string status = context.Request["Status"];
            ProductHelper.setProductInspection(ProductID, Remark, status);
        }

        /// <summary>
        /// 商品入库校验
        /// </summary>
        /// <param name="context"></param>
        public void CheckProductNum(System.Web.HttpContext context)
        {
            string ProductID = context.Request["ProductID"];
            string Remark = context.Request["Remark"];
            string status = context.Request["Status"];
            ProductHelper.setProductCheck(ProductID, Remark, status);
        }
        /// <summary>
        /// 更新商品备案号
        /// </summary>
        /// <param name="context"></param>
        public void setProductNum(System.Web.HttpContext context)
        {
            string ProductNum = context.Request["ProductNum"];
            string SkuId = context.Request["SkuId"];
            string status = context.Request["Status"];
            ProductHelper.Registration(ProductNum, SkuId, status);
        }


        /// <summary>
        /// 作废某个订单明细
        /// </summary>
        /// <param name="context"></param>
        public void ReMoveHS_Declare(System.Web.HttpContext context)
        {
            string orderid = context.Request["OrderID"].ToLower();
            string SkuId = context.Request["SkuId"];

            string UserId = context.Request["UserId"].ToLower();
            string Username = context.Request["Username"];
            if (!string.IsNullOrEmpty(SkuId) && !string.IsNullOrEmpty(orderid))
            {
                if (CheckPrivilegeReturn(Username, Privilege.ValidHSDocking))
                {
                    HSCodeHelper.ValidHSDocking(orderid, SkuId, int.Parse(UserId));
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"您无权操作，请联系管理员！\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        /// <summary>
        /// 获取订单对应的订单详情数据
        /// </summary>
        /// <param name="context"></param>
        public void GetHS_Declare(System.Web.HttpContext context)
        {
            string orderid = context.Request["orderid"].ToLower();

            if (!string.IsNullOrEmpty(orderid))
            {
                this.message = Newtonsoft.Json.JsonConvert.SerializeObject(HSCodeHelper.GetOrderItems(orderid));
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }

        }
        public void GetHS_Status(System.Web.HttpContext context)
        {
            string status = context.Request["status"].ToLower();
            string orderid = context.Request["orderid"].ToLower();
            string Remark = context.Request["Remark"];

            string UserId = context.Request["UserId"].ToLower();
            string Username = context.Request["Username"];
            if (!string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(orderid))
            {
                //重发四单
                if (status == "99")
                {
                    if (CheckPrivilegeReturn(Username, Privilege.ResendHSDocking))
                    {
                        HSCodeHelper.ResendHSDocking(orderid, int.Parse(UserId));
                    }
                    else
                    {
                        this.message = "{\"success\":\"NO\",\"MSG\":\"您无权操作，请联系管理员！\"}";
                    }
                    return;
                }
                //认领操作时判断是否已认领(未认领或者失败时可认领)
                if (status == "4" && HSCodeHelper.GetOrderClaimStatus(orderid))
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"该单已被认领\"}";
                    return;
                }
                //申报时判断是否自己认领的单
                if ((status == "0" || status == "1") && HSCodeHelper.GetOrderClaimUser(orderid).ToString() != UserId && Username != "admin")
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"您无权操作他人认领订单\"}";
                    return;
                }
                HSCodeHelper.SetOrderStatus(orderid, status, Remark, int.Parse(UserId), Username);
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        private bool CheckPrivilegeReturn(string Username, Privilege privilegeCode)
        {
            System.Collections.Generic.IList<string> userPrivileges = EcShop.Membership.Core.RoleHelper.GetUserPrivileges(Username);
            return userPrivileges != null && userPrivileges.Count != 0 && userPrivileges.Contains(privilegeCode.ToString());
        }


        public void GetHelpContent(System.Web.HttpContext context)
        {
            string text = context.Request["helpid"].ToLower();
            if (!string.IsNullOrEmpty(text))
            {
                int helpid = 0;
                int.TryParse(text, out helpid);
                HelpInfo helpInfo = CommentBrowser.GetHelp(helpid);
                if (helpInfo != null)
                {
                    HelpCategoryInfo helpCategory = CommentBrowser.GetHelpCategory(helpInfo.CategoryId);
                    this.message = helpInfo.Content;
                }
            }
        }

        /// <summary>
        /// 获取行邮编码数据
        /// </summary>
        /// <param name="context"></param>
        public void GetXYCode(System.Web.HttpContext context)
        {
            string code = context.Request["query"].ToString();
            this.message = Newtonsoft.Json.JsonConvert.SerializeObject(TaxRateHelper.GetTaxRateBuyCode(code));
        }

        /// <summary>
        /// 获取海关编码数据
        /// </summary>
        /// <param name="context"></param>
        public void GetHSCode(System.Web.HttpContext context)
        {
            string code = context.Request["query"].ToString();
            this.message = Newtonsoft.Json.JsonConvert.SerializeObject(HSCodeHelper.GetHSCodeBuyCode(code));
        }

        /// <summary>
        /// 获取海关编码数据
        /// </summary>
        /// <param name="context"></param>
        public void GetCountry(System.Web.HttpContext context)
        {
            string code = context.Request["query"].ToString();
            this.message = Newtonsoft.Json.JsonConvert.SerializeObject(BaseCountryHelper.GetBaseCountryByName(code));
        }
        

        /// <summary>
        /// 获取海关编码对应申报要素数据
        /// </summary>
        /// <param name="context"></param>
        public void GetHSCODEELMENTS(System.Web.HttpContext context)
        {
            string CodeId = context.Request["HSCode"].ToString();
            string ProductId = context.Request["ProductId"].ToString();
            if (!string.IsNullOrEmpty(CodeId) && !string.IsNullOrEmpty(ProductId))
            {
                this.message = Newtonsoft.Json.JsonConvert.SerializeObject(HSCodeHelper.GetHSCODEELMENTS(CodeId, ProductId));
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        /// <summary>
        /// 获取订单监控数据
        /// </summary>
        /// <param name="context"></param>
        public void GetOrderMonitoring(System.Web.HttpContext context)
        {
            string StartDate = context.Request["StartDate"].ToString();
            string EndDate = context.Request["EndDate"].ToString();
            string Hour = context.Request["Hour"].ToString();

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && !string.IsNullOrEmpty(Hour))
            {
                DataSet ds = HSCodeHelper.GetOrderMonitoring(StartDate, EndDate, Hour);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.message = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"未查询到数据，请重新输入查询\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        /// <summary>
        /// 获取订单监控详细数据
        /// </summary>
        /// <param name="context"></param>
        public void GetOrderMonitoringItem(System.Web.HttpContext context)
        {
            string date = context.Request["date"].ToString();

            //string StartDate = context.Request["StartDate"].ToString();
            //string EndDate = context.Request["EndDate"].ToString();
            string StartDate = date.Substring(0, 10).Trim();
            string EndDate = date.Substring(12).Trim();
            string Hour = context.Request["Hour"].ToString();
            string type = context.Request["type"].ToString();
            string Status = context.Request["Status"].ToString();

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && !string.IsNullOrEmpty(Hour))
            {
                DataSet ds = HSCodeHelper.GetOrderMonitoringItem(StartDate, EndDate, Hour, type, Status);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.message = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"未查询到数据，请重新输入查询\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        /// <summary>
        /// 获取报关单主体列表
        /// </summary>
        /// <param name="context"></param>
        public void POCDPurchaseOrderList(System.Web.HttpContext context)
        {
            string Id = context.Request["Id"].ToString();

            if (!string.IsNullOrEmpty(Id))
            {
                int headId = int.Parse(Id);
                DataTable dt = PurchaseOrderHelper.GetPOCDPurchaseOrderList(headId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.message = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"未查询到数据\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        /// <summary>
        /// 获取报关单主体列表
        /// </summary>
        /// <param name="context"></param>
        public void POCDPurchaseOrderBodyLj(System.Web.HttpContext context)
        {
            string Id = context.Request["Id"].ToString();

            if (!string.IsNullOrEmpty(Id))
            {
                int bodyId = int.Parse(Id);
                DataTable dt = PurchaseOrderHelper.GetPOCDPurchaseOrderBodyLj(bodyId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.message = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"未查询到数据\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        public void GetPO_Status(System.Web.HttpContext context)
        {
            string status = context.Request["status"].ToLower();
            string id = context.Request["id"].ToLower();
            string Remark = context.Request["Remark"];

            string UserId = context.Request["UserId"].ToLower();
            string Username = context.Request["Username"];
            if (!string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(id))
            {
                //招商权限（确认、取消确认）POZS和关务权限（除确认外其他）POGW
                //确认和取消确认只有招商有权限
                if ((status == "0" || status == "1") && !CheckPrivilegeReturn(Username, Privilege.POZS))
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"您无权操作，请联系管理员！\"}";
                    return;
                }

                if (status != "0" && status != "1" && !CheckPrivilegeReturn(Username, Privilege.POGW))
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"您无权操作，请联系管理员！\"}";
                    return;
                }

                //申报、确认、删除、编辑在页面判断是否是自己的
                if (status == "-1")
                {
                    Remark = string.Format("{0}_{1}退回，时间：{2}", UserId, Username, DateTime.Now);
                }

                if (status == "0")
                {
                    Remark = string.Format("{0}_{1}取消确认，时间：{2}", UserId, Username, DateTime.Now);
                }

                //确认时必须要有明细
                if (status == "1" && !PurchaseOrderHelper.IsExistsPOItem(id))
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"确认失败，请先添加采购订单明细！\"}";
                    return;
                }

                //如果是申报成功先调用WMS接口，成功后再修改
                if (status == "7")
                {
                    //1.获取发送需要的数据
                    DataSet ds = PurchaseOrderHelper.GetPurchaseOrderAndItem("po.id=" + id);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ErrorLog.Write("执行了putASNData->PurchaseOrderHelper.GetPurchaseOrderAndItem返回查询结果:" + ds.Tables[0].Rows.Count);
                        //构造数据
                        string skuData = this.CreatePOData(ds.Tables[0]);
                        skuData = skuData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("+", "＋");

                        //配置获取
                        HiConfiguration config = HiConfiguration.GetConfig();
                        XmlNode configSection = config.GetConfigSection("Ecdev/Jobs");
                        XmlNode configjob = configSection.SelectSingleNode("job[@name='OrderSendJob']");

                        string appkey = configjob.Attributes["appkey"].InnerText;// "test";//验签KEY
                        string appSecret = configjob.Attributes["appSecret"].InnerText; //"1234567890";//秘钥存配置
                        string customerid = configjob.Attributes["client_customerid"].InnerText;// "XJW";//client_customerid
                        string client_db = configjob.Attributes["client_db"].InnerText;// "WH01";
                        string apptoken = configjob.Attributes["apptoken"].InnerText; //"80AC1A3F-F949-492C-A024-7044B28C8025";
                        string url = configjob.Attributes["apiurl"].InnerText;//"http://192.168.1.120:8090/datahubWeb/FLUXWMSAPI/XJW";//apiurl

                        //生成验签值
                        string tempdata = appSecret + skuData + appSecret;
                        string md5tempdata = WMSHelper.MD5(tempdata);
                        string basetempdata = WMSHelper.EncodingString(md5tempdata.ToLower(), System.Text.Encoding.UTF8);
                        string sign = System.Web.HttpUtility.UrlEncode(basetempdata.ToUpper(), System.Text.Encoding.UTF8);
                        //时间戳
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//YYYY-MM-DD HH:MM:SS
                        string postData = "method=putASNData&client_customerid=" + customerid + "&client_db=" + client_db + "&messageid=ASN&apptoken=" + apptoken + "&appkey=" + appkey + "&sign=" + sign + "&timestamp=" + timestamp;

                        //记录发送数据日志
                        ErrorLog.Write("执行了putASNData->发送数据：postData=" + postData + "&data=" + skuData);
                        WMSHelper.SaveLog("putASNData", "postData=" + postData + "&data=" + skuData, "调用方法", "info", "out");

                        skuData = System.Web.HttpUtility.UrlEncode(skuData);
                        //发送并接收回传数据
                        string sendResult = WMSHelper.PostData(url, postData + "&data=" + skuData);
                        string tempResult = System.Web.HttpUtility.UrlDecode(sendResult);
                        //保存接收消息
                        ErrorLog.Write("执行了putASNData->接收WMS消息:" + tempResult + "\n");
                        WMSHelper.SaveLog("putASNData", "", "返回结果：" + tempResult, "info", "in");

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(tempResult);
                        XmlNode node = xmlDocument.SelectSingleNode("Response/return/returnCode");
                        XmlNode nodeFlag = xmlDocument.SelectSingleNode("Response/return/returnFlag");
                        XmlNode nodeDesc = xmlDocument.SelectSingleNode("Response/return/returnDesc");
                        if (node.InnerText != "0000")
                        {
                            //推送失败
                            this.message = "{\"success\":\"NO\",\"MSG\":\"采购订单传送WMS失败：" + nodeDesc.InnerText + "\"}";
                            return;
                        }
                    }
                    else
                    {
                        this.message = "{\"success\":\"NO\",\"MSG\":\"获取采购订单数据失败\"}";
                        return;
                    }
                }
                //执行更改
                if (PurchaseOrderHelper.SetPOStatus(id, status, Remark, int.Parse(UserId), Username))
                {
                    this.message = "{\"success\":\"YES\",\"MSG\":\"操作成功\"}";
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败，请检查您是否有权限，如无问题请刷新后重试。\"}";
                }
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            }
        }

        #region 构造PO发送数据
        /// <summary>
        /// 构造PO发送数据 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreatePOData(DataTable dt)
        {
            StringBuilder strData = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                strData.Append("<xmldata>");
                DataRow dataRow = dt.Rows[0];
                try
                {
                    strData.Append("<header>");
                    strData.AppendFormat("<OrderNo><![CDATA[{0}]]></OrderNo>", dataRow["PONumber"].ToString().Trim());//采购订单号
                    strData.AppendFormat("<OrderType><![CDATA[{0}]]></OrderType>", "PR");//采购订单
                    strData.AppendFormat("<CustomerID><![CDATA[{0}]]></CustomerID>", "SINCNET");//客户ID,固定值SINCNET
                    strData.AppendFormat("<ASNCreationTime><![CDATA[{0}]]></ASNCreationTime>", dataRow["CreateTime"].ToString().Trim().Length > 0 ? Convert.ToDateTime(dataRow["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : "");//ASN创建时间
                    strData.AppendFormat("<ExpectedArriveTime1><![CDATA[{0}]]></ExpectedArriveTime1>", dataRow["ExpectedTime"].ToString().Trim().Length > 0 ? Convert.ToDateTime(dataRow["ExpectedTime"]).ToString("yyyy-MM-dd HH:mm:ss") : "");//预期到货时间
                    strData.AppendFormat("<ASNReference2><![CDATA[{0}]]></ASNReference2>", "");//ASN参考信息2
                    strData.AppendFormat("<ASNReference3><![CDATA[{0}]]></ASNReference3>", "");//ASN参考信息3
                    strData.AppendFormat("<ASNReference4><![CDATA[{0}]]></ASNReference4>", "");//ASN参考信息4
                    strData.AppendFormat("<ASNReference5><![CDATA[{0}]]></ASNReference5>", "");//ASN参考信息5
                    strData.AppendFormat("<CountryOfOrigin><![CDATA[{0}]]></CountryOfOrigin>", "");//原产国（明细中才有）先不传
                    strData.AppendFormat("<CountryOfDestination><![CDATA[{0}]]></CountryOfDestination>", "");//目的国（char(2)类型要传什么）先不传
                    strData.AppendFormat("<PlaceOfLoading><![CDATA[{0}]]></PlaceOfLoading>", "");//装货地
                    strData.AppendFormat("<PlaceOfDischarge><![CDATA[{0}]]></PlaceOfDischarge>", "");//卸货地
                    strData.AppendFormat("<PlaceofDelivery><![CDATA[{0}]]></PlaceofDelivery>", "");//交货地
                    strData.AppendFormat("<UserDefine1><![CDATA[{0}]]></UserDefine1>", "");//自定义1
                    strData.AppendFormat("<UserDefine2><![CDATA[{0}]]></UserDefine2>", "");
                    strData.AppendFormat("<UserDefine3><![CDATA[{0}]]></UserDefine3>", "");
                    strData.AppendFormat("<UserDefine4><![CDATA[{0}]]></UserDefine4>", "");
                    strData.AppendFormat("<UserDefine5><![CDATA[{0}]]></UserDefine5>", "");
                    strData.AppendFormat("<Notes><![CDATA[{0}]]></Notes>", dataRow["Remark"].ToString().Trim());//备注
                    strData.AppendFormat("<SupplierID><![CDATA[{0}]]></SupplierID>", dataRow["SupplierId"].ToString().Trim());//供应商ID
                    strData.AppendFormat("<Supplier_Name><![CDATA[{0}]]></Supplier_Name>", dataRow["SupplierName"].ToString().Trim());//供应商名称
                    strData.AppendFormat("<CarrierID><![CDATA[{0}]]></CarrierID>", "");//承运人ID
                    strData.AppendFormat("<CarrierName><![CDATA[{0}]]></CarrierName>", "");//承运人名称
                    strData.AppendFormat("<H_EDI_02><![CDATA[{0}]]></H_EDI_02>", "");//收货人电话
                    strData.AppendFormat("<H_EDI_03><![CDATA[{0}]]></H_EDI_03>", "");//EDI相关信息
                    strData.AppendFormat("<H_EDI_04><![CDATA[{0}]]></H_EDI_04>", "");
                    strData.AppendFormat("<H_EDI_05><![CDATA[{0}]]></H_EDI_05>", "");
                    strData.AppendFormat("<H_EDI_06><![CDATA[{0}]]></H_EDI_06>", "");
                    strData.AppendFormat("<H_EDI_07><![CDATA[{0}]]></H_EDI_07>", "");
                    strData.AppendFormat("<H_EDI_08><![CDATA[{0}]]></H_EDI_08>", "");
                    strData.AppendFormat("<H_EDI_09><![CDATA[{0}]]></H_EDI_09>", "");
                    strData.AppendFormat("<H_EDI_10><![CDATA[{0}]]></H_EDI_10>", "");
                    strData.AppendFormat("<UserDefine6><![CDATA[{0}]]></UserDefine6>", "");//用户自定义6
                    strData.AppendFormat("<UserDefine7><![CDATA[{0}]]></UserDefine7>", "");
                    strData.AppendFormat("<UserDefine8><![CDATA[{0}]]></UserDefine8>", "");
                    strData.AppendFormat("<WarehouseID><![CDATA[{0}]]></WarehouseID>", "WH01");//所属仓库
                    strData.AppendFormat("<Priority><![CDATA[{0}]]></Priority>", "");//优先级
                    strData.AppendFormat("<FollowUp><![CDATA[{0}]]></FollowUp>", "");//业务担当

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strData.Append("<detailsItem>");
                        strData.AppendFormat("<LineNo><![CDATA[{0}]]></LineNo>", i + 1);//货主 固定：SINCNET
                        strData.AppendFormat("<CustomerID><![CDATA[{0}]]></CustomerID>", "SINCNET");//货主 固定：SINCNET
                        strData.AppendFormat("<SKU><![CDATA[{0}]]></SKU>", dt.Rows[i]["SkuId"]);//sku
                        strData.AppendFormat("<ExpectedQty><![CDATA[{0}]]></ExpectedQty>", dt.Rows[i]["ExpectQuantity"]);//预期数量(最小包装数量)
                        strData.AppendFormat("<LotAtt01><![CDATA[{0}]]></LotAtt01>", dt.Rows[i]["ManufactureDate"]);//生产日期
                        strData.AppendFormat("<LotAtt02><![CDATA[{0}]]></LotAtt02>", dt.Rows[i]["EffectiveDate"]);//失效日期
                        strData.AppendFormat("<LotAtt03><![CDATA[{0}]]></LotAtt03>", "");//批次属性
                        strData.AppendFormat("<LotAtt04><![CDATA[{0}]]></LotAtt04>", dt.Rows[i]["SupplierId"]);
                        strData.AppendFormat("<LotAtt05><![CDATA[{0}]]></LotAtt05>", "");
                        strData.AppendFormat("<LotAtt06><![CDATA[{0}]]></LotAtt06>", "");
                        strData.AppendFormat("<LotAtt07><![CDATA[{0}]]></LotAtt07>", "");
                        strData.AppendFormat("<LotAtt08><![CDATA[{0}]]></LotAtt08>", "");
                        strData.AppendFormat("<LotAtt09><![CDATA[{0}]]></LotAtt09>", "");
                        strData.AppendFormat("<LotAtt10><![CDATA[{0}]]></LotAtt10>", "");
                        strData.AppendFormat("<LotAtt11><![CDATA[{0}]]></LotAtt11>", "");
                        strData.AppendFormat("<LotAtt12><![CDATA[{0}]]></LotAtt12>", "");
                        strData.AppendFormat("<TotalPrice><![CDATA[{0}]]></TotalPrice>", dt.Rows[i]["TotalCostPrice"]);//总价
                        strData.AppendFormat("<UserDefine1><![CDATA[{0}]]></UserDefine1>", "");//用户自定义
                        strData.AppendFormat("<UserDefine2><![CDATA[{0}]]></UserDefine2>", "");
                        strData.AppendFormat("<UserDefine3><![CDATA[{0}]]></UserDefine3>", "");
                        strData.AppendFormat("<UserDefine4><![CDATA[{0}]]></UserDefine4>", "");
                        strData.AppendFormat("<UserDefine5><![CDATA[{0}]]></UserDefine5>", "");
                        strData.AppendFormat("<UserDefine6><![CDATA[{0}]]></UserDefine6>", "");
                        strData.AppendFormat("<Notes><![CDATA[{0}]]></Notes>", "");//备注
                        strData.AppendFormat("<D_EDI_03><![CDATA[{0}]]></D_EDI_03>", "");//EDI相关信息
                        strData.AppendFormat("<D_EDI_04><![CDATA[{0}]]></D_EDI_04>", "");
                        strData.AppendFormat("<D_EDI_05><![CDATA[{0}]]></D_EDI_05>", "");
                        strData.AppendFormat("<D_EDI_06><![CDATA[{0}]]></D_EDI_06>", "");
                        strData.AppendFormat("<D_EDI_07><![CDATA[{0}]]></D_EDI_07>", "");
                        strData.AppendFormat("<D_EDI_08><![CDATA[{0}]]></D_EDI_08>", "");
                        strData.AppendFormat("<D_EDI_09><![CDATA[{0}]]></D_EDI_09>", "");
                        strData.AppendFormat("<D_EDI_10><![CDATA[{0}]]></D_EDI_10>", "");
                        strData.AppendFormat("<D_EDI_11><![CDATA[{0}]]></D_EDI_11>", "");
                        strData.AppendFormat("<D_EDI_12><![CDATA[{0}]]></D_EDI_12>", "");
                        strData.AppendFormat("<D_EDI_13><![CDATA[{0}]]></D_EDI_13>", "");
                        strData.AppendFormat("<D_EDI_14><![CDATA[{0}]]></D_EDI_14>", "");
                        strData.AppendFormat("<D_EDI_15><![CDATA[{0}]]></D_EDI_15>", "");
                        strData.AppendFormat("<D_EDI_16><![CDATA[{0}]]></D_EDI_16>", "");
                        strData.Append("</detailsItem>");
                    }
                    strData.Append("</header>");
                }
                catch
                {
                }
                strData.Append("</xmldata>");
            }
            return strData.ToString().Trim();
        } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void GetCI_Status(System.Web.HttpContext context)
        {
            string status = context.Request["status"].ToLower();
            string id = context.Request["id"].ToLower();
            string Remark = context.Request["Remark"];

            string UserId = context.Request["UserId"].ToLower();
            string Username = context.Request["Username"];

            //执行更改
            if (PurchaseOrderHelper.SetCIStatus(id, status, Remark, int.Parse(UserId), Username))
            {
                this.message = "{\"success\":\"YES\",\"MSG\":\"操作成功\"}";
            }
            else
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败，请检查您是否有权限，如无问题请刷新后重试。\"}";
            }
        }

        private void ExportPdf(System.Web.HttpContext context,string id)
        {
            this._PdfPath = "/storage/data/poToPdf/" + DateTime.Now.ToString("yyyyMM") + "/";
            try
            {
                if (id == null || id.Length < 0)
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"无数据。\"}";
                    return;
                }

                DataSet ds = PurchaseOrderHelper.exportPurchaseOrder(int.Parse(id));
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    //判断目录是否存在，不存在则创建
                    string path = context.Request.MapPath(_PdfPath);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    string strHeaderID = "";

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataRow[] drs = ds.Tables[0].Select("HeaderID=" + dr["HeaderID"]);
                        if (drs.Length <= 0)
                        {
                            this.message = "{\"success\":\"NO\",\"MSG\":\"并非所有入库单都有正确返回数据。\"}";
                            //this.ShowMsg("并非所有入库单都有正确返回数据", false);
                            return;
                        }
                        strHeaderID += dr["HeaderID"].ToString() + ",";
                        DataSet dsData = new DataSet();
                        DataTable dt = ds.Tables[0].Clone();
                        dt.TableName = "dt";
                        foreach (DataRow item in drs)
                            dt.Rows.Add(item.ItemArray);
                        dsData.Tables.Add(dt);

                        RptClass rc = new RptClass();
                        if (rc.ExportRpt(context.Request.MapPath("/Admin/rptTemplates/POInvoice.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POInvoice.pdf") != "导出成功")
                        {
                            this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败。\"}";
                            //this.ShowMsg("操作失败", false);
                            return;
                        }

                        if (rc.ExportRpt(context.Request.MapPath("/Admin/rptTemplates/POPacking.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POPacking.pdf") != "导出成功")
                        {
                            this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败。\"}";
                            //this.ShowMsg("操作失败", false);
                            return;
                        }

                        if (rc.ExportRpt(context.Request.MapPath("/Admin/rptTemplates/POContract.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POContract.pdf") != "导出成功")
                        {
                            this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败。\"}";
                            //this.ShowMsg("操作失败", false);
                            return;
                        }
                    }
                    //插入数据库文件地址
                    if (PurchaseOrderHelper.SaveFilePath(_PdfPath, strHeaderID.TrimEnd(',')))
                    {
                        this.message = "{\"success\":\"NO\",\"MSG\":\"操作成功。\"}";
                        //this.ShowMsg("操作成功", true);
                    }
                    else
                    {
                        this.message = "{\"success\":\"NO\",\"MSG\":\"操作失败。\"}";
                        //this.ShowMsg("操作失败", false);
                    }
                }
                else
                {
                    this.message = "{\"success\":\"NO\",\"MSG\":\"未查询到数据，请检查报关单数据是否成功返回。\"}";
                    //this.ShowMsg("未查询到数据，请检查报关单数据是否成功返回。", false);
                }
            }
            catch (Exception ex)
            {
                this.message = "{\"success\":\"NO\",\"MSG\":\"失败： "+ ex.Message+"。\"}";
                //this.ShowMsg("失败：" + ex.Message, false);
            }
        }
    }
}
