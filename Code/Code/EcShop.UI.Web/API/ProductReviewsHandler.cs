using EcShop.ControlPanel.Comments;
using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EcShop.UI.Web.API
{
    public class ProductReviewsHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            string action = context.Request["action"];
            switch (action)
            {
                case "GetProductReviews":
                    this.GetProductReviews(context);
                    break;


                case "LoadMoreProductReviews":
                    this.LoadMoreProductReviews(context);
                    break;
            }
        }

        public void GetProductReviews(System.Web.HttpContext context)
        {
            string producId = context.Request["ProductId"];
            int Id=0;
            int.TryParse(producId,out Id);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            if (!int.TryParse(producId,out Id))
            {
                stringBuilder.Append("\"Success\":-1");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
                return;
            }

            //处理平均数和总数
            DataTable dt = ProductCommentHelper.GetReviewsCountAndAvg(Id);
            if(dt.Rows.Count>0)
            {
                ProductReviewQuery productReviewQuery = new ProductReviewQuery();
                productReviewQuery.SortBy = "ReviewDate";
                productReviewQuery.SortOrder = SortAction.Desc;
                productReviewQuery.PageIndex = int.Parse(context.Request["pageNumber"]);
                productReviewQuery.PageSize = int.Parse(context.Request["size"]);
                productReviewQuery.productId = int.Parse(context.Request["ProductId"]);

                Globals.EntityCoding(productReviewQuery, true);
                DataTable dtcomments = (DataTable)ProductCommentHelper.GetProductReviews(productReviewQuery).Data;
               
                if (dtcomments.Rows.Count <= 0)
                {
                    stringBuilder.Append("\"Success\":0");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    context.Response.End();
                    return;
                }
                else
                {
                    int amount = 0;
                    try
                    {
                        int.TryParse(dt.Rows[0]["Amount"].ToString(), out amount);

                    }
                    catch
                    {
                        amount = 0;
                    }
                    stringBuilder.Append("\"Success\":1,");
                    stringBuilder.AppendFormat("\"Amount\":{0},", amount);
                    stringBuilder.Append("\"data\":");
                    for (int i = 0; i < dtcomments.Rows.Count; i++)
                    {
                        dtcomments.Rows[i]["UserName"] = ReplaceStr(dtcomments.Rows[i]["UserName"].ToString());
                    }

                    IsoDateTimeConverter convert = new IsoDateTimeConverter();
                    convert.DateTimeFormat = "yyyy-MM-dd";
                    string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(dtcomments, Formatting.None, convert);
                    stringBuilder.Append(strReviews);

                    stringBuilder.Append(",");
                    //stringBuilder.AppendFormat("\"Avg\":{0}", Math.Round((1.0 * count / list.Count), 1));
                    double avg=0;
                    try{
                       double.TryParse(dt.Rows[0]["avg"].ToString(),out avg);
                    
                    }
                    catch
                    {
                        avg=0;
                    }

                    stringBuilder.AppendFormat("\"Avg\":{0}", avg.ToString("0.0"));
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    context.Response.End();
                    return;
                }
            }

            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
                return;
            }
            
        }

        public void LoadMoreProductReviews(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ProductReviewQuery productReviewQuery = new ProductReviewQuery();

            productReviewQuery.SortBy = "ReviewDate";
            productReviewQuery.SortOrder = SortAction.Desc;
            productReviewQuery.PageIndex = int.Parse(context.Request["pageNumber"]);
            productReviewQuery.PageSize = int.Parse(context.Request["size"]);
            productReviewQuery.productId = int.Parse(context.Request["ProductId"]);

            Globals.EntityCoding(productReviewQuery, true);
            DataTable dt = (DataTable)ProductCommentHelper.GetProductReviews(productReviewQuery).Data;

            stringBuilder.Append("{");
            if (dt.Rows.Count > 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"productcomments\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strReviews = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None, convert);
                stringBuilder.Append(strReviews);
                stringBuilder.Append("}");
            }

            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        private string ReplaceStr(string str)
        {
            string strStart = str.Substring(0, 1);
            string strbody = str.Substring(1, str.Length - 1).Replace(str.Substring(1, str.Length - 1), "*****");
            string strEnd = str.Substring(str.Length - 1);
            return strStart + strbody + strEnd;
            //string patten = @"(?<=\S)\S(?=\S)";
            //Regex reg = new Regex(patten);
            //str = reg.Replace(str, "*");
            //return str;
        }
    }
}
