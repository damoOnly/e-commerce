using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_BrandList : AscxTemplatedWebControl
    {
         public const string TagID = "list_Default_BrandList";
        private System.Web.UI.WebControls.Repeater rp_guest;
        private System.Web.UI.WebControls.Repeater rp_BrandTagguest;
        private int maxNum = 6;
        private int pageIndex = 0;
        private int brandTagId = 0;
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
        public Common_BrandList()
        {
            base.ID = "list_Default_BrandList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common-BrandList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            string index = Context.Request.QueryString["pageIndex"];
            string bidTagId = Context.Request.QueryString["brandTagId"];
            int.TryParse(index, out pageIndex);
            int.TryParse(bidTagId, out brandTagId);
            this.rp_guest = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_Brandguest");
            this.rp_BrandTagguest = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_BrandTagguest");
            if (pageIndex == 0)
                this.BindList();
            else
                BindList(pageIndex);
        }
        private void BindList()
        {
            if (this.rp_guest != null)
            {
                System.Collections.Generic.IList<int> browedProductList = BrandQueue.GetBrowedProductList(this.MaxNum);
                DataTable table = BrandBrowser.GetVistiedBrandList(browedProductList);
                if (table != null)
                {
                    if (table.Rows.Count < this.MaxNum)
                    {
                        DataTable dt = BrandBrowser.GetVistiedBrandList(this.MaxNum - table.Rows.Count, 1, this.MaxNum,this.brandTagId);
                        TableToTable(table, dt);

                    }
                }
                else
                {
                    table = BrandBrowser.GetVistiedBrandList(this.MaxNum, 1, this.MaxNum,brandTagId);
                }
                this.rp_guest.DataSource = table;
                this.rp_guest.DataBind();         
            }
        }
        public DataTable GetBrandTag()
        {
           var tag=BrandQueue.GetBrandTagCache();
           if (tag == null)
           {
               var model= BrandBrowser.GetVistiedBrandTagList();
               BrandQueue.RemoveBrandTagCache();
               BrandQueue.SaveBrandTagCache(model);
               return model;
           }
           else
           {
               return (DataTable)tag;
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
                System.Collections.Generic.IList<int> browedProductList = BrandQueue.GetBrowedProductList((skip * this.maxNum) + this.MaxNum);
                
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
                DataTable table = BrandBrowser.GetVistiedBrandList(browedProductList);
                if (table != null)
                {
                    if (table.Rows.Count < this.maxNum)
                    {
                        DataTable dt = BrandBrowser.GetVistiedBrandList(this.MaxNum - table.Rows.Count, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum,brandTagId);
                        TableToTable(table, dt);
                    }
                }
                else
                {
                    table = BrandBrowser.GetVistiedBrandList(this.MaxNum, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum,brandTagId);
                }

                if (table.Rows.Count < 8)
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
