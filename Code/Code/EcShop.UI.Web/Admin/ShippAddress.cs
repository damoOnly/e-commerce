using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
    public class ShippAddress : AdminPage
    {
        private string orderId;
        private string action = "";
        protected System.Web.UI.WebControls.TextBox txtShipTo;
        protected RegionSelector dropRegions;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        protected System.Web.UI.WebControls.TextBox txtZipcode;
        protected System.Web.UI.WebControls.TextBox txtTelPhone;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.Button btnMondifyAddress;
        protected System.Web.UI.WebControls.TextBox txtIdentityCard;

        private string efindUrl;
        private string runType;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

            if (!string.IsNullOrEmpty(masterSettings.efindUrl))
            {
                efindUrl = masterSettings.efindUrl;
            }

            if (!string.IsNullOrEmpty(masterSettings.efindRunType))
            {
                runType = masterSettings.efindRunType;
            }

            if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"]))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.action = this.Page.Request.QueryString["action"];
            if (this.action == "update")
            {
                if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
                {
                    base.GotoResourceNotFound();
                    return;
                }
                this.orderId = this.Page.Request.QueryString["OrderId"];
                if (!base.IsPostBack)
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                    this.BindUpdateSippingAddress(orderInfo);
                }
            }
            this.btnMondifyAddress.Click += new System.EventHandler(this.btnMondifyAddress_Click);
        }
        private void BindUpdateSippingAddress(OrderInfo order)
        {
            this.txtShipTo.Text = order.ShipTo;
            this.dropRegions.SetSelectedRegionId(new int?(order.RegionId));
            this.txtAddress.Text = order.Address;
            this.txtZipcode.Text = order.ZipCode;
            this.txtTelPhone.Text = order.TelPhone;
            this.txtCellPhone.Text = order.CellPhone;
            if (order.IdentityCard.Length > 14)
            {
                string identity = order.IdentityCard.Replace(order.IdentityCard.Substring(6, 8), "********");
                this.txtIdentityCard.Text = identity;
            }
        }
        private void btnMondifyAddress_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            string tempToName = orderInfo.ShipTo;
            string tempToId = orderInfo.IdentityCard;

            orderInfo.ShipTo = this.txtShipTo.Text.Trim();
            orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
            orderInfo.Address = this.txtAddress.Text.Trim();
            orderInfo.TelPhone = this.txtTelPhone.Text.Trim();
            orderInfo.CellPhone = this.txtCellPhone.Text.Trim();
            orderInfo.ZipCode = this.txtZipcode.Text.Trim();
            orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
            if (!string.IsNullOrEmpty(this.txtIdentityCard.Text))
            {
                if (this.txtIdentityCard.Text.IndexOf("********") < 0)
                {
                    orderInfo.IdentityCard = this.txtIdentityCard.Text.Trim();
                }
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
            {
                this.ShowMsg("电话号码和手机号码必填其一", false);
                return;
            }
            if (this.action == "update")
            {
                string errorMsg = "";
                //if ((tempToName != this.txtShipTo.Text.Trim() || tempToId != this.txtIdentityCard.Text.Trim()) && this.txtIdentityCard.Text.IndexOf("********") < 0)
                if (tempToName != this.txtShipTo.Text.Trim() || tempToId.ToLower() != this.txtIdentityCard.Text.Trim().ToLower())
                {
                    string orderid = this.orderId;
                    string ShipToID = this.txtIdentityCard.Text.Trim();
                    string ShipToName = this.txtShipTo.Text.Trim();
                    string ShipToaddress = this.txtAddress.Text.Trim();
                    string ShipToPhone = this.txtCellPhone.Text.Trim() != "" ? this.txtCellPhone.Text.Trim() : this.txtTelPhone.Text.Trim();
                    //orderNo  订单号
                    //userName 姓名
                    //idCareNum 身份证号码
                    //phoneNum 收货人手机
                    //address 收货地址
                    string postData = "orderNo=" + orderid + "&userName=" + ShipToName + "&idCareNum=" + ShipToID + "&phoneNum=" + ShipToPhone + "&address=" + ShipToaddress;
                    ErrorLog.Write("后台订单身份验证：efindUrl send data : " + postData);

                    string str = SendData(efindUrl, postData);

                    try
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(str);

                        XmlNode node = xmlDocument.SelectSingleNode("result/code");

                        if (node.InnerText == "200")
                        {
                            XmlNode nodeName = xmlDocument.SelectSingleNode("result/retvalue/emResult/name");
                            XmlNode nodeOrderId = xmlDocument.SelectSingleNode("result/retvalue/emResult/orderId");
                            XmlNode nodeStatus = xmlDocument.SelectSingleNode("result/retvalue/emResult/status");
                            XmlNode nodeErrorcode = xmlDocument.SelectSingleNode("result/retvalue/emResult/errorcode");
                            XmlNode nodeErrormessage = xmlDocument.SelectSingleNode("result/retvalue/emResult/errormessage");
                            XmlNode nodeIDCardNum = xmlDocument.SelectSingleNode("result/retvalue/emResult/idCardNum");

                            if (nodeStatus != null)
                            {
                                if (nodeStatus.InnerText.ToUpper() == "PASS")
                                {
                                    errorMsg = "实名认证成功！";
                                }
                                if (nodeStatus.InnerText.ToUpper() == "NOPASS")
                                {
                                    errorMsg = "实名认证失败！";
                                }
                            }
                            else
                            {
                                errorMsg = "实名认证失败！";
                            }
                        }
                        else
                        {
                            ErrorLog.Write("efindUrl interface fail");
                        }
                        HSCodeHelper.SetPayerIDStatus(orderid, ShipToName, ShipToID, ShipToaddress, ShipToPhone, str, runType);
                    }
                    catch (Exception)
                    {
                        errorMsg = "实名认证失败！";
                    }
                }
                else
                {
                    if (orderInfo.payerIdStatus == 2)
                    {
                        errorMsg = "实名认证成功！";
                    }
                    else
                    {
                        errorMsg = "实名认证失败！";
                    }
                }

                orderInfo.OrderId = this.orderId;
                if (errorMsg.Contains("实名认证成功"))
                {
                    if (OrderHelper.MondifyAddress(orderInfo))
                    {
                        OrderHelper.GetOrderInfo(this.orderId);
                        this.ShowMsg("修改成功，" + errorMsg, true);
                        return;
                    }
                }

                this.ShowMsg("修改失败，" + errorMsg, false);
            }
        }


        public string SendData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();

                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                    str = myStreamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }
    }
}
