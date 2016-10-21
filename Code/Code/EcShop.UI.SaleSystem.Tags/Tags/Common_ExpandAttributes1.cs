using EcShop.SaleSystem.Catalog;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_ExpandAttributes1 : WebControl   //海美生活新版本使用
    {
        public DataTable DbAttribute
        {
            get;
            set;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" class=\"com-table\">");
            stringBuilder.Append("<tbody>");
            if (DbAttribute != null && DbAttribute.Rows.Count > 0)
            {
                int count = DbAttribute.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i == 0)
                    {
                        stringBuilder.Append("<tr>");
                    }
                    stringBuilder.AppendFormat("<td>{0}：{1}</td>", DbAttribute.Rows[i]["AttributeName"], DbAttribute.Rows[i]["ValueStr"]);
                    if (i!=0&&(i+1) % 3 == 0)
                    {
                        stringBuilder.Append("</tr>");
                        if ((i + 1)< count)
                        {
                            stringBuilder.Append("<tr>");
                        }
                    }
                }
                if (count % 3 != 0)
                {
                    int otherElement = 3-count %3;
                    for (int i = 0; i < otherElement; i++)
                    {
                         stringBuilder.Append("<td></td>");
                    }
                    stringBuilder.Append("</tr>");
                }
            }
            stringBuilder.Append("</tbody>");
            stringBuilder.Append("</table>");
            writer.Write(stringBuilder.ToString());
        }
    }
}
