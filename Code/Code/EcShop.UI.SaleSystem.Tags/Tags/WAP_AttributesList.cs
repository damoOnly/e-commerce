using EcShop.ControlPanel.Commodities;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class WAP_AttributesList : WebControl
    {
        private int categoryId;

        protected override void OnInit(EventArgs e)
        {
            int.TryParse(this.Context.Request.QueryString["categoryId"], out this.categoryId);

            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat(" <div class=\"brush-list\"  id=\"{0}\">", this.ID);
            //原产地列表
            IList<ImportSourceTypeInfo> importSourceTypelist = ImportSourceTypeHelper.GetAllImportSourceTypes();

            stringBuilder.Append(" <ol class=\"fix\"><h4>地区</h4><div class=\"fix\"><a class=\"selected\">不限</a>");

             if (importSourceTypelist != null)
             {
                 foreach (ImportSourceTypeInfo item in importSourceTypelist)
                 {
                   stringBuilder.AppendFormat("<a name=\"{0}\">{1}</a>",item.ImportSourceId.ToString(),item.CnArea);
                 }
             }

             stringBuilder.AppendFormat("</div></ol>");

            //根据商品分类获取品牌
            DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, 1000);

            stringBuilder.Append(" <ol class=\"fix\"><h4>品牌</h4><div class=\"fix\"><a class=\"selected\">不限</a>");

            if (brandCategories != null && brandCategories.Rows.Count > 0)
            {
                 foreach (DataRow dataRow in brandCategories.Rows)
                 {
                     stringBuilder.AppendFormat("<a name=\"{0}\">{1}</a>", dataRow["BrandId"].ToString(),  dataRow["BrandName"]);
                 }
            }

            stringBuilder.AppendFormat("</div></ol>");

            stringBuilder.Append("</div>");

            writer.Write(stringBuilder.ToString());
        }
    }
}
