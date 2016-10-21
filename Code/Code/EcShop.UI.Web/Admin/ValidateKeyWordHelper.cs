using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Admin
{
    public class ValidateKeyWordHelper
    {
        /// <summary>
        /// 验证是否存在广告关键字
        /// </summary>
        /// <param name="dic">要检测的关键字集合 key：验证的内容名称 val:验证的内容</param>
        /// <param name="msg">如有广告关键字 key:广告关键字名称 val:存在的关键字</param>
        /// <returns></returns>
        public static bool ValidateKeyWord(Dictionary<string, string> dic, out Dictionary<string, string> msg)
        {
            msg = null;
            if (dic == null)
            {
                msg = null;
                return true;
            }
            msg = new Dictionary<string, string>();
            foreach (string key in dic.Keys)
            {
                foreach (string v in list)
                {
                    if (dic[key].Contains(v))
                    {
                        if (msg.ContainsKey(key))
                        {
                            msg[key] += "," + v + "";
                        }
                        else
                        {
                            msg.Add(key, ""+v+"");
                        }
                        
                    }
                }
            }
            if (msg.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static List<string> list = new List<string>(){
            "国家级","世界级","最高级","最佳","最大","第一","唯一","首个","首选","最好","最大","精确","顶级","最高","最低","最","最具"
            ,"最便宜","最新","最先进","最大程度","最新技术","最先进科学","国家级商品","填补国内空白","绝对","独家","首家","最新","最先进"
            ,"第一品牌","金牌","名牌","优秀","最先","顶级","独家","全网销量第一","全球首发","全国首家","全网首发","世界领先","顶级工艺"
            ,"最新科学","最新技术","最先进加工工艺","最时尚","极品","顶级","顶尖","终极","最受欢迎","王牌","销量冠军","NO.1","Top1"
            ,"极致","永久","王牌","掌门人","领袖品牌","独一无二","独家","绝无仅有","前无古人","史无前例","万能","盛大"
        };
    }
}
