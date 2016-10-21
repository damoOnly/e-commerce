using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EcShop.SaleSystem.Vshop;
using EcShop.Entities;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_PCHotProductList : AscxTemplatedWebControl
    {
        public const string TagID = "list_Common_Common_PCHotProductList";
        private System.Web.UI.WebControls.Repeater rptHotSale;
        private int maxNum = 5;
        private int pageIndex = 0;
        private int supplierId = 0;
        private System.Web.UI.HtmlControls.HtmlControl div;
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
        public Common_PCHotProductList()
        {
            base.ID = "list_Common_Common_PCHotProductList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common-PCHotProductList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["supplierId"], out this.supplierId);
            this.rptHotSale = (System.Web.UI.WebControls.Repeater)this.FindControl("rptHotSale");
            this.div = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("showhotsale");
            string index = Context.Request.QueryString["pageIndex"];
            int.TryParse(index, out pageIndex);
            if (pageIndex == 0)
                this.BindList();
            else
                BindList(pageIndex);
        }

        private void BindList()
        {
            if (this.rptHotSale != null)
            {
                DataTable dt = VshopBrowser.GetAllHotSaleNomarl(this.MaxNum, 1, this.MaxNum, supplierId, ClientType.PC);
                rptHotSale.DataSource =  dt;
                rptHotSale.DataBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.div.Style.Add("display", "block");
                }
                else
                {
                    this.div.Style.Add("display", "none");
                }
            }
        }

        private void BindList(int skip)
        {
            if (this.rptHotSale != null)
            {
                DataTable dt = VshopBrowser.GetAllHotSaleNomarl(this.MaxNum, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum, supplierId, ClientType.PC);
                rptHotSale.DataSource = dt;
                rptHotSale.DataBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.div.Style.Add("display", "block");
                }
                else
                {
                    this.div.Style.Add("display", "none");
                }
            }
        }

    }
}
