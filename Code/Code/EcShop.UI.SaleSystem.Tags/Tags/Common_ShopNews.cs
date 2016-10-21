using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.ControlPanel.Comments;
using EcShop.Entities.Comments;
using System.Data;

namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_ShopNews : AscxTemplatedWebControl
    {
        private Repeater recordshopNews;
        private int maxCNum = 8;
        public int MaxCNum
        {
            get
            {
                return this.maxCNum;
            }
            set
            {
                this.maxCNum = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-ShopNews.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.recordshopNews = (Repeater)this.FindControl("recordshopNews");
            DataTable list = NoticeHelper.GetAffiches(this.maxCNum);
            if (list != null && list.Rows.Count > 0)
            {
                this.recordshopNews.DataSource = list;
                this.recordshopNews.DataBind();
            }
        }
    }
}
