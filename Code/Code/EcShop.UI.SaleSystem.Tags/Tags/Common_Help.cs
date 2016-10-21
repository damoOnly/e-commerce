using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Comments;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_Help : WebControl   //海美生活脚部帮助信息
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string strHelps = HiCache.Get("DataCache-Helps") as string;
            if (string.IsNullOrEmpty(strHelps))
            {
                DataTable dtHelps = CommentBrowser.GetFooterHelps();
                IList<HelpCategoryInfo> helpCategorys = CommentBrowser.GetHelpCategorys();
                StringBuilder stringBuiler = new StringBuilder();
                stringBuiler.Append("<ul class=\"g-cf\">");

                if (dtHelps != null && dtHelps.Rows.Count > 0)
                {
                    int count = dtHelps.Rows.Count;
                  
                        for (int j = 0; j < 4; j++)
                        {
                            stringBuiler.AppendFormat("<li class=\"rookie\">");
                            stringBuiler.Append(" <h3>");
                            stringBuiler.AppendFormat(" <span class=\"g-dib\"><img src=\"{0}\"/></span>{1}", helpCategorys[j].IconUrl, helpCategorys[j].Name);
                            stringBuiler.Append("</h3>");
                            stringBuiler.Append("<div>");
                            //int z = 0;
                            for (int i = 0; i < count; i++)
                            {
                                
                                if(dtHelps.Rows[i]["CategoryId"].ToString()==(helpCategorys[j].CategoryId.ToString()))
                                {
                                    //z++;
                                    //stringBuiler.AppendFormat("<a href=\"{0}\">{1}</a>", Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails", dtHelps.Rows[i]["HelpId"]), dtHelps.Rows[i]["Title"]);

                                    stringBuiler.AppendFormat("<a href=\"{0}\">{1}</a>", ResolveUrl("/helpItem.aspx?helpid=" + dtHelps.Rows[i]["HelpId"]), dtHelps.Rows[i]["Title"]);
                                    //if(z%3==0)
                                    //{
                                    //    stringBuiler.Append("<br>");
                                    //}
                                }
                            }

                            stringBuiler.Append("</div>");
                            stringBuiler.Append("</li>");
                        }
                 
                    //for (int i = 0; i < count; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        categoryId = dtHelps.Rows[i]["CategoryId"];
                    //    }
                    //    if (categoryId != dtHelps.Rows[i]["CategoryId"])
                    //    {
                    //        if (i != 0)
                    //        {
                    //            stringBuiler.Append("</div>");
                    //            stringBuiler.Append("</li>");
                    //        }
                    //        if (i == 0)
                    //        {
                    //            stringBuiler.AppendFormat("<li class=\"rookie\">");
                    //        }
                    //        else if (i == 1)
                    //        {
                    //            stringBuiler.AppendFormat("<li class=\"buyers\">");
                    //        }
                    //        else if (i == 2)
                    //        {
                    //            stringBuiler.AppendFormat("<li class=\"clause\">");
                    //        }
                    //        else if (i == 3)
                    //        {
                    //            stringBuiler.AppendFormat("<li class=\"safe\">");
                    //        }
                    //        else if (i == 4)
                    //        {
                    //            stringBuiler.AppendFormat("<li class=\"wechat\">");
                    //        }
                    //        stringBuiler.Append(" <h3>");
                    //        stringBuiler.AppendFormat(" <span class=\"g-dib\"><img src=\"{0}\"/></span>{1}", dtHelps.Rows[i]["IconUrl"], dtHelps.Rows[i]["Name"]);
                    //        stringBuiler.Append("</h3>");
                    //        stringBuiler.Append("<div>");
                    //    }
                    //    stringBuiler.AppendFormat("<a href=\"{0}\">{1}</a>",Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails",dtHelps.Rows[i]["HelpId"]), dtHelps.Rows[i]["Title"]);
                    //     ;
                    //    if ((count - 1) == i)
                    //    {
                    //        stringBuiler.Append("</div>");
                    //        stringBuiler.Append("</li>");
                    //    }
                    //}
                }

                stringBuiler.Append("<li class=\"wechat\"><h3>微信公众号</h3><div><img align=\"left\" src=\"/templates/master/haimei/images/qc.jpg\" width=\"75\" height=\"75\"> 微信公众号<br>海美生活</div><div style='clear:both'><a href='http://weibo.com/haimylife?refer_flag=1005050010_&is_hot=1' target='_blank' rel='no-follow'><img src=\"/templates/master/haimei/images/weibo.png\" style='width:70px;margin-left:0px;height:auto;'></a></div></li>");//固定
                stringBuiler.Append("</ul>");

                strHelps = stringBuiler.ToString();
            }
            writer.Write(strHelps);
        }
    }
}

