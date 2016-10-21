using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using EcShop.UI.Web.Admin;

namespace EcShop.UI.Web.Admin.sales
{
    public class BathSendOrderByExcel : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnBatchSend;
        protected FileUpload excelFile;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnBatchSend.Click += new System.EventHandler(this.btnBatchSend_Click);
        }
        protected void btnBatchSend_Click(object sender, System.EventArgs e)//新增上传excel来发货
        {

            this.btnBatchSend.Enabled = false;
            HttpPostedFile file = this.excelFile.PostedFile;
            if (file.ContentLength == 0)
            {
                this.btnBatchSend.Enabled = true;
                this.ShowMsg("请选择文件!", false);
                return;
            }
            string expandName = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
            int excelType = 0;
            if (expandName == "xls") { excelType = 1; }
            else if (expandName == "xlsx") { excelType = 2; }
            else
            {
                this.btnBatchSend.Enabled = true;
                this.ShowMsg("请上传excel文件!", false);
                return;
            }
            ExcelHelper excelHelper = new ExcelHelper(file.InputStream);
            DataTable dtExcel = excelHelper.ExcelToDataTable("", true, excelType);

            if (dtExcel == null || dtExcel.Columns.Count < 4)
            {
                this.btnBatchSend.Enabled = true;
                this.ShowMsg("excel表格列数错误!", false);
                return;
            }
            if (!(dtExcel.Columns[0].ToString() == "订单编号" && dtExcel.Columns[1].ToString() == "发货时间" && dtExcel.Columns[2].ToString() == "快递公司" && dtExcel.Columns[3].ToString() == "快递单号"))
            {
                this.btnBatchSend.Enabled = true;
                this.ShowMsg("excel表格列名称错误!", false);
                return;
            }
            List<string> orderIds = new List<string>();
            List<string> expressCompanyName = new List<string>();
            List<string> ShipOrderNumber = new List<string>();
            List<DateTime> deliveryTimes = new List<DateTime>();
            Dictionary<string, string> deliverFailOrders = new Dictionary<string, string>();
            List<deliverFailOrderInfo> listdeliverFailOrderInfo = new List<deliverFailOrderInfo>();
            int num = 0;

            #region 整理发货单数据
            if (dtExcel != null)
            {
                int count = dtExcel.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string tmpOrderId = string.Empty;
                    string tmpexpressCompanyName = string.Empty;
                    string tmpkuaidi100Code = string.Empty;
                    DateTime tmpdeliveryTime = DateTime.Now;

                    tmpOrderId = dtExcel.Rows[i][0].ToString();
                    DateTime.TryParse(dtExcel.Rows[i][1].ToString(), out tmpdeliveryTime);
                    tmpexpressCompanyName = dtExcel.Rows[i][2].ToString();
                    tmpkuaidi100Code = dtExcel.Rows[i][3].ToString();
                    string[] singleLineOrderIds = tmpOrderId.Split(',');//单行可能多个订单号
                    for (int j = 0; j < singleLineOrderIds.Length; j++)
                    {
                        tmpOrderId = singleLineOrderIds[j];
                        if (string.IsNullOrEmpty(tmpOrderId))
                            continue;
                        orderIds.Add(tmpOrderId);
                        deliveryTimes.Add(tmpdeliveryTime);
                        expressCompanyName.Add(tmpexpressCompanyName);
                        ShipOrderNumber.Add(tmpkuaidi100Code);
                    }
                }
            }
            #endregion

            #region 发货业务逻辑处理
            for (int i = 0; i < orderIds.Count; i++)
            {
                deliverFailOrderInfo failOrderInfo = null;
                if (string.IsNullOrEmpty(orderIds[i]) && string.IsNullOrEmpty(expressCompanyName[i]) && string.IsNullOrEmpty(ShipOrderNumber[i]))
                {
                    if (!deliverFailOrders.ContainsKey(orderIds[i]))
                    {
                        failOrderInfo = new deliverFailOrderInfo();
                        failOrderInfo.OrderId = orderIds[i];
                        failOrderInfo.FailInfo = "订单号、快递公司或快递单号不能为空";
                        failOrderInfo.ExpressCompanyCode = ShipOrderNumber[i];
                        failOrderInfo.ExpressCompanyName = expressCompanyName[i];
                        listdeliverFailOrderInfo.Add(failOrderInfo);
                        deliverFailOrders.Add(orderIds[i], "订单号、快递公司或快递单号不能为空");

                        continue;
                    }

                }
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderIds[i]);
                if (orderInfo == null)
                {
                    if (!deliverFailOrders.ContainsKey(orderIds[i]))
                    {
                        failOrderInfo = new deliverFailOrderInfo();
                        failOrderInfo.OrderId = orderIds[i];
                        failOrderInfo.FailInfo = "不存在该订单";
                        failOrderInfo.ExpressCompanyCode = ShipOrderNumber[i];
                        failOrderInfo.ExpressCompanyName = expressCompanyName[i];
                        listdeliverFailOrderInfo.Add(failOrderInfo);
                        deliverFailOrders.Add(orderIds[i], "不存在该订单");
                    }
                    continue;
                }
                if ((orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "hishop.plugins.payment.podrequest") || orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                {

                    string exCompanyName = expressCompanyName[i];
                    ShippingModeInfo shippingModeByCompany = SalesHelper.GetShippingModeByCompany(exCompanyName);
                    if (shippingModeByCompany != null)
                    {
                        if (shippingModeByCompany.ModeId > 0)
                        {
                            orderInfo.RealModeName = shippingModeByCompany.Name;
                            orderInfo.RealShippingModeId = shippingModeByCompany.ModeId;
                        }
                        else
                        {
                            orderInfo.RealShippingModeId = 2;//配送方式
                            orderInfo.RealModeName = "圆通";
                        }
                    }
                    else
                    {
                        orderInfo.RealShippingModeId = 2;//配送方式
                        orderInfo.RealModeName = "圆通";
                    }

                    ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(expressCompanyName[i]);//物流公司
                    if (expressCompanyInfo == null)
                    {
                        if (!deliverFailOrders.ContainsKey(orderIds[i]))
                        {
                            failOrderInfo = new deliverFailOrderInfo();
                            failOrderInfo.OrderId = orderIds[i];
                            failOrderInfo.FailInfo = "不存在物流公司";
                            failOrderInfo.ExpressCompanyCode = ShipOrderNumber[i];
                            failOrderInfo.ExpressCompanyName = expressCompanyName[i];
                            listdeliverFailOrderInfo.Add(failOrderInfo);
                            deliverFailOrders.Add(orderIds[i], "不存在物流公司");
                        }
                        continue;
                    }
                    orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
                    orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
                    orderInfo.ShipOrderNumber = ShipOrderNumber[i];
                    orderInfo.ShippingDate = deliveryTimes[i];
                    //todo 发货时间
                    if (OrderHelper.SendGoods(orderInfo))
                    {
                        SendNoteInfo sendNoteInfo = new SendNoteInfo();
                        sendNoteInfo.NoteId = Globals.GetGenerateId() + num;
                        sendNoteInfo.OrderId = orderIds[i];
                        sendNoteInfo.Operator = HiContext.Current.User.Username;
                        sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
                        OrderHelper.SaveSendNote(sendNoteInfo);
                        if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
                        {
                            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
                            if (paymentMode != null && !string.IsNullOrEmpty(paymentMode.Settings) && paymentMode.Settings != "1hSUSkKQ/ENo0JDZah8KKQweixin")
                            {
                                PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
									{
										paymentMode.Gateway
									})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
									{
										paymentMode.Gateway
									})), "");
                                if (paymentRequest != null)
                                    paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                            }
                        }
                        if (!string.IsNullOrEmpty(orderInfo.TaobaoOrderId))
                        {
                            try
                            {
                                string requestUriString = string.Format("http://order2.ecdev.cn/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}&Host={3}", new object[]
									{
										orderInfo.TaobaoOrderId,
										expressCompanyInfo.TaobaoCode,
										orderInfo.ShipOrderNumber,
										HiContext.Current.SiteUrl
									});
                                System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
                                webRequest.GetResponse();
                            }
                            catch
                            {
                            }
                        }
                        int num3 = orderInfo.UserId;
                        if (num3 == 1100)
                        {
                            num3 = 0;
                        }
                        IUser user = Users.GetUser(num3);
                        Messenger.OrderShipping(orderInfo, user);
                        orderInfo.OnDeliver();
                        num++;
                    }
                    else
                    {
                        if (!deliverFailOrders.ContainsKey(orderIds[i]))
                        {
                            failOrderInfo = new deliverFailOrderInfo();
                            failOrderInfo.OrderId = orderIds[i];
                            failOrderInfo.FailInfo = "发货失败";
                            failOrderInfo.ExpressCompanyCode = ShipOrderNumber[i];
                            failOrderInfo.ExpressCompanyName = expressCompanyName[i];
                            listdeliverFailOrderInfo.Add(failOrderInfo);
                            deliverFailOrders.Add(orderIds[i], "发货失败");
                        }
                    }
                }
                else
                {
                    if (!deliverFailOrders.ContainsKey(orderIds[i]))
                    {
                        failOrderInfo = new deliverFailOrderInfo();
                        failOrderInfo.OrderId = orderIds[i];
                        failOrderInfo.FailInfo = "该订单不满足发货条件";
                        failOrderInfo.ExpressCompanyCode = ShipOrderNumber[i];
                        failOrderInfo.ExpressCompanyName = expressCompanyName[i];
                        listdeliverFailOrderInfo.Add(failOrderInfo);
                        deliverFailOrders.Add(orderIds[i], "该订单不满足发货条件");
                    }
                }
            }
            #endregion

            #region 失败订单抛出
            if (deliverFailOrders.Count > 0)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                stringBuilder.AppendLine("<caption style='text-align:center;'>发货未成功订单列表</caption>");
                stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                stringBuilder.AppendLine("<td>订单号</td>");
                stringBuilder.AppendLine("<td>物流公司</td>");
                stringBuilder.AppendLine("<td>物流单号</td>");
                stringBuilder.AppendLine("<td>错误信息</td>");
                stringBuilder.AppendLine("</tr>");

                foreach (deliverFailOrderInfo failOrderInfo in listdeliverFailOrderInfo)
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine(string.Format("<td style=\"vnd.ms-excel.numberformat:@\">{0}</td>", failOrderInfo.OrderId));
                    stringBuilder.AppendLine(string.Format("<td style=\"vnd.ms-excel.numberformat:@\">{0}</td>", failOrderInfo.ExpressCompanyName));
                    stringBuilder.AppendLine(string.Format("<td style=\"vnd.ms-excel.numberformat:@\">{0}</td>", failOrderInfo.ExpressCompanyCode));
                    stringBuilder.AppendLine(string.Format("<td style=\"vnd.ms-excel.numberformat:@\">{0}</td>", failOrderInfo.FailInfo));
                    stringBuilder.AppendLine("</tr>");
                }
                stringBuilder.AppendLine("</table>");
                stringBuilder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=发货未成功订单_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(stringBuilder.ToString());
            }
            #endregion
            this.btnBatchSend.Enabled = true;
            this.ShowMsg("操作完成!", true);
            //base.Response.End();
        }
    }
    public class deliverFailOrderInfo
    {
        public string OrderId
        {
            get;
            set;
        }
        public string FailInfo
        {
            get;
            set;
        }
        public string ExpressCompanyName
        {
            get;
            set;
        }
        public string ExpressCompanyCode
        {
            get;
            set;
        }
    }
}
