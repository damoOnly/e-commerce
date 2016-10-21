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
using EcShop.SqlDal.Active;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class Common_ActiveProductList : AscxTemplatedWebControl
    {
        public const string TagID = "list_Common_Common_ActiveProductList";
        private System.Web.UI.WebControls.Repeater rptHotSale;
        private int maxNum = 12;
        private int pageIndex = 0;
        private int supplierId = 0;
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
        public Common_ActiveProductList()
        {
            base.ID = "list_Common_Common_ActiveProductList";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common-PCActiveProductList.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
       
            this.rptHotSale = (System.Web.UI.WebControls.Repeater)this.FindControl("rptHotSale");
            string index = Context.Request.QueryString["pageIndex"];
            int.TryParse(index, out pageIndex);
            this.BindList();
        }

        private void BindList()
        {
            if (this.rptHotSale != null)
            {
                rptHotSale.DataSource =new PCActiveDao().GetThisTopicList();
                rptHotSale.DataBind();
            }
        }


    }
}
