using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Ecdev.Plugins.Integration;
using Ecdev.Plugins.Integration.Xinge;

namespace Ecdev.Plugins
{
    public abstract class XingePush : ConfigablePlugin, IPlugin
    {
        public static XingePush CreateInstance(string name, string configXml)
        {
            XingePush result;
            if (string.IsNullOrEmpty(name))
            {
                result = null;
            }
            else
            {
                Type plugin = XingePushPlugins.Instance().GetPlugin("XingePush", name);
                if (plugin == null)
                {
                    result = null;
                }
                else
                {
                    XingePush xingePush = Activator.CreateInstance(plugin) as XingePush;
                    if (xingePush != null && !string.IsNullOrEmpty(configXml))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(configXml);
                        xingePush.InitConfig(xmlDocument.FirstChild);
                    }
                    result = xingePush;
                }
            }
            return result;
        }

        public static XingePush CreateInstance(string name)
        {
            return XingePush.CreateInstance(name, null);
        }

        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="accessId">accessId</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="valid_time">配合timestamp确定请求的有效期，单位为秒，
        /// 最大值为600。若不设置此参数或参数值非法，则按默认值600秒计算有效期</param>
        /// <returns></returns>
        public abstract bool SetInitialize(string accessId, string secretKey, uint valid_time = 600);

        // 详细的api接口
        public abstract Ret PushSingleDevice(string deviceToken, Message message);

        public abstract Ret PushSingleDevice(string deviceToken, MessageIOS message, int environment);

        public abstract Ret PushSingleAccount(int deviceType, string account, Message message);

        public abstract Ret PushAccountList(int deviceType, List<string> accountList, Message message);

        public abstract Ret PushSingleAccount(int deviceType, string account, MessageIOS message, int environment);

        public abstract Ret PushAccountList(int deviceType, List<string> accountList, MessageIOS message, int environment);

        public abstract Ret PushAllDevice(int deviceType, Message message);

        public abstract Ret PushAllDevice(int deviceType, MessageIOS message, int environment);

        public abstract Ret PushTags(int deviceType, List<string> tagList, string tagOp, Message message);

        public abstract Ret PushTags(int deviceType, List<string> tagList, string tagOp, MessageIOS message, int environment);

        public abstract Ret CreateMultipush(Message message);

        public abstract Ret CreateMultipush(MessageIOS message, int environment);

        public abstract Ret PushAccountListMultiple(int pushId, List<string> accountList);

        public abstract Ret PushDeviceListMultiple(int pushId, List<string> deviceList);

        public abstract Ret QueryPushStatus(List<string> pushIdList);

        public abstract Ret QueryDeviceCount();

        public abstract Ret QueryTags(int start, int limit);

        public abstract Ret QueryTags();

        public abstract Ret QueryTagTokenNum(string tag);

        public abstract Ret QueryTokenTags(string deviceToken);

        public abstract Ret CancelTimingPush(string pushId);

        public abstract Ret BatchSetTag(List<TagTokenPair> tagTokenPairs);

        public abstract Ret BatchDelTag(List<TagTokenPair> tagTokenPairs);

        public abstract Ret QueryInfoOfToken(string deviceToken);

        public abstract Ret QueryTokensOfAccount(string account);

    }
}
