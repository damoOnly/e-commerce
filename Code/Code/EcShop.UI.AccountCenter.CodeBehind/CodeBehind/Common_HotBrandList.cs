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
    public class Common_HotBrandList : AscxTemplatedWebControl
    {
          public const string TagID = "list_HotBrandList";
          private System.Web.UI.WebControls.Repeater rpHotBrandlist;
        private int maxNum = 4;
        private int pageIndex = 0;
        private int brandTagId = 5;
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
        public Common_HotBrandList()
        {
            base.ID = "list_HotBrandList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_HotBrandList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            string index = Context.Request.QueryString["pageIndex"];
            string bidTagId = Context.Request.QueryString["brandTagId"];
            int.TryParse(index, out pageIndex);
            int.TryParse(bidTagId, out brandTagId);
            this.rpHotBrandlist = (System.Web.UI.WebControls.Repeater)this.FindControl("rpHotBrandlist");
            if (pageIndex == 0)
                this.BindList(0);
            else
                BindList(pageIndex);
         
        }
        private void BindList(int skip)
        {
            if (this.rpHotBrandlist != null)
            {
                DataTable dt = null;
                if (skip > 0)
                {
                    dt = BrandBrowser.GetVistiedBrandList(this.MaxNum, skip * this.maxNum + 1, skip * this.maxNum + 1 + this.MaxNum, brandTagId);
                }
                else
                {
                    dt = BrandBrowser.GetVistiedBrandList(this.MaxNum, 1, this.MaxNum, brandTagId);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    int num = dt.Rows.Count - 1;
                    for (int i = num; i >= 0; i--)
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["BigLogo"].ToString()))
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                }
                
                rpHotBrandlist.DataSource = dt;
                rpHotBrandlist.DataBind();
            }
        }

    }
}
