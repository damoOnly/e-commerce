namespace Ecdev.Plugins.Payment.ShengPay
{
    using Ecdev.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class ShengPayNotify : PaymentNotify
    {
        private readonly NameValueCollection _parameters;

        public ShengPayNotify(NameValueCollection parameters)
        {
            this._parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this._parameters["TransNo"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this._parameters["TransAmount"]);
        }

        public override string GetOrderId()
        {
            return this._parameters["OrderNo"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this._parameters["Name"];
            string str2 = this._parameters["Version"];
            string str3 = this._parameters["Charset"];
            string str4 = this._parameters["TraceNo"];
            string str5 = this._parameters["MsgSender"];
            string str6 = this._parameters["SendTime"];
            string str7 = this._parameters["InstCode"];
            string str8 = this._parameters["OrderNo"];
            string str9 = this._parameters["OrderAmount"];
            string str10 = this._parameters["TransNo"];
            string str11 = this._parameters["TransAmount"];
            string str12 = this._parameters["TransStatus"];
            string str13 = this._parameters["TransType"];
            string str14 = this._parameters["TransTime"];
            string str15 = this._parameters["MerchantNo"];
            string str16 = this._parameters["ErrorCode"];
            string str17 = this._parameters["ErrorMsg"];
            string str18 = this._parameters["Ext1"];
            string str19 = this._parameters["Ext2"];
            string str20 = this._parameters["SignType"];
            string str21 = this._parameters["SignMsg"];
            string data = "";
            string errorMsg = "";
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string innerText = document.FirstChild.SelectSingleNode("Key").InnerText;
            string str25 = str + str2 + str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + str11 + str12 + str13 + str14 + str15 + str16 + str17 + str18 + str19 + str20;
            data = str25;
            string str26 = FormsAuthentication.HashPasswordForStoringInConfigFile(str25 + innerText, "MD5");
            string str27 = data;
            data = str27 + "|status:" + str12 + "|sign:" + str26 + "|mac:" + str21;
            if ((str12 != "01") || (str21 != str26))
            {
                errorMsg = "签名错误";
                this.writeXML(data, errorMsg);
                this.OnNotifyVerifyFaild();
            }
            else
            {
                this.OnFinished(false);
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if ((context != null) && success)
            {
                context.Response.Clear();
                context.Response.Write("OK");
                context.Response.End();
            }
        }

        public void writeXML(string Data, string ErrorMsg)
        {
            DataTable table = new DataTable {
                TableName = "Error"
            };
            table.Columns.Add("DateTime");
            table.Columns.Add("Error");
            table.Columns.Add("Data");
            DataRow row = table.NewRow();
            row["DateTime"] = DateTime.Now;
            row["Error"] = ErrorMsg;
            row["Data"] = Data;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/PayNotifyLog.xml"));
        }
    }
}

