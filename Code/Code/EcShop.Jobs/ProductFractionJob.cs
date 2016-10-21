using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using EcShop.Core.Entities;
using EcShop.Core.Jobs;
using EcShop.ControlPanel.Commodities;

namespace EcShop.Jobs
{
    public class ProductFractionJob : IJob
    {
        Database database = DatabaseFactory.CreateDatabase();

        public void Execute(XmlNode node)
        {
            int minutes = 600;
            XmlAttribute xmlAttribute = node.Attributes["minutes"];
            if (xmlAttribute != null)
            {
                int.TryParse(xmlAttribute.Value, out minutes);
            }

            ProcessFraction();
        }

        private void ProcessFraction()
        {
            int pageSize = 50;
            int pageIndex = 1;

            DbQueryResult result = ProductHelper.GetProducts(1, pageIndex, pageSize);

            if (result != null)
            {
                int totalRecords = result.TotalRecords;

                if (totalRecords > 0)
                {
                    int pageCount = totalRecords / pageSize;
                    if (totalRecords % pageSize == 0)
                    {
                        pageCount++;
                    }

                    DataTable dt = result.Data as DataTable;

                    CalcProductFraction(dt);

                    for (pageIndex = 2; pageIndex <= pageCount; pageIndex++)
                    {
                        result = ProductHelper.GetProducts(1, pageIndex, pageSize);
                        dt = result.Data as DataTable;

                        CalcProductFraction(dt);
                    }
                }
            }
        }

        private void CalcProductFraction(DataTable dt)
        {
            if (dt != null)
            {
                List<int> productIdList = new List<int>();

                foreach (DataRow row in dt.Rows)
                {
                    int productId = int.Parse(row["ProductId"].ToString());

                    productIdList.Add(productId);
                }

                CalcProductFraction(productIdList);
            }
        }

        private void CalcProductFraction(List<int> productIdList)
        {
            if (productIdList.Count == 0)
            {
                return;
            }

            try
            {
                string productIds = string.Join<int>(",", productIdList);

                Dictionary<string, decimal> productFractions = ProductHelper.CalculateProductFraction(productIds);

                ProductHelper.UpdateProductFractions(productFractions);
            }
            catch
            { }
        }

        private void CalcFraction(DataRow row)
        {
            /*
            一、商品分值
            商品标题小于等于10 字为1 分大于10 字为1.5 分
            商品图片1 张0.5 分
            商品描述大于500 字符0.5 分大于1000 字符1 分大于2000 字符1.5 分
            商品分值= 商品标题+ 商品图片+ 商品描述
            */
            string productName = row["ProductName"].ToString();
            decimal titleScore = productName.Length < 10 ? 1M : 1.5M;
            decimal imageScore = 0M;
            if (row["ImageUrl1"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl2"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl3"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl4"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl5"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }

            string description = RemoveHTMLTags(row["Description"].ToString());
            string mobileDescription = RemoveHTMLTags(row["MobbileDescription"].ToString());

            int descLength = (description + mobileDescription).Length;
            decimal descScore = 0M;

            if (descLength > 500)
            {
                descScore = 0.5M;
            }
            else if (descLength > 1000)
            {
                descScore = 1M;
            }
            else if (descLength > 2000)
            {
                descScore = 1.5M;
            }

            decimal productScore = titleScore + imageScore + descScore;

            /*
            二、店铺分值（暂无，或在后台配供应商的相关信息）
            1）会员信用指数
            等级点数= 会员信用指数
            2）会员证书
            证书点数= 证书个数
            3）店铺好评率
            店铺分值= 会员信用指数+会员证书+店铺满意率
            */

            /*
            三、活跃度
            点击率、收藏数、交易量、商品新鲜度
            点击率点数= 点击数/1000
            收藏数点数= 收藏数/100
            交易量点数= 交易量/100
            新鲜度点数= (7 天内新商品/全部商品)*100%
            活跃度= 点击率点数+收藏数点数+交易量点数+新鲜度点数
            */

            /*
            四、提升权重值
            此值为管理员提升或降权用的分值
            */

            /*
            权重计算公式：权重值=商品分值+店铺分值+活跃度+提升分数
            */
        }

        private string RemoveHTMLTags(string htmlStream)
        {
            if (htmlStream == null)
            {
                return "";
            }

            /*
             * 最好把所有的特殊HTML标记都找出来，然后把与其相对应的Unicode字符一起影射到Hash表内，最后一起都替换掉
             */
            //先单独测试,成功后,再把所有模式合并
            //注:这两个必须单独处理
            //去掉嵌套了HTML标记的JavaScript:(<script)[\\s\\S]*(</script>)
            //去掉css标记:(<style)[\\s\\S]*(</style>)
            //去掉css标记:\\..*\\{[\\s\\S]*\\}
            htmlStream = Regex.Replace(htmlStream, "(<script)[\\s\\S]*?(</script>)|(<style)[\\s\\S]*?(</style>)", " ", RegexOptions.IgnoreCase);
            //htmlStream = RemoveTag(htmlStream, "script");
            //htmlStream = RemoveTag(htmlStream, "style");
            //去掉普通HTML标记:<[^>]+>
            //替换空格:&nbsp;|&amp;|&shy;|&#160;|&#173;
            htmlStream = Regex.Replace(htmlStream, "<[^>]+>|&nbsp;|&amp;|&shy;|&#160;|&#173;|&bull;|&lt;|&gt;", " ", RegexOptions.IgnoreCase);
            //htmlStream = RemoveTag(htmlStream);
            //替换左尖括号
            //htmlStream = Regex.Replace(htmlStream, "&lt;", "<");
            //替换右尖括号
            //htmlStream = Regex.Replace(htmlStream, "&gt;", ">");
            //替换空行
            //htmlStream = Regex.Replace(htmlStream, "[\n|\r|\t]", " ");//[\n|\r][\t*| *]*[\n|\r]
            htmlStream = Regex.Replace(htmlStream, "(\r\n[\r|\n|\t| ]*\r\n)|(\n[\r|\n|\t| ]*\n)", "\r\n");
            htmlStream = Regex.Replace(htmlStream, "[\t| ]{1,}", " ");

            return htmlStream.Trim();
        }
    }
}
