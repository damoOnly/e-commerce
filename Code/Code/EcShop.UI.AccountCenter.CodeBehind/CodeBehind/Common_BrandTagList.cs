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
    public class Common_BrandTagList : AscxTemplatedWebControl
    {
          public const string TagID = "list_Default_BrandTagList";
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
        public Common_BrandTagList()
        {
            base.ID = "list_Default_BrandTagList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common-BrandTagList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.rp_BrandTagguest = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_BrandTagguest");
          
            this.BindList();
         
        }
        private void BindList()
        {
            if (this.rp_BrandTagguest != null)
            {                
                rp_BrandTagguest.DataSource = GetBrandTag();
                rp_BrandTagguest.DataBind();
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

    }
}
