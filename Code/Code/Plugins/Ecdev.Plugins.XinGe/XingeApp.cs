using Ecdev.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecdev.Plugins.Xinge.Utility;
using Ecdev.Plugins.Integration;
using Ecdev.Plugins.Integration.Xinge;
using System.IO;

//using EcShop.Core.ErrorLog;

namespace Ecdev.Plugins.Xinge
{
    [Plugin("ASP.NET信鸽消息发送组件")]
    public class XingeApp : XingePush
    {
        [ConfigElement("ACCESS ID(Android)", Nullable = false)]
        public string accessId { get; set; }
        [ConfigElement("SECRET KEY(Android)", Nullable = false)]
        public string secretKey { get; set; }

        [ConfigElement("有效时间(Android)", Nullable = false)]
        public uint valid_time { get; set; }
        [ConfigElement("ACCESS ID(iOS)", Nullable = false)]
        public string accessId_IOS { get; set; }
        [ConfigElement("SECRET KEY(iOS)", Nullable = false)]
        public string secretKey_IOS { get; set; }

        [ConfigElement("有效时间(iOS)", Nullable = false)]
        public uint valid_time_IOS { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessId">accessId</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="valid_time">配合timestamp确定请求的有效期，单位为秒，
        /// 最大值为600。若不设置此参数或参数值非法，则按默认值600秒计算有效期</param>
        public XingeApp(string accessId, string secretKey, uint valid_time = 600)
        {
            if (string.IsNullOrEmpty(accessId))
            {
                throw new ArgumentNullException("accessId");
            }
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("secretKey");
            }
            this.valid_time = valid_time;
            this.accessId = accessId;
            this.secretKey = secretKey;
        }

        public XingeApp()
        {
        }

        public XingeApp(string accessId, string secretKey)
        {
            this.accessId = accessId;
            this.secretKey = secretKey;
        }

        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="accessId">accessId</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="valid_time">配合timestamp确定请求的有效期，单位为秒，
        /// 最大值为600。若不设置此参数或参数值非法，则按默认值600秒计算有效期</param>
        /// <returns></returns>
        public override bool SetInitialize(string accessId, string secretKey, uint valid_time = 600)
        {
            if (string.IsNullOrEmpty(accessId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(secretKey))
            {
                return false;
            }
            this.valid_time = valid_time;
            this.accessId = accessId;
            this.secretKey = secretKey;
            return true;
        }
        
        // 详细的api接口
        public override Ret PushSingleDevice(string deviceToken, Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                throw new ArgumentNullException("DeviceToken");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }
            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("multi_pkg", message.MultiPkg.ToString());
            parameters.Add("device_token", deviceToken);
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("message", message.ToJsonString());

            return CallRestful(XingeConfig.RESTAPI_PUSHSINGLEDEVICE, parameters);
        }

        public override Ret PushSingleDevice(string deviceToken, MessageIOS message, int environment)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                throw new ArgumentNullException("DeviceToken");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }
            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("device_token", deviceToken);
            parameters.Add("message_type", 0);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment.ToString());

            if (message.LoopInterval > 0 && message.LoopTimes > 0)
            {
                parameters.Add("loop_interval", message.LoopInterval);
                parameters.Add("loop_times", message.LoopTimes);
            }

            return CallRestful(XingeConfig.RESTAPI_PUSHSINGLEDEVICE, parameters);
        }

        public override Ret PushSingleAccount(int deviceType, string account, Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException("account");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }
            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("multi_pkg", message.MultiPkg);
            parameters.Add("device_type", deviceType);
            parameters.Add("account", account);
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("message", message.ToJsonString());

            return CallRestful(XingeConfig.RESTAPI_PUSHSINGLEACCOUNT, parameters);
        }

        public override Ret PushAccountList(int deviceType, List<string> accountList, Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (accountList.Count <= 0)
            {
                throw new ArgumentNullException("accountList");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }
            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("multi_pkg", message.MultiPkg.ToString());
            parameters.Add("device_type", deviceType);
            parameters.Add("account_list", new JArray(accountList).ToString());
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("message", message.ToJsonString());

            return CallRestful(XingeConfig.RESTAPI_PUSHACCOUNTLIST, parameters);
        }

        public override Ret PushSingleAccount(int deviceType, string account, MessageIOS message, int environment)
        {
            //ErrorLog.Write(string.Format("deviceType: {0}\taccount: {1}\tmessage: {2}\tenvironment: {3}", deviceType, account, message.ToJsonString(), environment));

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException("account");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("device_type", deviceType);
            parameters.Add("account", account);
            parameters.Add("message_type", 0);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment);

            //ErrorLog.Write("C");
            return CallRestful(XingeConfig.RESTAPI_PUSHSINGLEACCOUNT, parameters);
        }

        public override Ret PushAccountList(int deviceType, List<string> accountList, MessageIOS message, int environment)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (accountList.Count <= 0)
            {
                throw new ArgumentNullException("accountList");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("device_type", deviceType);
            parameters.Add("account_list", new JArray(accountList).ToString());
            parameters.Add("message_type", 0);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment);

            return CallRestful(XingeConfig.RESTAPI_PUSHACCOUNTLIST, parameters);
        }

        public override Ret PushAllDevice(int deviceType, Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("multi_pkg", message.MultiPkg.ToString());
            parameters.Add("device_type", deviceType);
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("message", message.ToJsonString());

            if (message.LoopInterval > 0 && message.LoopTimes > 0)
            {
                parameters.Add("loop_interval", message.LoopInterval);
                parameters.Add("loop_times", message.LoopTimes);
            }

            return CallRestful(XingeConfig.RESTAPI_PUSHALLDEVICE, parameters);
        }

        public override Ret PushAllDevice(int deviceType, MessageIOS message, int environment)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("device_type", deviceType);
            parameters.Add("message_type", 0);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment);

            if (message.LoopInterval > 0 && message.LoopTimes > 0)
            {
                parameters.Add("loop_interval", message.LoopInterval);
                parameters.Add("loop_times", message.LoopTimes);
            }

            return CallRestful(XingeConfig.RESTAPI_PUSHALLDEVICE, parameters);
        }

        public override Ret PushTags(int deviceType, List<string> tagList, string tagOp, Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }

            if (!message.IsValid() || tagList.Count == 0 || (!tagOp.Equals("AND") && !tagOp.Equals("OR")))
            {
                return new Ret(-1, "param invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("multi_pkg", message.MultiPkg.ToString());
            parameters.Add("device_type", deviceType);
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("tags_list", new JArray(tagList).ToString());
            parameters.Add("tags_op", tagOp);
            parameters.Add("message", message.ToJsonString());

            if (message.LoopInterval > 0 && message.LoopTimes > 0)
            {
                parameters.Add("loop_interval", message.LoopInterval);
                parameters.Add("loop_times", message.LoopTimes);
            }

            return CallRestful(XingeConfig.RESTAPI_PUSHTAGS, parameters);
        }

        public override Ret PushTags(int deviceType, List<string> tagList, string tagOp, MessageIOS message, int environment)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }

            if (!message.IsValid() || tagList.Count == 0 || (!tagOp.Equals("AND") && !tagOp.Equals("OR")))
            {
                return new Ret(-1, "param invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("send_time", message.SendTime);
            parameters.Add("device_type", deviceType);
            parameters.Add("message_type", 0);
            parameters.Add("tags_list", new JArray(tagList).ToString());
            parameters.Add("tags_op", tagOp);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment);

            if (message.LoopInterval > 0 && message.LoopTimes > 0)
            {
                parameters.Add("loop_interval", message.LoopInterval);
                parameters.Add("loop_times", message.LoopTimes);
            }

            return CallRestful(XingeConfig.RESTAPI_PUSHTAGS, parameters);
        }

        public override Ret CreateMultipush(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message))
            {
                return new Ret(-1, "message type error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("multi_pkg", message.MultiPkg);
            parameters.Add("message_type", (int)message.MessageType);
            parameters.Add("message", message.ToJsonString());

            return CallRestful(XingeConfig.RESTAPI_CREATEMULTIPUSH, parameters);
        }

        public override Ret CreateMultipush(MessageIOS message, int environment)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!ValidateMessageType(message, environment))
            {
                return new Ret(-1, "message type or environment error!");
            }

            if (!message.IsValid())
            {
                return new Ret(-1, "message invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("expire_time", message.ExpireTime);
            parameters.Add("message_type", 0);
            parameters.Add("message", message.ToJsonString());
            parameters.Add("environment", environment);

            return CallRestful(XingeConfig.RESTAPI_CREATEMULTIPUSH, parameters);
        }

        public override Ret PushAccountListMultiple(int pushId, List<string> accountList)
        {
            if (pushId <= 0)
            {
                return new Ret(-1, "pushId invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("push_id", pushId);
            parameters.Add("account_list", new JArray(accountList).ToString());

            return CallRestful(XingeConfig.RESTAPI_PUSHACCOUNTLISTMULTIPLE, parameters);
        }

        public override Ret PushDeviceListMultiple(int pushId, List<string> deviceList)
        {
            if (pushId <= 0)
            {
                return new Ret(-1, "pushId invalid!");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("push_id", pushId);
            parameters.Add("device_list", new JArray(deviceList).ToString());

            return CallRestful(XingeConfig.RESTAPI_PUSHDEVICELISTMULTIPLE, parameters);
        }

        public override Ret QueryPushStatus(List<string> pushIdList)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            JArray jArray = new JArray();
            foreach (string pushId in pushIdList)
            {
                JObject js = new JObject();
                js.Add("push_id", pushId);

                jArray.Add(js);
            }
            parameters.Add("push_ids", jArray.ToString());

            return CallRestful(XingeConfig.RESTAPI_QUERYPUSHSTATUS, parameters);
        }

        public override Ret QueryDeviceCount()
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("access_id", this.accessId);

            return CallRestful(XingeConfig.RESTAPI_QUERYDEVICECOUNT, parameters);
        }

        public override Ret QueryTags(int start, int limit)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("access_id", this.accessId);
            parameters.Add("start", start);
            parameters.Add("limit", limit);

            return CallRestful(XingeConfig.RESTAPI_QUERYTAGS, parameters);
        }

        public override Ret QueryTags()
        {
            return QueryTags(0, 100);
        }

        public override Ret QueryTagTokenNum(string tag)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("tag", tag);

            return CallRestful(XingeConfig.RESTAPI_QUERYTAGTOKENNUM, parameters);
        }

        public override Ret QueryTokenTags(string deviceToken)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("device_token", deviceToken);

            return CallRestful(XingeConfig.RESTAPI_QUERYTOKENTAGS, parameters);
        }

        public override Ret CancelTimingPush(string pushId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("push_id", pushId);

            return CallRestful(XingeConfig.RESTAPI_CANCELTIMINGPUSH, parameters);
        }

        private bool ValidateToken(string token)
        {
            long theAccessId = 0L;

            if (long.TryParse(this.accessId, out theAccessId))
            {
                if (theAccessId >= 2200000000L)
                {
                    return token.Length == 64;
                }
                else
                {
                    return (token.Length == 40 || token.Length == 64);
                }
            }

            return false;
        }

        public override Ret BatchSetTag(List<TagTokenPair> tagTokenPairs)
        {

            foreach (TagTokenPair pair in tagTokenPairs)
            {
                if (!this.ValidateToken(pair.Token))
                {
                    string returnMsgJsonStr = string.Format("invalid token {0}}", pair.Token);
                    return new Ret(-1, returnMsgJsonStr);
                }
            }

            Dictionary<string, Object> parameters = new Dictionary<string, object>();

            List<List<string>> tag_token_list = new List<List<string>>();

            foreach (TagTokenPair pair in tagTokenPairs)
            {
                List<string> singleTagToken = new List<string>();
                singleTagToken.Add(pair.Tag);
                singleTagToken.Add(pair.Token);

                tag_token_list.Add(singleTagToken);
            }

            parameters.Add("tag_token_list", new JArray(tag_token_list).ToString());

            return CallRestful(XingeConfig.RESTAPI_BATCHSETTAG, parameters);
        }

        public override Ret BatchDelTag(List<TagTokenPair> tagTokenPairs)
        {

            foreach (TagTokenPair pair in tagTokenPairs)
            {
                if (!this.ValidateToken(pair.Token))
                {
                    string returnMsgJsonStr = string.Format("invalid token {0}}", pair.Token);
                    return new Ret(-1, returnMsgJsonStr);
                }
            }

            Dictionary<string, Object> parameters = new Dictionary<string, object>();

            List<List<string>> tag_token_list = new List<List<string>>();

            foreach (TagTokenPair pair in tagTokenPairs)
            {
                List<string> singleTagToken = new List<string>();
                singleTagToken.Add(pair.Tag);
                singleTagToken.Add(pair.Token);

                tag_token_list.Add(singleTagToken);
            }

            parameters.Add("tag_token_list", new JArray(tag_token_list).ToString());

            return CallRestful(XingeConfig.RESTAPI_BATCHDELTAG, parameters);
        }

        public override Ret QueryInfoOfToken(string deviceToken)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("device_token", deviceToken);

            return CallRestful(XingeConfig.RESTAPI_QUERYINFOOFTOKEN, parameters);
        }

        public override Ret QueryTokensOfAccount(string account)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("account", account);

            return CallRestful(XingeConfig.RESTAPI_QUERYTOKENSOFACCOUNT, parameters);
        }

        #region Private

        private bool ValidateMessageType(Message message)
        {
            long theAccessId = long.MaxValue;

            if (long.TryParse(this.accessId, out theAccessId))
            {
                if (theAccessId < XingeConfig.IOS_MIN_ID)
                    return true;
                else
                    return false;
            }

            return false;
        }

        private bool ValidateMessageType(MessageIOS message, int environment)
        {

            long theAccessId = long.MaxValue;

            if (long.TryParse(this.accessId_IOS, out theAccessId))
            {
                if (theAccessId >= XingeConfig.IOS_MIN_ID && (environment == XingeConfig.IOSENV_PROD || environment == XingeConfig.IOSENV_DEV))
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 发起推送请求到信鸽并获得相应
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">字段</param>
        /// <returns>返回值json反序列化后的类</returns>
        private Ret CallRestful(String url, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            try
            {
                bool isAndroid = true;

                if (parameters.ContainsKey("environment"))
                {
                    object environment = null;

                    if (parameters.TryGetValue("environment", out environment))
                    {
                        if (environment is int)
                        {
                            int env = Convert.ToInt32(environment);

                            if (env == 1 || env == 2)
                            {
                                isAndroid = false;
                            }
                        }

                        if (environment is string)
                        {
                            int env = 0;

                            if (int.TryParse(environment.ToString(), out env))
                            {
                                if (env == 1 || env == 2)
                                {
                                    isAndroid = false;
                                }
                            }
                        }
                    }
                }

                parameters.Add("access_id", (isAndroid ? accessId : accessId_IOS));
                parameters.Add("timestamp", ((int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString());
                parameters.Add("valid_time", valid_time.ToString());
                string md5sing = SignUtility.GetSignature(parameters, (isAndroid ? this.secretKey : this.secretKey_IOS), url);
                parameters.Add("sign", md5sing);

                var res = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);

                //var resstr = res.GetResponseStream();
                //StreamReader sr = new StreamReader(resstr);
                //var resstring = sr.ReadToEnd();

                Ret ret = new Ret { ret_code = -1, err_msg = "未知错误" };

                using (StreamReader streamReader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                {
                    string resString = streamReader.ReadToEnd();

                    ret = JsonConvert.DeserializeObject<Ret>(resString);
                }

                return ret;
            }
            catch (Exception e)
            {
                return new Ret { ret_code = -1, err_msg = e.Message };
            }
        }

        #endregion

        protected override bool NeedProtect
        {
            get { return true; }
        }

        public override string Logo
        {
            get { return string.Empty; }
        }

        public override string ShortDescription
        {
            get { return string.Empty; }
        }

        public override string Description
        {
            get { return string.Empty; }
        }
    }
}
