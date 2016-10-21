using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    /// <summary>
    /// Ö÷Ò³²ÂÄãÏ²»¶¿Ø¼þ
    /// </summary>
    public class Common_Default_ProductList : AscxTemplatedWebControl
    {
        public const string TagID = "list_Default_ProductList";
        private System.Web.UI.WebControls.Repeater rp_guest;
        private int maxNum = 6;
        private int pageIndex = 0;
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
        public Common_Default_ProductList()
        {
            base.ID = "list_Default_ProductList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Default_ProductList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            string index = Context.Request.QueryString["pageIndex"];
            int.TryParse(index, out pageIndex);
            this.rp_guest = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_guest");
            if (pageIndex == 0)
                this.BindList();
            else
                BindList(pageIndex);
        }
        private void BindList()
        {
            if (this.rp_guest != null)
            {
                System.Collections.Generic.IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList(this.MaxNum);
                DataTable table = ProductBrowser.GetVistiedProducts(browedProductList);
                if (table != null)
                {
                    if (table.Rows.Count < this.MaxNum)
                    {
                        DataTable dt = ProductBrowser.GetVistiedProducts(this.MaxNum - table.Rows.Count, 1, this.MaxNum);
                        TableToTable(table, dt);

                    }
                }
                else
                {
                    table = ProductBrowser.GetVistiedProducts(this.MaxNum, 1, this.MaxNum);
                }
                this.rp_guest.DataSource = table;
                this.rp_guest.DataBind();
            }
        }
        public void TableToTable(DataTable table, DataTable table1)
        {
            List<string> cloName = new List<string>();
            foreach (DataColumn c in table.Columns)
            {
                cloName.Add(c.ColumnName);
            }
            foreach (DataRow r in table1.Rows)
            {
                DataRow row = table.NewRow();
                foreach (string c in cloName)
                {
                    try
                    {
                        row[c] = r[c];
                    }
                    catch
                    {
                    }
                }
                table.Rows.Add(row);
            }
        }
        private void BindList(int skip)
        {
            if (this.rp_guest != null)
            {
                System.Collections.Generic.IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList((skip * this.maxNum) + this.MaxNum);
                
                int browedListCount = browedProductList.Count;
                if (browedListCount > (skip * this.maxNum) + this.MaxNum)
                {
                    int isRemove = browedListCount - this.maxNum;
                    while (isRemove > 0)
                    {
                        isRemove--;
                        browedProductList.RemoveAt(0);
                    }
                }
                else
                {
                    browedProductList.Clear();
                }
                //DataTable table = ProductBrowser.GetSuggestProductsProducts(browedProductList, 6);
                DataTable table = ProductBrowser.GetVistiedProducts(browedProductList);
                if (table != null)
                {
                    if (table.Rows.Count < this.maxNum)
                    {
                        DataTable dt = ProductBrowser.GetVistiedProducts(this.MaxNum - table.Rows.Count, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum);
                        TableToTable(table, dt);
                    }
                }
                else
                {
                    table = ProductBrowser.GetVistiedProducts(this.MaxNum, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum);
                }

                if (table.Rows.Count < 5)
                {
                    this.rp_guest.DataSource = null;
                    this.rp_guest.DataBind();
                    return;
                }
                this.rp_guest.DataSource = table;
                this.rp_guest.DataBind();
            }
        }
    }
}
