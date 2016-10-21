using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Ecdev.Plugins.SMS
{
    [Plugin("信利康通用短信接口")]
    public class XinLkSMS : SMSSender
    {
        private const string Gateway = "http://183.62.226.59:89/PublicService /rest/SmsBeanRestController/hmsh/sendCaptcha";

        public override bool Send(string cellPhone, string message, out string returnMsg)
        {
            return this.Send(cellPhone, message, out returnMsg, "0");
        }

        public override bool Send(string cellPhone, string message,int type, out string returnMsg)
        {
            return this.Send(cellPhone, message,type, out returnMsg, "0");
        }

        public override bool Send(string[] phoneNumbers, string message, out string returnMsg)
        {
            return this.Send(phoneNumbers, message, out returnMsg, "1");
        }

        public override bool Send(string[] phoneNumbers, string message, out string returnMsg, string speed = "1")
        {
            if ((((phoneNumbers == null) || (phoneNumbers.Length == 0)) || string.IsNullOrEmpty(message)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            string phones = string.Join(",", phoneNumbers);
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("mobile", phones);
            dicArrayPre.Add("content ", Uri.EscapeDataString(message));
            dicArrayPre.Add("verificationMark", Sign);
            string temptype = string.Empty;
            if (message.Length == 6 && Regex.IsMatch(message, @"^\d{6}$"))
            {
                dicArrayPre.Add("type", "1");
                temptype = "1";
            }
            else
            {
                dicArrayPre.Add("type", "0");
                temptype = "0";
            }
            Dictionary<string, string> dicArray = SMSAPiHelper.Parameterfilter(dicArrayPre);

            string postData = "";// SMSAPiHelper.CreateLinkstring(dicArray);
            string sendResult = "";

            try
            {
                string url = ApiUrl;
                postData = "mobile=" + phones + "&content=" + Uri.EscapeDataString(message) + "&verificationMark=" + Sign + "&type=" + temptype;
                sendResult = SMSAPiHelper.PostData(url, postData);
                sendResult = sendResult.Replace("\"", "");
                //this.WriteError(Sign, phones + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "sendResult|" + sendResult);
                ErrorLogs("发送短信：Sign:" + Sign + "|" + phones + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed );
                //返回格式：字符串 0：发送成功;其他：发送失败
                //XinLKSmsModel result = new XinLKSmsModel();
                //result = (XinLKSmsModel)Newtonsoft.Json.JsonConvert.DeserializeObject<XinLKSmsModel>(sendResult);
                //if (result != null && result.result == 0)
                //{
                //    returnMsg = "发送成功!";
                //    return true;
                //}
                //returnMsg = result.tips;

                if (!string.IsNullOrEmpty(sendResult) && sendResult == "0")
                {
                    returnMsg = "发送成功!";
                    return true;
                }
                returnMsg = sendResult;

                return false;
            }
            catch (Exception ex)
            {
                ErrorLogs("发送短信：Sign:" + Sign + "|" + phones + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message);
                //this.WriteError(Sign, phones + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message + "sendResult|" + sendResult);
                returnMsg = "未知错误:接口返回" + sendResult;
                return false;
            }
        }


        /// <summary>
        /// 文本日志
        /// </summary>
        /// <param name="list"></param>
        public static void ErrorLogs(string remark)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/SMSerror";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string FileNames = DateTime.Now.ToString("yyyyMMdd");
                StreamWriter sw = new StreamWriter(path + @"/" + FileNames + ".log", true);

                sw.Write(remark);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public override bool Send(string cellPhone, string message, out string returnMsg, string speed = "0")
        {
            if (((string.IsNullOrEmpty(cellPhone) || string.IsNullOrEmpty(message)) || (cellPhone.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("mobile", cellPhone);
            dicArrayPre.Add("content ", Uri.EscapeDataString(message));
            dicArrayPre.Add("verificationMark", Sign);
            string temptype = string.Empty;
            if (message.Length == 6 && Regex.IsMatch(message, @"^\d{6}$"))
            {
                dicArrayPre.Add("type", "1");
                temptype = "1";
            }
            else
            {
                dicArrayPre.Add("type", "0");
                temptype = "0";
            }
            //ErrorLogs(string.Format("发送短信9：手机号码：{0}，短信内容：{1}，dicArrayPre内容：", cellPhone, message, dicArrayPre.ToString()));
            //Dictionary<string, string> dicArray = SMSAPiHelper.Parameterfilter(dicArrayPre);
            //ErrorLogs(string.Format("发送短信10：手机号码：{0}，短信内容：{1}", cellPhone, message));
            //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed);

            ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed );

            string postData = ""; //SMSAPiHelper.CreateLinkstring(dicArray);
            string sendResult = "";
            try
            {
                //ErrorLogs(string.Format("发送短信11：手机号码：{0}，短信内容：{1}", cellPhone, message));
                string url = ApiUrl;
                postData = "mobile=" + cellPhone + "&content=" + Uri.EscapeDataString(message) + "&verificationMark=" + Sign + "&type=" + temptype;
                sendResult = SMSAPiHelper.PostData(url, postData);
                //ErrorLogs(string.Format("发送短信12：手机号码：{0}，短信内容：{1}", cellPhone, message));
                sendResult = sendResult.Replace("\"", "");


                ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + " sendResult|" + sendResult);

                //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "sendResult|" + sendResult);
                //ErrorLogs(string.Format("发送短信13：手机号码：{0}，短信内容：{1}", cellPhone, message));
                //返回格式：字符串 0：发送成功;其他：发送失败
                //XinLKSmsModel result = new XinLKSmsModel();
                //result = (XinLKSmsModel)Newtonsoft.Json.JsonConvert.DeserializeObject<XinLKSmsModel>(sendResult);
                //if (result != null && result.result == 0)
                //{
                //    returnMsg = "发送成功!";
                //    return true;
                //}
                //returnMsg = result.tips;

                if (!string.IsNullOrEmpty(sendResult) && sendResult == "0")
                {
                    returnMsg = "发送成功!";
                    return true;
                }
                returnMsg = sendResult;

                return false;
            }
            catch (Exception ex)
            {
                ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message);
                //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message);
                returnMsg = "未知错误:接口返回" + sendResult;
                return false;
            }
        }

        public override bool Send(string cellPhone, string message, int type,out string returnMsg, string speed = "0")
        {
            if (((string.IsNullOrEmpty(cellPhone) || string.IsNullOrEmpty(message)) || (cellPhone.Trim().Length == 0)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("mobile", cellPhone);
            dicArrayPre.Add("content ", Uri.EscapeDataString(message));
            dicArrayPre.Add("verificationMark", Sign);
            string temptype = string.Empty;
            if (message.Length == 6 && Regex.IsMatch(message, @"^\d{6}$"))
            {
                dicArrayPre.Add("type", "1");
                temptype = "1";
            }
            else
            {
                dicArrayPre.Add("type", "0");
                temptype = "0";
            }
            //ErrorLogs(string.Format("发送短信9：手机号码：{0}，短信内容：{1}，dicArrayPre内容：", cellPhone, message, dicArrayPre.ToString()));
            //Dictionary<string, string> dicArray = SMSAPiHelper.Parameterfilter(dicArrayPre);
            //ErrorLogs(string.Format("发送短信10：手机号码：{0}，短信内容：{1}", cellPhone, message));
            //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed);

            if(type==2)
            {
                this.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["SMSSecond"].ToString();
            }
            ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed);

            string postData = ""; //SMSAPiHelper.CreateLinkstring(dicArray);
            string sendResult = "";
            try
            {
                //ErrorLogs(string.Format("发送短信11：手机号码：{0}，短信内容：{1}", cellPhone, message));

                if (type== 2)
                {
                    string url = ApiUrl; 
                    postData = "mobile=" + cellPhone + "&content=" + Uri.EscapeDataString(message) + "&verificationMark=" + Sign + "&type=" + temptype + "&change=2";
                    sendResult = SMSAPiHelper.PostData(url, postData);
                }

                else
                {
                    string url = ApiUrl;
                    postData = "mobile=" + cellPhone + "&content=" + Uri.EscapeDataString(message) + "&verificationMark=" + Sign + "&type=" + temptype;
                    sendResult = SMSAPiHelper.PostData(url, postData);
                }
                //ErrorLogs(string.Format("发送短信12：手机号码：{0}，短信内容：{1}", cellPhone, message));
                sendResult = sendResult.Replace("\"", "");


                ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + " sendResult|" + sendResult);

                //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "sendResult|" + sendResult);
                //ErrorLogs(string.Format("发送短信13：手机号码：{0}，短信内容：{1}", cellPhone, message));
                //返回格式：字符串 0：发送成功;其他：发送失败
                //XinLKSmsModel result = new XinLKSmsModel();
                //result = (XinLKSmsModel)Newtonsoft.Json.JsonConvert.DeserializeObject<XinLKSmsModel>(sendResult);
                //if (result != null && result.result == 0)
                //{
                //    returnMsg = "发送成功!";
                //    return true;
                //}
                //returnMsg = result.tips;

                if (!string.IsNullOrEmpty(sendResult) && sendResult == "0")
                {
                    returnMsg = "发送成功!";
                    return true;
                }
                returnMsg = sendResult;

                return false;
            }
            catch (Exception ex)
            {
                ErrorLogs("发送短信：Sign:" + Sign + "|" + cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message);
                //this.WriteError(Sign, cellPhone + "|" + message + "|" + this.ApiUrl + "|" + DateTime.Now.ToString() + "|" + speed + "|ex" + ex.Message);
                returnMsg = "未知错误:接口返回" + sendResult;
                return false;
            }
        }

        public void WriteError(string syssign, string param)
        {
            DataTable table = new DataTable
            {
                TableName = "SMSLog"
            };
            table.Columns.Add(new DataColumn("time"));
            table.Columns.Add(new DataColumn("SysSign"));
            table.Columns.Add(new DataColumn("Sign"));
            DataRow row = table.NewRow();
            row["time"] = DateTime.Now;
            row["SysSign"] = syssign;
            row["sign"] = param;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/SMSLog.xml"));
        }

        [ConfigElement("调用地址", Nullable = false)]
        public string ApiUrl { get; set; }

        [ConfigElement("签名编号", Nullable = false)]
        public string Sign { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
    public class XinLKSmsModel
    {
        //Json:{“result”:”1”,”tips”:”成功”}
        //result :1.正常 <1.异常
        //tips ：提示信息
        /// <summary>
        /// 1.正常 小于1.异常
        /// </summary>
        public int result { get; set; }
        public string tips { get; set; }
    }
}
