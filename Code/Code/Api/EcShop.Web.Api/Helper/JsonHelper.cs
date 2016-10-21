using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EcShop.Web.Api.Helper
{
    public static class JsonHelper
    {

        /// <summary>
        /// 将Json数据转换成对象
        /// </summary>
        /// <typeparam name="T">要转换的对象类型</typeparam>
        /// <param name="jsonString">Json数据</param>
        /// <returns>转换后的对象</returns>
        public static T ToObject<T>(this string jsonString)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                var setting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                return JsonConvert.DeserializeObject<T>(jsonString, setting);
            }

            return default(T);
        }

        /// <summary>
        /// 将json字符串转化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <param name="isIgnoreMissingMemberHandling">是否忽略处理丢失字段</param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonString, bool isIgnoreMissingMemberHandling)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                var setting = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                if (isIgnoreMissingMemberHandling)
                {
                    setting.MissingMemberHandling = MissingMemberHandling.Ignore;
                }
                else
                {
                    setting.MissingMemberHandling = MissingMemberHandling.Error;
                }

                return JsonConvert.DeserializeObject<T>(jsonString, setting);
            }

            return default(T);
        }

        /// <summary>
        /// 用Newtonsoft.Json将对象转换成Json数据
        /// </summary>
        /// <param name="data">要转换的对象</param>
        /// <returns>Json数据</returns>
        public static string ToJsonString(this object data)
        {
            string json = "{}";

            if (data != null)
            {
                var setting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };


                json = JsonConvert.SerializeObject(data, Formatting.None, setting);
            }

            return json;

        }

        /// <summary>
        /// 用Newtonsoft.Json将对象转换成Json数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isIgnoreDefaultValue">是否忽略默认的值，比如Null字段就不转化到字符串中</param>
        /// <returns></returns>
        public static string ToJsonString(this object data, bool isIgnoreDefaultValue)
        {
            string json = "{}";

            if (data != null)
            {
                var setting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                if (isIgnoreDefaultValue)
                {
                    setting.DefaultValueHandling = DefaultValueHandling.Ignore;
                }
                else
                {
                    setting.DefaultValueHandling = DefaultValueHandling.Include;
                }


                json = JsonConvert.SerializeObject(data, Formatting.None, setting);
            }

            return json;

        }
    }
}