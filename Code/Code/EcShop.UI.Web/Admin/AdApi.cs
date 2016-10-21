using EcShop.ControlPanel.Sales;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class AdApi : Page
    {
        private int cid;
        private long orderStartTime=0;
        private long orderEndTime = 0;
        private long UpdateStartTime = 0;
        private long EndUpdateTime = 0;
        private DateTime start;
        private DateTime end;
        protected void Page_Load(object sender, System.EventArgs e)
        {
             if(!this.IsPostBack)
             {
                 SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
                 int type =-1;

                 #region 参数验证
                 if (Request.QueryString["cid"] != null)
                 {
                     int.TryParse(Request.QueryString["cid"].ToString(), out cid);
                 }
                 if(cid<=0)
                 {
                     Response.Write("paramter is not the numeric!");
                     return;
                 }
                 dic.Add("cid", this.cid.ToString());
                 if(Request.QueryString["orderStartTime"]!=null)
                 {
                     long.TryParse(Request.QueryString["orderStartTime"].ToString(),out this.orderStartTime );
                 }
                 if (Request.QueryString["updateStartTime "] != null)
                 {
                     long.TryParse(Request.QueryString["updateStartTime "].ToString(), out this.UpdateStartTime);
                 }
                 #endregion 

                 #region  根据新增时间查询
                 if (this.orderStartTime > 0 && this.UpdateStartTime==0)//根据新增时间查询
                 {
                     if (Request.QueryString["orderEndTime"] == null)
                     {
                         Response.Write("参数数错误！");
                         return;
                     }
                     long.TryParse(Request.QueryString["orderEndTime"].ToString(), out orderEndTime);

                     if (this.orderEndTime <= 0)
                     {
                         Response.Write("paramter is not the numeric!");
                         return;
                     }
                     start= StampToDateTime(this.orderStartTime.ToString());
                     end=StampToDateTime(this.orderEndTime.ToString());
                     dic.Add("orderStartTime", this.orderStartTime.ToString());
                     dic.Add("orderEndTime", this.orderEndTime.ToString());
                     type = 0;
                 }
                 #endregion 

                 #region 根据修改时间查询
                 if (this.orderStartTime ==0 && this.UpdateStartTime >0) //根据修改时间查询
                 {
                     if (Request.QueryString["updateEndTime"] == null)
                     {
                         Response.Write("paramter is not the numeric!");
                         return;
                     }
                     long.TryParse(Request.QueryString["updateEndTime"].ToString(), out  this.EndUpdateTime);
                     if (this.EndUpdateTime<=0)
                     {
                         Response.Write("paramter is not the numeric!");
                         return;
                     }
                     start = StampToDateTime(this.UpdateStartTime.ToString());
                     end = StampToDateTime(this.EndUpdateTime.ToString());
                     type = 1;
                     dic.Add("updateStartTime", this.UpdateStartTime.ToString());
                     dic.Add("updateEndTime", this.EndUpdateTime.ToString());
                 }
                 #endregion 

                 #region  秘钥验证
                 if (Request.QueryString["mid"] == null)
                 {
                     Response.Write("paramter is not the numeric!");
                     return;
                 }
                 string kegSign = Request.QueryString["mid"].ToString();
                 string Sing = GetSing(dic);
                 if (!kegSign.ToLower().Equals(Sing.ToLower()))
                 {
                     Response.Write("sign is error!");
                     return;
                 }
                 #endregion 

                 #region 是否进入参数
                 if (type<0)
                 {
                     Response.Write("paramter is not the numeric!");
                     return;
                 }
                 #endregion 

                 #region 返回数据
                 try
                 {

                     DataTable dt = OrderHelper.SelectAdOrderInfo(cid, start, end, 0);
                    
                     #region 查询订单信息
                     if (type==0)
                     {
                         List<orders> Nlist = new List<orders>();
                         AdOrderInfo ors = null;
                         if (dt != null && dt.Rows.Count > 0)
                         {
                             foreach (DataRow row in dt.Rows)
                             {
                                 string Jsonstr = row["JsonStr"].ToString();
                                 ors = Newtonsoft.Json.JsonConvert.DeserializeObject<AdOrderInfo>(Jsonstr);
                                 Nlist.Add(ors.orders[0]);
                             }
                         }
                         if (Nlist.Count > 0)
                         {
                             Response.Write("{\"orders\":"+Newtonsoft.Json.JsonConvert.SerializeObject(Nlist)+"}");
                             return;
                         }
                         else
                         {
                             Response.Write("no data!");
                         }
                     }
                     #endregion 

                     #region 查询订单修改信息
                     if (type==1)
                     {    
                         AdOrerStatus adstatus=new AdOrerStatus();
                         List<orderStatus> stlist = new List<orderStatus>();
                         if (dt != null && dt.Rows.Count > 0)
                         {
                             orderStatus ords = null;
                             foreach (DataRow row in dt.Rows)
                             {
                                 ords = new orderStatus();
                                 ords.orderstatus = row["OrderStatus"].ToString();
                                 ords.paymentType = row["PaymentType"].ToString();
                                 ords.paymentStatus = row["paymentStatus"].ToString();
                                 ords.updateTime    = row["UpdateDate"].ToString();
                                 ords.feedback      = row["feedback"].ToString();
                                 ords.orderNo       = row["OrderNo"].ToString();
                                 stlist.Add(ords);
                             }  
                         }
                         adstatus.orderStatus = stlist;
                         if (stlist.Count > 0)
                         {
                             Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(adstatus));
                             return;
                         }
                         else
                         {
                             Response.Write("no data!");
                         }
                     }
                     #endregion
                 }
                 catch (Exception ee)
                 {
                     Response.Write("request time out !"+ee.Message);
                 }
                 #endregion 
             }
        }

        /// <summary>
        /// 获取秘钥
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string GetSing(SortedDictionary<string, string> dic)
        {
            StringBuilder strb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in dic)
            {
                strb.Append(kv.Key + "=" + kv.Value + "&");
            }
            string keyStr = strb.ToString().Substring(0, strb.ToString().Length-1);
            return GetMD5(keyStr);
        }

        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp(DateTime times)
        {
            TimeSpan ts = times - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        } 
        // 时间戳转为C#格式时间
        private DateTime StampToDateTime(string timeStamp)
        {
          DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
          long lTime = long.Parse(timeStamp + "0000000");
          TimeSpan toNow = new TimeSpan(lTime);
          return dateTimeStart.Add(toNow);
        }
        public static string GetMD5(string input)
        {
            byte[] result = Encoding.Default.GetBytes(input);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
           return  BitConverter.ToString(output).Replace("-", "");
        }
    }
}